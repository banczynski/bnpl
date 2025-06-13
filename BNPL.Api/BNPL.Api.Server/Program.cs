using BNPL.Api.Server.src.Presentation.Configurations;
using Core.Context;
using Core.Context.Interfaces;
using Dapper;

var builder = WebApplication.CreateBuilder(args);

SimpleCRUD.SetDialect(SimpleCRUD.Dialect.PostgreSQL);
DefaultTypeMap.MatchNamesWithUnderscores = true;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContext, UserContext>();
builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services
    .AddSwaggerConfiguration()
    .AddDatabaseConfiguration(builder.Configuration)
    .AddRepositories()
    .AddInternalServices()
    .AddExternalServices()
    .AddUseCasesConfiguration();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();
