using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PolicyBasedAuthourization.AuthorizationModel;
using PolicyBasedAuthourization.Controllers;

namespace PolicyBasedAuthourization
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IList<UserCredential> users = new List<UserCredential>
            {
                new UserCredential { UserName = "test1", Password = "Password1",
                    Roles = new List<string> { "Administrator", "User" } },
                new UserCredential { UserName = "test2", Password = "Password2",
                    Roles = new List<string> {"User"}}
            };

            IDictionary<string, UserCredential> tokens = new Dictionary<string, UserCredential>();

            services.AddControllers();

            services.AddAuthentication("Basic")
                .AddScheme<BasicAuthenticationOptions, CustomAuthenticationHandler>("Basic", null);

            services.AddAuthorization(option =>
            {
                option.AddPolicy("AdminAndPowerUser", policy => policy.RequireRole("Administrator", "Poweruser"));

                option.AddPolicy("EmployeeWithMoreYearsRequirement", policy => policy.Requirements.Add(new EmployeeWithMoreYearsRequirement(20)));
            });

            services.AddSingleton<IAuthorizationHandler, EmployeeWithMoreYearsHandler>();

            services.AddSingleton<IEmployeeNumberOfYears, EmployeeNumberOfYears>();

            services.AddSingleton<ICustomAuthenticationManager>(option =>
                new CustomAuthenticationManager(users, tokens));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
