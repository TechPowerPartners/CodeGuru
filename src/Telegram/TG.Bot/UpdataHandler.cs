//using Telegram.Bot.Polling;
//using Telegram.Bot.Types.Enums;
//using Telegram.Bot.Types;
//using Telegram.Bot;
//using Telegram.Bot.Types.ReplyMarkups;
//using Domain.Entities;
//using Api.Persistence;

//class UpdataHandler : IUpdateHandler
//{
//    private Random _random = new Random();

//    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
//    {
//        throw new NotImplementedException();
//    }

//    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
//    {
//        switch (update.Type)
//        {
//            case UpdateType.Message:
//                if (update.Message!.Text == "/questions")
//                    await SendQuestion(botClient, update);
//                break;
//            case UpdateType.CallbackQuery:

//                break;
//        }
//    }

//    public async Task SendQuestion(ITelegramBotClient botClient, Update update)
//    {
//        var question = await GetRandomQuestionAsync();

//        Console.WriteLine(question.Id);
//        Console.WriteLine(question.Text);

//        var userId = update.Message!.From!.Id;

//        if (question == null)
//        {
//            await botClient.SendTextMessageAsync(userId, "Вопросов нет");
//            return;
//        }

//        var inlineKeyboardAnswers = GetInlineButtons(question.Answers.Select(answer => answer.Text));

//        if (question.Photos == null || question.Photos.Count == 0)
//        {
//            await botClient.SendTextMessageAsync(userId, question.Text, replyMarkup: inlineKeyboardAnswers);
//            return;
//        }

//        var inputMediaPhotos = question.Files!.Select(p => new InputMediaPhoto(InputFile.FromUri(p.Url)));

//        var messages = await botClient.SendMediaGroupAsync(userId, media: inputMediaPhotos);
//        await botClient.EditMessageTextAsync(userId, messages[0].MessageId, question.Text, replyMarkup: inlineKeyboardAnswers);
//        Console.WriteLine(question.Id);
//    }

//    public static InlineKeyboardMarkup GetInlineButtons(IEnumerable<string> buttons)
//    {
//        var inlineKeyboardButtons = new List<List<InlineKeyboardButton>>();

//        foreach (var button in buttons)
//            inlineKeyboardButtons.Add([new(button) { CallbackData = button }]);

//        return new InlineKeyboardMarkup(inlineKeyboardButtons);
//    }
//}