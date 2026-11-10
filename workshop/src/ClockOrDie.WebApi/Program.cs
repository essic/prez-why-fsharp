using ClockOrDie.Core;
using ClockOrDie.WebApi;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<Effects.IHandleDatabaseOperations, FakeDb>();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.MapClockOrDieEndpoints();

app.Run();
