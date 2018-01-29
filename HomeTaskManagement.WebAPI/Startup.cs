using HomeTaskManagement.App.Accounting;
using HomeTaskManagement.App.Common;
using HomeTaskManagement.App.Task;
using HomeTaskManagement.App.User;
using HomeTaskManagement.Sql;
using HomeTaskManagement.WebAPI.Auth;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
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
                services.AddMvc();

                var sqlDb = new SqlDatabase(new EnvironmentVariable("HomeTaskManagementSqlConnection"));
                services.AddSingleton(sqlDb);
                services.AddSingleton(new MiniAuth(new EnvironmentVariable("HomeTaskManagementSecret")));
                services.AddSingleton(new AppHealth(TimeSpan.FromMinutes(15), sqlDb));
                services.AddSingleton<IEventStore>(x => new InMemoryEventStore());
                services.AddSingleton<IEntityStore<UserRecord>>(x => new InMemoryEntityStore<UserRecord>());
                services.AddSingleton<IEntityStore<TaskRecord>>(x => new InMemoryEntityStore<TaskRecord>());
                services.AddScoped(x => new Accounts(x.GetService<IEventStore>()));
                services.AddScoped(x => new Users(x.GetService<IEntityStore<UserRecord>>()));
                services.AddScoped(x => new Tasks(x.GetService<IEntityStore<TaskRecord>>()));
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

            app.UseMvc();
        }
    }
}
