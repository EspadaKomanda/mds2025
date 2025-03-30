using System.Net;
using System.Net.Mail;
using System.Text;
using MDSBackend.Database;
using MDSBackend.Database.Repositories;
using MDSBackend.Logs;
using MDSBackend.Mapper;
using MDSBackend.Services.Cookies;
using MDSBackend.Services.CurrentUsers;
using MDSBackend.Services.InstructionTests;
using MDSBackend.Services.JWT;
using MDSBackend.Services.UsersProfile;
using MDSBackend.Utils;
using MDSBackend.Utils.Factory;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace MDSBackend.Extensions;

public static class MappingExtensions
{
    public static IServiceCollection AddMapping(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        return services;
    }
}

public static class CachingExtensions
{
    public static IServiceCollection AddRedisCaching(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration["Redis:ConnectionString"] ?? "localhost:6379";
            options.InstanceName = configuration["Redis:InstanceName"] ?? "default";
        });
        return services;
    }
}

public static class LoggingExtensions
{
    public static IHostBuilder UseCustomSerilog(this IHostBuilder hostBuilder)
    {
        LoggingConfigurator.ConfigureLogging();
        return hostBuilder.UseSerilog();
    }
}

public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationContext>(x =>
        {
            var dbSettings = configuration.GetSection("DatabaseSettings");
            var hostname = dbSettings["Hostname"] ?? "localhost";
            var port = dbSettings["Port"] ?? "5432";
            var name = dbSettings["Name"] ?? "postgres";
            var username = dbSettings["Username"] ?? "postgres";
            var password = dbSettings["Password"] ?? "postgres";
            x.UseNpgsql($"Server={hostname}:{port};Database={name};Uid={username};Pwd={password};");
        });
        services.AddScoped<UnitOfWork>(sp => new UnitOfWork(sp.GetRequiredService<ApplicationContext>()));
        services.AddScoped(typeof(GenericRepository<>));
        return services;
    }
}

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }
}

public static class JwtAuthExtensions
{
    public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new SystemException("JwtSettings:SecretKey not found");
        var key = Encoding.ASCII.GetBytes(secretKey);
        
        services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        
        return services;
    }
}

public static class BackendServicesExtensions
{
    public static IServiceCollection AddBackendServices(this IServiceCollection services)
    {
        services.AddScoped<IUserProfileService, UserProfileService>();
        services.AddScoped<IInstructionTestsService, InstructionTestsService>();
        return services;
    }
}

public static class UtilServicesExtensions
{
    public static IServiceCollection AddUtilServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<ICookieService, CookieService>();
        return services;
    }
}

public static class NotificationSettings
{
    public static IServiceCollection AddPushNotifications(this IServiceCollection services, IConfiguration configuration)
    {
        var notificationSettings = configuration.GetSection("NotificationSettings");
        var apiKey = notificationSettings["ApiKey"];
        var token = notificationSettings["Token"];
        var baseUrl = notificationSettings["Url"];
        var projectId = notificationSettings["ProjectId"];
        
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri(baseUrl);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        services.AddSingleton(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<PushNotificationsClient>>();
            return new PushNotificationsClient(client, logger, token, projectId);
        });
        return services;
    }
}
public static class EmailExtensions
{
    public static IServiceCollection AddEmail(this IServiceCollection services, IConfiguration configuration)
    {
        var smtpSettings = configuration.GetSection("EmailSettings");
        var host = smtpSettings["Host"] ?? "localhost";
        var port = Convert.ToInt32(smtpSettings["Port"] ?? "25");
        var username = smtpSettings["Username"] ?? "username";
        var password = smtpSettings["Password"] ?? "password";
        var email = smtpSettings["EmailFrom"] ?? "email";
        services.AddScoped<SmtpClient>(sp => new SmtpClient(host)
        {
            Port = port,
            Credentials = new NetworkCredential(username, password),
            EnableSsl = true,
        });

        services.AddSingleton<EmailClient>();
        return services;
    }
}

public static class FactoryExtensions
{
    public static IServiceCollection AddFactories(this IServiceCollection services)
    {
        services.AddSingleton<MailNotificationsFactory>();
        services.AddSingleton<PushNotificationsFactory>();
        return services;
    }
}
