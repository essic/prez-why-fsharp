using ClockOrDie.WebApi.Controllers;
using ClockOrDie.WebApi.Controllers.CodeGen;
using ClockOrDie.Core;
using ClockOrDie.WebApi;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddScoped<ICodeGenClockOrDieController, ClockOrDieController>();
builder.Services.AddSingleton<Effects.IHandleDatabaseOperations, FakeDb>();

builder.Services.AddControllers(opt => opt.Filters.Add(new CustomValidationFilter()));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


app.Run();
