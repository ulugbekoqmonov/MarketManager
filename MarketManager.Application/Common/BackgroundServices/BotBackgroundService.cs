//using Microsoft.Extensions.Hosting;
//using Serilog;
//using Telegram.Bot;
//using Telegram.Bot.Polling;

//namespace MarketManager.Application.Common.BackgroundServices;
//public class BotBackgroundService : BackgroundService
//{
//    private readonly ITelegramBotClient _botClient;
//    private readonly IUpdateHandler _updateHandler;

//    public BotBackgroundService(ITelegramBotClient botClient, IUpdateHandler updateHandler)
//    {
//        _botClient = botClient;
//        _updateHandler = updateHandler;
//    }

//    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//    {
//        var me = await _botClient.GetMeAsync(stoppingToken);
//        Log.Information("Bot started:{username}", me.Username);

//        _botClient.StartReceiving(
//            updateHandler: _updateHandler,
//            receiverOptions: default,
//            cancellationToken: stoppingToken);
//    }
//}

