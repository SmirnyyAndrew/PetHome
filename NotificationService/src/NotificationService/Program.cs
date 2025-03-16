using NotificationService.DependencyInjections;
using NotificationService.Infrastructure.TelegramNotification;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDependencyInjections(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("test-telegram", async (
    TelegramManager telegramManager) =>
{
    Guid userId = Guid.Parse("51c5a16c-3bb2-476a-a89a-aaf1a8724482");
    await telegramManager.StartRegisterChatId(userId, "andrey_5701"); 
    await telegramManager.SendMessage(userId, "wasssup"); 
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
