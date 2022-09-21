using IDCH.CloudDrive.Data;
using IDCH.CloudDrive.Services;
using MudBlazor.Services;
using Blazored.LocalStorage;
using Blazored.Toast;
using Microsoft.Extensions.Configuration;
using System.Text;
using IDCH.CloudDrive.Properties;
using IDCH.Tools;
using Microsoft.AspNetCore.Components.Authorization;
using IDCH.CloudDrive.Helpers;

namespace IDCH.CloudDrive;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(Resources.appsettings_json));

        var config = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();


        builder.Configuration.AddConfiguration(config);

        ConfigureServices(builder.Services, builder.Configuration);
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
#endif

        //builder.Services.AddSingleton<WeatherForecastService>();

        return builder.Build();
    }

    static void ConfigureServices(IServiceCollection services, Microsoft.Extensions.Configuration.ConfigurationManager Configuration)
    {


        services.AddBlazoredLocalStorage();
        services.AddBlazoredToast();
        services.AddMudServices();



        MailService.MailUser = Configuration["MailSettings:MailUser"];
        MailService.MailPassword = Configuration["MailSettings:MailPassword"];
        MailService.MailServer = Configuration["MailSettings:MailServer"];
        MailService.MailPort = int.Parse(Configuration["MailSettings:MailPort"]);
        MailService.SetTemplate(Configuration["MailSettings:TemplatePath"]);
        MailService.SendGridKey = Configuration["MailSettings:SendGridKey"];
        MailService.UseSendGrid = true;


        SmsService.UserKey = Configuration["SmsSettings:ZenzivaUserKey"];
        SmsService.PassKey = Configuration["SmsSettings:ZenzivaPassKey"];
        SmsService.TokenKey = Configuration["SmsSettings:TokenKey"];
        
        services.AddMauiBlazorWebView();
        services.AddBlazoredToast();
        services.AddBlazoredLocalStorage();

        services.AddScoped<ISyncMemoryStorageService, LocalMemoryStorage>();
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

        services.AddSingleton<TempStorageService>();


        //data service
        services.AddTransient<CloudApi>();
        services.AddTransient<WeatherForecastService>();

    }
}
