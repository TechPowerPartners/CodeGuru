﻿using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotExtension.Types;
using TestingPlatform.Api.Contracts.Dto;
using TG.Bot.Extensions;

namespace TG.Bot.TelegramApi.TestService.Views;

internal class QuestionView
{
    public static async Task<Message> ShowAsync(TelegramContext context, GetQuestionDto question)
    {
        return await context.BotClient.SendTextMessageAsync(
            context.UserId,
            question.Text,
            replyMarkup: question.Answers.ToInlineKeyboardMarkup(),
            parseMode: ParseMode.Html);
    }

    public static async Task EditAnswerAsync(TelegramContext context, ICollection<GetAnswerDto> answers)
    {
        try
        {
            await context.BotClient.EditMessageReplyMarkupAsync(
                context.UserId,
                context.Update.CallbackQuery!.Message!.MessageId,
                answers.ToInlineKeyboardMarkup()
                );
        }
        catch
        {
            //Ignore
        }
    }

    public static async Task EditAsync(TelegramContext context, GetQuestionDto question)
    {
        await context.BotClient.EditMessageTextAsync(
            context.UserId,
            context.Update.CallbackQuery!.Message!.MessageId,
            question.Text,
            replyMarkup: question.Answers.ToInlineKeyboardMarkup()
            );
    }

    public static async Task ErrorAsync(TelegramContext context)
    {
        await context.BotClient.AnswerCallbackQueryAsync(
            context.Update.CallbackQuery!.Id,
            "Ошибка",
            showAlert: true);
    }
}
