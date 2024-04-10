using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Guard.Api.DTOs.Interview;
using Guard.Bot.Integrations.GuardApi;
using Guard.Bot.Settings;
using Microsoft.Extensions.Options;
using Nefarius.DSharpPlus.Extensions.Hosting.Events;
using Shared.Domain;

namespace Guard.Bot.SubscriberModules;

public class DiscordModalSubmittedEventSubscriber(
   IGuardApi guardApi,
   IOptions<TPPServerSettings> _tppServerSettings) : IDiscordModalSubmittedEventSubscriber
{
   public async Task DiscordOnModalSubmitted(DiscordClient sender, ModalSubmitEventArgs args)
   {
      var guild = await sender.GetGuildAsync(_tppServerSettings.Value.Id);
      var member = await guild.GetMemberAsync(args.Interaction.User.Id);

      var isValid = TryParseToInterviewDto(member, args.Values, out var interviewDto);

      if (!isValid)
      {
         var errorEmbed = new DiscordEmbedBuilder()
            .WithTitle("Введены неверные данные")
            .AddField("Вводите данные в следующем формате", "Дата: dd.mm.yyyy \n Время: hh:mm-hh:mm")
            .WithColor(DiscordColor.Red);

         await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().AddEmbed(errorEmbed));
         return;
      }

      var response = await guardApi.CreateInterviewAsync(interviewDto);
      
      if (!response.IsSuccessStatusCode)
      {
         var errorEmbed = new DiscordEmbedBuilder()
            .WithTitle("Bad request")
            .AddField("Error", response.Error.Content)
            .WithColor(DiscordColor.Red);
      
         await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().AddEmbed(errorEmbed));
         return;
      }

      var discordEmbedBuilder = new DiscordEmbedBuilder()
         .WithTitle("Запрос на собеседование отправлен")
         .AddField("Текущая роль", $"{interviewDto.FromRole.GetRealName()}")
         .AddField("Следующая роль", $"{interviewDto.ToRole.GetRealName()}")
         .AddField("Дата проведения", $"{interviewDto.StartDate:dd.MM.yyyy}")
         .AddField("Время проведения", $"С {interviewDto.FromTime:hh:mm} по {interviewDto.ToTime:hh:mm}")
         .WithColor(DiscordColor.Green);
      
      await args.Interaction.CreateResponseAsync(
         InteractionResponseType.ChannelMessageWithSource,
         new DiscordInteractionResponseBuilder().AddEmbed(discordEmbedBuilder));
   }

   private bool TryParseToInterviewDto(
      DiscordMember member,
      IReadOnlyDictionary<string, string> values,
      out InterviewDto dto)
   {
      dto = null!;
      var date = values["date"];
      var time = values["time"].Split("-");

      if (time.Length != 2)
         return false;

      if (!DateOnly.TryParse(date, out var startDate) ||
          !TimeOnly.TryParse(time[0], out var from) ||
          !TimeOnly.TryParse(time[1], out var to))
         return false;

      var currentRole = member.Roles.FirstOrDefault(r => CareerRoleExtensions.IsRealRoleValid(r.Name));
      var role = CareerRoleExtensions.GetFromRealName(currentRole.Name);

      dto = new InterviewDto
      {
         StartDate = startDate,
         FromTime = from,
         ToTime = to,
         FromRole = role,
         ToRole = role.BoostByInterview(),
         Name = member.Username
      };

      return true;
   }
}