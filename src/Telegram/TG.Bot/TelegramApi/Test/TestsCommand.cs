using System;
using System.Collections.Generic;
using System.Linq;
using TG.Bot.Intagrations.BackendApi;
using TG.Bot.Contracts;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using TG.Bot.Common;

namespace TG.Bot.TelegramApi.Test;

/// <summary>
/// Команда на получение тестов
/// </summary>
/// <param name="_backendApi"></param>
internal class TestsCommand(IBackendApi _backendApi) : IBotController
{
    public List<UpdateType> UpdateTypes => [UpdateType.Message];

    public async Task HandleUpdateAsync(TelegramContext context)
    {
        if (context.Update.Message!.Text != "/tests")
            return;


    }
}