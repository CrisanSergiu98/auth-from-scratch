using AuthFromScratch.Application;
using AuthFromScratch.Common.Errors;
using AuthFromScratch.Infrastructure;
using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddApplication();
builder.Services.AddInfrastrcture(builder.Configuration);

builder.Services.AddSingleton<ProblemDetailsFactory, AuthFromScratchProblemDetailsFactory>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler("/error");

app.MapControllers();
app.UseHttpsRedirection();

app.Run();
