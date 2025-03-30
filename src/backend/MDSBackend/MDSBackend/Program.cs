using MDSBackend.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

#region Factories

builder.Services.AddFactories();

#endregion

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

builder.Services.AddEmail(builder.Configuration);

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
builder.Services.AddSwagger();

var app = builder.Build();

app.MapOpenApi();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MDS2025 API V1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
