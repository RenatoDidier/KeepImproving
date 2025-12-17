using FastEndpoints;
using FastEndpoints.Swagger;
using KeepImproving.API.Extensions;
using KeepImproving.Infra.Contexts;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureDatabase(builder.Configuration)
    .AddAuthorization()
    .AddFastEndpointWithSwagger()
    .AddHealthChecks();

WebApplication app = builder.Build();

using (IServiceScope? scope = app.Services.CreateScope())
{
    AppDbContext? db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.UseFastEndpoints(options =>
{
    options.Endpoints.RoutePrefix = "api";
});
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
}

app.MapHealthChecks("/health");


app.Run();