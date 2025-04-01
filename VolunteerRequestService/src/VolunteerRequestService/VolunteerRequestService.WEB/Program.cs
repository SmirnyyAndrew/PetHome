using VolunteerRequestService.WEB.DI;

var builder = WebApplication.CreateBuilder(args); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDependencyInjection(builder.Configuration);

var app = builder.Build();

//app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddAppDependencyInjection();

app.UseHttpsRedirection();
app.UseAuthentication();  
app.UseAuthorization();
app.MapControllers();

app.Run();