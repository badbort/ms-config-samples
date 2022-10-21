using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Options;
using SampleApp.Config;
using SampleApp.Data;

namespace SampleApp;

public static class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Retrieve the connection string
        string appConfigConnectionString = builder.Configuration.GetConnectionString("AppConfig");

        if (!string.IsNullOrEmpty(appConfigConnectionString))
        {
            // Load configuration from Azure App Configuration
            builder.Configuration.AddAzureAppConfiguration(o =>
            {
                o.Connect(appConfigConnectionString)
                    .Select(KeyFilter.Any)
                    .ConfigureRefresh(refreshOptions =>
                    {
                        refreshOptions.Register("Sample:Name", refreshAll: true)
                            .SetCacheExpiration(TimeSpan.FromSeconds(10));
                    });
            });
            
            builder.Services.AddAzureAppConfiguration();
        }
        
        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddSingleton<WeatherForecastService>();

        // Uses reflection
        builder.Services.ConfigureOptions<ConfigureThoughts>();
        builder.Services.ConfigureOptions<PostConfigureThoughts>();

        
        // builder.Services.ConfigureOptions>()
        builder.Services.AddOptions<SampleSettings>()
            .Bind(builder.Configuration.GetSection(SampleSettings.ConfigKey))
            .Validate(o => !string.IsNullOrEmpty(o.Name), "Sample    name was unspecified >:(")
            .Validate(o =>  o.Name?.Count(c => c == ' ') <= 1, "You cannot have multiple spaces in your name!")
            .ValidateDataAnnotations();
            //.ValidateOnStart();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        app.MapGet("/api/test", async context =>
        {
            var optionsMonitor = app.Services.GetRequiredService<IOptionsMonitor<SampleSettings>>();
            await context.Response.WriteAsJsonAsync(optionsMonitor.CurrentValue);
        });

        await app.RunAsync();
    }

    private static void Test2()
    {
        ConfigurationBuilder configBuilder = new();
        configBuilder.AddJsonFile("appsettings.json", true, reloadOnChange: true);
        
        
    }
}