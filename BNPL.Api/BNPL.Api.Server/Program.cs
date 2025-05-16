using BNPL.Api.Server.src.Presentation.Configurations;

var builder = WebApplication.CreateBuilder(args);

Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddSwaggerConfiguration()
    .AddServiceConfiguration()
    .AddRepositories()
    .AddInternalServices()
    .AddExternalServices()
    .AddHttpContextAccessor()
    .AddDatabaseConfiguration(builder.Configuration);

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();
