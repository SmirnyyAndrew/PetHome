using FilesService.Core.Loggers;
using FilesService.Core.Validation;
using FilesService.Extentions.AppExtentions;
using FilesService.Extentions.BuilderExtentions;
using Microsoft.AspNetCore.Builder;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);  

builder.Services.AddSerilog();
Log.Logger = SeqLogger.InitDefaultSeqConfiguration(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Auto validation
builder.Services.AddFluentValidationAutoValidation(configuration =>
{
    configuration.OverrideDefaultResultFactoryWith<CustomResultFactory>();
});

builder.Services.AddServices(builder.Configuration);


var app = builder.Build(); 

app.AddAppAttributes();

app.UseSerilogRequestLogging(); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();