using Infrastructure.Messaging.Kafka.DI;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using TelegramBot.Consumers;
using TelegramBot.Handlers;
using TelegramBot.Options;
using TelegramBot.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<TelegramBotOptions>(
    builder.Configuration.GetSection(TelegramBotOptions.SectionName));

builder.Services.AddSingleton(sp =>
{
    var options = sp.GetRequiredService<IOptions<TelegramBotOptions>>().Value;
    return new TelegramBotClient(options.BotToken);
});

builder.Services.AddSingleton<TelegramNotificationService>();
builder.Services.AddSingleton<ExchangeCompletedTelegramHandler>();
builder.Services.AddKafka(builder.Configuration);

builder.Services.AddHostedService<ExchangeCompletedConsumer>();

var app = builder.Build();
await app.RunAsync();
