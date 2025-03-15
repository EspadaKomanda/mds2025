using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MDSBackend.Database;
using MDSBackend.Database.Repositories;
using MDSBackend.Extensions;
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

builder.Services.AddMapping();

#endregion


#region Caching

builder.Services.AddRedisCaching(builder.Configuration);

#endregion


#region Logging

builder.Services.AddLogging();

#endregion

#region UtilServices

builder.Services.AddUtilServices();

#endregion


#region Database

builder.Services.AddDatabase(builder.Configuration);

#endregion


#region JwtAuth

builder.Services.AddJwtAuth(builder.Configuration);

#endregion


#region Services

builder.Services.AddBackendServices();

#endregion

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
