using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFC4RESTAPI.Repositories;
using EFC4RESTAPI.Services;
using EFC4RESTAPI.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace EFC4RESTAPI
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
            var settings = Configuration.GetSection("DB_MySQL").Get<DBConnection>();
            services.AddDbContext<AppDBContext>(options =>
            options.UseMySql(settings.ConnString, new MySqlServerVersion(new Version(8, 0, 22)),mySqlOptions =>
            mySqlOptions.CharSetBehavior(CharSetBehavior.NeverAppend)).EnableSensitiveDataLogging().EnableDetailedErrors());

            services.AddScoped<IDBContext, EFCRepository>();

            services.AddControllers(option => { option.SuppressAsyncSuffixInActionNames = false; });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EFC4RESTAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EFC4RESTAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
