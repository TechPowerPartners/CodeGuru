using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Guard.Api.Contracts.Interviews;
using Guard.Bot.Integrations.GuardApi;
using Guard.Bot.Settings;
using Microsoft.Extensions.Options;
using Nefarius.DSharpPlus.Extensions.Hosting.Events;
using Domain.Shared;
using Guard.Bot.Extensions;

namespace Guard.Bot.SubscriberModules;

internal class DiscordModalSubmittedEventSubscriber(
   IGuardApi _guardApi,
   IOptions<DiscordServerSettings> _tppServerSettings) : IDiscordModalSubmittedEventSubscriber
{
   public async Task DiscordOnModalSubmitted(DiscordClient sender, ModalSubmitEventArgs args)
   {
      var guild = await sender.GetGuildAsync(_tppServerSettings.Value.Id);
      var member = await guild.GetMemberAsync(args.Interaction.User.Id);

      var isValid = TryParseCreateInterviewRequest(member, args.Values, out var interviewDto);

      if (!isValid)
      {
         var errorEmbed = new DiscordEmbedBuilder()
            .WithTitle("Некорректные данные")
            .AddField("Вводите данные в следующем формате", "Дата: dd.mm.yyyy \n Время: hh:mm-hh:mm")
            .WithColor(DiscordColor.Red);

         await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().AddEmbed(errorEmbed));
         return;
      }

      var response = await _guardApi.CreateInterviewAsync(interviewDto);
      
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
         .AddField("Дата проведения", $"<t:{interviewDto.FromTime.ToUnixTimestamp()}:D>")
         .AddField("Время проведения",
            $"С <t:{interviewDto.FromTime.ToUnixTimestamp()}:t> по <t:{interviewDto.ToTime.ToUnixTimestamp()}:t>")
         .WithColor(DiscordColor.Green);

      await args.Interaction.CreateResponseAsync(
         InteractionResponseType.ChannelMessageWithSource,
         new DiscordInteractionResponseBuilder().AddEmbed(discordEmbedBuilder));
   }

   private bool TryParseCreateInterviewRequest(
      DiscordMember member,
      IReadOnlyDictionary<string, string> values,
      out CreateInterviewRequest request)
   {
      request = null!;
      var date = values["date"];
      var time = values["time"].Split("-");

      if (time.Length != 2)
         return false;


      if (!date.TryParseWithRuCulture(out DateOnly startDate) ||
          !time[0].TryParseWithRuCulture(out TimeOnly from) ||
          !time[1].TryParseWithRuCulture(out TimeOnly to))
         return false;

      var currentRole = member.Roles.FirstOrDefault(r => CareerRoleExtensions.IsRealRoleValid(r.Name));
      var role = CareerRoleExtensions.GetFromRealName(currentRole.Name);

      request = new CreateInterviewRequest
      {
         FromTime = new DateTime(startDate, from),
         ToTime = new DateTime(startDate, to),
         FromRole = role,
         ToRole = role.BoostByInterview(),
         IntervieweeName = member.Username
      };

      return true;
   }
}