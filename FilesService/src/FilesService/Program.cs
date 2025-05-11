using FilesService.Extentions.AppExtentions;
using FilesService.Extentions.BuilderExtentions;
using PetHome.Core.Application.Validation;
using PetHome.Core.Web.Loggers;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);  

builder.Services.AddSerilog();
Log.Logger = LoggerManager.InitConfiguration(builder.Configuration);

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