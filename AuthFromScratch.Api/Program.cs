using AuthFromScratch.Application;
using AuthFromScratch.Infrastructure;
using AuthFromScratch.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddInfrastrcture(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler("/error");

app.MapControllers();
app.UseHttpsRedirection();

app.Run();
