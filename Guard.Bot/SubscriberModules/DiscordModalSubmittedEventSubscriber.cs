using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Guard.Api.DTOs.Interview;
using Guard.Bot.Integrations.GuardApi;
using Nefarius.DSharpPlus.Extensions.Hosting.Events;
using Shared.Domain;

namespace Guard.Bot.SubscriberModules;

public class DiscordModalSubmittedEventSubscriber(IGuardApi guardApi) : IDiscordModalSubmittedEventSubscriber
{
   public async Task DiscordOnModalSubmitted(DiscordClient sender, ModalSubmitEventArgs args)
   {
      var isValid = TryParseToInterviewDto(args.Values, out var interviewDto);

      if (!isValid)
      {
         var errorEmbed = new DiscordEmbedBuilder()
            .WithTitle("Введены неверные данные")
            .AddField("Вводите данные в следующем формате", "Дата: dd.mm.yyyy \n Время: hh:mm-hh:mm")
            .WithColor(DiscordColor.Red);

         await sender.SendMessageAsync(args.Interaction.Channel, errorEmbed);
         return;
      }

      var response = await guardApi.CreateInterviewAsync(interviewDto);

      if (!response.IsSuccessStatusCode)
      {
         var errorEmbed = new DiscordEmbedBuilder()
            .WithTitle("Bad request")
            .AddField("Error", response.Error.Content)
            .WithColor(DiscordColor.Red);

         await sender.SendMessageAsync(args.Interaction.Channel, errorEmbed);
         return;
      }

      var discordEmbedBuilder = new DiscordEmbedBuilder()
         .WithTitle("Запрос на собеседование отправлен")
         .AddField("Участник", interviewDto.Name)
         .AddField("Текущая роль", $"{interviewDto.FromRole}")
         .AddField("Следующая роль", $"{interviewDto.ToRole}")
         .AddField("Дата проведения",
            $"{interviewDto.StartDate:dd.MM.yyyy}. С {interviewDto.FromTime:hh:mm}. По {interviewDto.ToTime:hh:mm}")
         .WithColor(DiscordColor.Green);

      await sender.SendMessageAsync(args.Interaction.Channel, discordEmbedBuilder);

      bool TryParseToInterviewDto(
         IReadOnlyDictionary<string, string> values,
         out InterviewDto interviewDto)
      {
         interviewDto = null;
         var date = args.Values["date"];
         var time = args.Values["time"].Split("-");

         if (time.Length != 2)
            return false;

         if (!DateOnly.TryParse(date, out var startDate) ||
             !TimeOnly.TryParse(time[0], out var from) ||
             !TimeOnly.TryParse(time[1], out var to))
            return false;

         interviewDto = new InterviewDto
         {
            StartDate = startDate,
            FromTime = from,
            ToTime = to,
            FromRole = CareerRole.Amateur,
            ToRole = CareerRole.Amateur,
            Name = args.Interaction.User.Username
         };

         return true;
      }
   }
}