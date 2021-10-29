using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using ProjectArchitecture.Bootstrapper;
using ProjectArchitecture.Bootstrapper.Extensions;

namespace ProjectArchitecture.UI.Api {
    public class Startup {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) {
            services.AddServices()
                    .AddControllers()
                    .AddJsonOptions(opt => {
                        opt.JsonSerializerOptions.IgnoreNullValues = true;
                        opt.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                        opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    });

            services.AddHealthChecks()
                    .AddCheck("AppHealthy", () => HealthCheckResult.Healthy(), null);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler(appError => {
                    appError.Run(async context => {
                        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (contextFeature != null) {
                            context.Response.StatusCode = 500;
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync(JsonSerializer.Serialize(new {
                                Code = 500,
                                Message = "Internal Server Error Occurred. Please Call Provider!"
                            }));
                        }
                    });
                });
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions() {
                    ResponseWriter = (context, healthreport) => {
                        if (healthreport.Status == HealthStatus.Healthy) {
                            context.Response.StatusCode = 200;
                            return context.Response.WriteAsync("it works fine. :-)", Encoding.UTF8);
                        }

                        context.Response.StatusCode = 500;
                        return context.Response.WriteAsync("app is not healthy :-(", Encoding.UTF8);
                    }
                });
            });
        }
    }
}
