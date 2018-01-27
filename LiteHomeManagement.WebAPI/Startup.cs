using LiteHomeManagement.App.Accounting;
using LiteHomeManagement.App.Common;
using LiteHomeManagement.App.Task;
using LiteHomeManagement.App.User;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LiteHomeManagement.WebAPI
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
            services.AddMvc();

            services.AddSingleton<IEventStore>(x => new InMemoryEventStore());
            services.AddSingleton<IEntityStore<UserRecord>>(x => new InMemoryEntityStore<UserRecord>());
            services.AddSingleton<IEntityStore<TaskRecord>>(x => new InMemoryEntityStore<TaskRecord>());
            services.AddScoped(x => new Accounts(x.GetService<IEventStore>()));
            services.AddScoped(x => new Users(x.GetService<IEntityStore<UserRecord>>()));
            services.AddScoped(x => new Tasks(x.GetService<IEntityStore<TaskRecord>>()));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseMvc();
        }
    }
}
