using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MDSBackend.Database;
using MDSBackend.Database.Repositories;
using MDSBackend.Logs;
using MDSBackend.Mapper;
using MDSBackend.Services.Auth;
using MDSBackend.Services.Cookies;
using MDSBackend.Services.JWT;
using MDSBackend.Services.CurrentUsers;
using MDSBackend.Services.UsersProfile;
using MDSBackend.Models.Database;
using MDSBackend.Services.Users;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

#region Mapping

builder.Services.AddAutoMapper(typeof(MappingProfile));

#endregion


#region Caching

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:ConnectionString"] ?? "localhost:6379";
    options.InstanceName = builder.Configuration["Redis:InstanceName"] ?? "default";
});

#endregion


#region Logging

LoggingConfigurator.ConfigureLogging();
builder.Host.UseSerilog();

#endregion


#region UtilServices

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddScoped<ICookieService,CookieService>();

#endregion


#region Database

builder.Services.AddDbContext<ApplicationContext>(x => {
    var dbSettings = builder.Configuration.GetSection("DatabaseSettings");
    var hostname = dbSettings["Hostname"] ?? "localhost";
    var port = dbSettings["Port"] ?? "5432";
    var name = dbSettings["Name"] ?? "postgres";
    var username = dbSettings["Username"] ?? "postgres";
    var password = dbSettings["Password"] ?? "postgres";
    x.UseNpgsql($"Server={hostname}:{port};Database={name};Uid={username};Pwd={password};");
});
builder.Services.AddScoped<UnitOfWork>(sp => 
    new (sp.GetRequiredService<ApplicationContext>()));

builder.Services.AddScoped<GenericRepository<Role>>();
builder.Services.AddScoped<GenericRepository<User>>();
builder.Services.AddScoped<GenericRepository<UserProfile>>();
#endregion


#region JwtAuth

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new SystemException("JwtSettings:SecretKey not found");

var key = Encoding.ASCII.GetBytes(secretKey);
builder.Services.AddAuthentication(x =>
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

#endregion


#region Services

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<IAuthService, AuthService>();

#endregion

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
