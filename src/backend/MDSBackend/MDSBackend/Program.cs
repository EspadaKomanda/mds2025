using MDSBackend.Extensions;

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

#region SMTP

builder.Services.AddSMTP(builder.Configuration);

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
