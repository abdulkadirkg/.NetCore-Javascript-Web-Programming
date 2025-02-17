
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSD.Util.Mappers;
using CSD.Util.Mappers.Mapster;
using System.Threading;

using CSD.PostalCodeSearchApp.Data.Services;
using CSD.PostalCodeSearchApp.Data.DAL;
using CSD.PostalCodeSearchApp.Data.Repositories;
using CSD.PostalCodeSearchApp.Geonames;
using CSD.PostalCodeSearchApp.Data.Repositories.Context;

namespace PostalCodeSearchServiceGeoNames
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PostalCodeSearchServiceGeoNames", Version = "v1" });
            });

            //For DI

            services
                .AddHttpClient()
                .AddSingleton<PostalCodeSearchClient>()
                .AddSingleton<PostalCodeSearchAppDbContext>()

                .AddSingleton<PostalCodeSearchAppDataHelper>()
                .AddSingleton<PostalCodeSearchAppService>()
                .AddSingleton<IMapper, Mapper>()
                .AddSingleton<IPostalCodeSearchRepository, PostalCodeSearchRepository>()
                ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PostalCodeServiceGeoNames v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //Tamamen örnek amaçlı yazılmıştır. Arka plan bir iş (job) için uygun olabilir. Şüphesiz yeri burası olmak zorunda değildir
            //new Timer(_ => Console.Write("."), null, 3000, 3000);
        }       
    }
}
