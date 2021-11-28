using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NLog.Web;
using ProjectArchitecture.Bootstrapper.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(AppContext.BaseDirectory)
                     .AddJsonFile("appsettings.json", false, true);
builder.WebHost.UseNLog();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddServices()
                    .AddControllers()
                    .AddJsonOptions(opt => {
                        opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                        opt.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                        opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    });

builder.Services.AddHealthChecks()
        .AddCheck("AppHealthy", () => HealthCheckResult.Healthy());

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler(appError => {
        appError.Run(async context => {
            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
            if (contextFeature != null)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    Code = 500,
                    Message = "Internal Server Error Occurred. Please Call Provider!"
                }));
            }
        });
    });
}

app.UseAuthorization();
app.UseRouting();
app.MapControllers();
app.MapHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = (context, healthreport) => {
        if (healthreport.Status == HealthStatus.Healthy)
        {
            context.Response.StatusCode = 200;
            return context.Response.WriteAsync("it works fine. :-)", Encoding.UTF8);
        }

        context.Response.StatusCode = 500;
        return context.Response.WriteAsync("app is not healthy :-(", Encoding.UTF8);
    }
});

app.Run();
