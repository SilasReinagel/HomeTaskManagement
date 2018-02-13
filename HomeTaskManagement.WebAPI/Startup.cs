using HomeTaskManagement.App.Accounting;
using HomeTaskManagement.App.Commands;
using HomeTaskManagement.App.Common;
using HomeTaskManagement.App.Pledge;
using HomeTaskManagement.App.ServiceJobs;
using HomeTaskManagement.App.Shopping;
using HomeTaskManagement.App.Task;
using HomeTaskManagement.App.Task.Assignment;
using HomeTaskManagement.App.Task.Instance;
using HomeTaskManagement.App.User;
using HomeTaskManagement.Sql;
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

                var messages = new Messages();
                var eventStore = new SqlEventStore(sqlDb, "HomeTask.Events");
                var users = new Users(new UsersTable(sqlDb));
                var tasks = new Tasks(new TasksTable(sqlDb));
                var accounts = new Accounts(eventStore);
                var pledges = new Pledges(eventStore, users, accounts, new PledgeFundingSettings());
                var assignments = new TaskAssignments(eventStore, tasks, users, new AssignmentSettings());
                var taskInstances = new TaskInstances(new InMemoryTaskInstanceStore(), assignments, messages);
                var treasury = new Treasury(new InMemoryBlobStore(), accounts);

                services.AddSingleton(new AppCommands(new Dictionary<string, ICommand>(StringComparer.InvariantCultureIgnoreCase)
                {
                    { nameof(OpenAccount), new AdminOnly(new JsonCommand<OpenAccount>(x => accounts.Apply(x))) },
                    { nameof(SetOverdraftPolicy), new AdminOnly(new JsonCommand<SetOverdraftPolicy>(x => accounts.Apply(x))) },
                    { nameof(TransactionRequest), new AdminOnly(new JsonCommand<TransactionRequest>(x => accounts.Apply(x))) },

                    { nameof(RegisterUser), new JsonCommand<RegisterUser>(x => users.Apply(x)) },
                    { nameof(UnregisterUser), new JsonCommand<UnregisterUser>(x => users.Apply(x)) },
                    { nameof(AddRoles), new AdminOnly(new JsonCommand<AddRoles>(x => users.Apply(x))) },
                    { nameof(RemoveRoles), new AdminOnly(new JsonCommand<RemoveRoles>(x => users.Apply(x))) },

                    { nameof(AssignTask), new AdminOnly(new JsonCommand<AssignTask>(x => assignments.Apply(x))) },

                    { nameof(CreateTask), new AdminOnly(new JsonCommand<CreateTask>(x => tasks.Apply(x))) },
                    { nameof(DeleteTask), new AdminOnly(new JsonCommand<DeleteTask>(x => tasks.Apply(x))) },

                    { nameof(SetPledge), new AdminOnly(new JsonCommand<SetPledge>(x => pledges.Apply(x))) },
                    { nameof(FundPledges), new ServiceOrAdmin(new JsonCommand<FundPledges>(x => pledges.Apply(x))) },

                    { nameof(MarkTaskComplete), new ApproverOnly(new JsonCommand<MarkTaskComplete>(x => taskInstances.Apply(x))) },
                    { nameof(MarkTaskNotComplete), new JsonCommand<MarkTaskNotComplete>(x => taskInstances.Apply(x)) },
                    { nameof(MarkTaskFunded), new ServiceOrAdmin(new JsonCommand<MarkTaskFunded>(x => taskInstances.Apply(x))) },
                    { nameof(ScheduleWorkItemsThrough), new ServiceOrAdmin(new JsonCommand<ScheduleWorkItemsThrough>(x => taskInstances.Apply(x))) },
                    { nameof(WaiveTask), new AdminOnly(new JsonCommand<WaiveTask>(x => taskInstances.Apply(x))) },

                    { nameof(RecordExpenditure), new JsonCommand<RecordExpenditure>(x => treasury.Apply(x)) }
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
