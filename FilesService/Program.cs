using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using FilesService.Extentions;
using PetHome.Core.Response.Loggers;
using PetHome.Core.Response.Validation;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog();
Log.Logger = SeqLogger.InitDefaultSeqConfiguration(builder.Configuration);

// Add services to the container.

builder.Services.AddEndpoints();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAmazonS3();

//Auto validation
builder.Services.AddFluentValidationAutoValidation(configuration =>
{
    configuration.OverrideDefaultResultFactoryWith<CustomResultFactory>();
});

builder.Services.AddCors();

var app = builder.Build();

//Добавить CORS
app.AddCORS("http://localhost:5173");

//Middleware для отлова исключений (-стэк трейс)
app.UseExceptionHandler();

app.UseSerilogRequestLogging();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();

