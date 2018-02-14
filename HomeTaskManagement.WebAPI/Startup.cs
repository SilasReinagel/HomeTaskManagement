using HomeTaskManagement.App.Accounting;
using HomeTaskManagement.App.Common;
using HomeTaskManagement.App.Pledge;
using HomeTaskManagement.App.ServiceJobs;
using HomeTaskManagement.App.Shopping;
using HomeTaskManagement.App.Task;
using HomeTaskManagement.App.Task.Assignment;
using HomeTaskManagement.App.Task.Instance;
using HomeTaskManagement.App.User;
using HomeTaskManagement.Sql;
using HomeTaskManagement.Sql.BlobStore;
using HomeTaskManagement.Sql.EventStore;
using HomeTaskManagement.Sql.Tasks;
using HomeTaskManagement.Sql.Users;
using HomeTaskManagement.WebAPI.Auth;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HomeTaskManagement.App.Requests;

namespace HomeTaskManagement.WebAPI
{
    public sealed class Startup
    {
        public static void Main(string[] args) => WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().Build().Run();

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                services.AddCors();
                services.AddMvc();

                var sqlDb = new SqlDatabase(new EnvironmentVariable("HomeTaskManagementSqlConnection"));
                services.AddSingleton(new MiniAuth(new EnvironmentVariable("HomeTaskManagementSecret")));
                services.AddSingleton(new AppHealth(TimeSpan.FromMinutes(15), sqlDb));
                var eventStore = new SqlEventStore(sqlDb, "HomeTask.Events");
                var blobStore = new SqlBlobStore(sqlDb, "HomeTask.Blobs");

                var messages = new Messages();
                var users = new Users(new UsersTable(sqlDb));
                services.AddSingleton(users);
                var tasks = new Tasks(new TasksTable(sqlDb));
                var accounts = new Accounts(eventStore);
                services.AddSingleton(accounts);
                var pledges = new Pledges(eventStore, users, accounts, new PledgeFundingSettings());
                var assignments = new TaskAssignments(eventStore, tasks, users, new AssignmentSettings());
                var taskInstances = new TaskInstances(new InMemoryTaskInstanceStore(), assignments, messages);
                var treasury = new Treasury(blobStore, accounts);

                services.AddSingleton(users);

                services.AddSingleton(new AppCommands(new Dictionary<string, IRequest>(StringComparer.InvariantCultureIgnoreCase)
                {
                    { nameof(OpenAccount), new AdminOnly(new JsonRequest<OpenAccount>(x => accounts.Apply(x))) },
                    { nameof(SetOverdraftPolicy), new AdminOnly(new JsonRequest<SetOverdraftPolicy>(x => accounts.Apply(x))) },
                    { nameof(TransactionRequest), new AdminOnly(new JsonRequest<TransactionRequest>(x => accounts.Apply(x))) },

                    { nameof(RegisterUser), new JsonRequest<RegisterUser>(x => users.Apply(x)) },
                    { nameof(UnregisterUser), new JsonRequest<UnregisterUser>(x => users.Apply(x)) },
                    { nameof(AddRoles), new AdminOnly(new JsonRequest<AddRoles>(x => users.Apply(x))) },
                    { nameof(RemoveRoles), new AdminOnly(new JsonRequest<RemoveRoles>(x => users.Apply(x))) },

                    { nameof(AssignTask), new AdminOnly(new JsonRequest<AssignTask>(x => assignments.Apply(x))) },

                    { nameof(CreateTask), new AdminOnly(new JsonRequest<CreateTask>(x => tasks.Apply(x))) },
                    { nameof(DeleteTask), new AdminOnly(new JsonRequest<DeleteTask>(x => tasks.Apply(x))) },

                    { nameof(SetPledge), new AdminOnly(new JsonRequest<SetPledge>(x => pledges.Apply(x))) },
                    { nameof(FundPledges), new ServiceOrAdmin(new JsonRequest<FundPledges>(x => pledges.Apply(x))) },

                    { nameof(MarkTaskComplete), new ApproverOnly(new JsonRequest<MarkTaskComplete>(x => taskInstances.Apply(x))) },
                    { nameof(MarkTaskNotComplete), new JsonRequest<MarkTaskNotComplete>(x => taskInstances.Apply(x)) },
                    { nameof(MarkTaskFunded), new ServiceOrAdmin(new JsonRequest<MarkTaskFunded>(x => taskInstances.Apply(x))) },
                    { nameof(ScheduleWorkItemsThrough), new ServiceOrAdmin(new JsonRequest<ScheduleWorkItemsThrough>(x => taskInstances.Apply(x))) },
                    { nameof(WaiveTask), new AdminOnly(new JsonRequest<WaiveTask>(x => taskInstances.Apply(x))) },

                    { nameof(RecordExpenditure), new JsonRequest<RecordExpenditure>(x => treasury.Apply(x)) }
                }));

                services.AddSingleton(new AppQueries(new Dictionary<string, IRequest>(StringComparer.InvariantCultureIgnoreCase)
                {
                    { "accounts", new ParameterlessRequest(() => accounts.GetAll().Select(x => new AccountBalance { Name = users.Get(x.Id).Name, Balance = x.Balance })) },
                    { nameof(Account), new JsonRequest<GetById>(x => Response.Success(accounts.Get(x.Id))) }
                }));

                new AppRecurringTasks(
                    new FundPledgesDaily(pledges), 
                    new MarkNotCompletedTasksDaily(taskInstances),
                    new ScheduleTasksDaily(taskInstances))
                        .Start();

                new HandleTaskInstanceCompletionPayments(taskInstances, accounts, messages)
                    .Start();

                new FundScheduledTasks(taskInstances, accounts, messages)
                    .Start();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Fatal error in Startup: {e.Message}");
                Debug.WriteLine($"Fatal error in Startup: {e.Message}");
                Environment.Exit(-1);
            }
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            app.UseMvc();
        }
    }
}
