
using botovskiy.DTOs;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace botovskiy.Commands
{
    public class IsmeCommands : BaseCommandModule
    {
        [Command("daygazu")]
        public async Task GenericIsma(CommandContext context)
        {

            string json = await File.ReadAllTextAsync("isme.json");

            List<dynamic> jokes = JsonConvert.DeserializeObject<List<dynamic>>(json);

            Random random = new Random();
            dynamic randomJoke = jokes[random.Next(jokes.Count)];

            await context.Channel.SendMessageAsync("```!Запомните ребята, безделье - игрушка дьявола,и так к анекдоту:```\n" + randomJoke.joke.ToString());
        }
        [Command("sila")]
        public async Task GenericCitation(CommandContext context)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("http://127.0.0.1:5184/getrandom");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    await context.Channel.SendMessageAsync(content);
                }
                else
                {
                    await context.Channel.SendMessageAsync("Произошла ошибка при получении анекдота.");
                }
            }
        }

        [Command("dayparol")]
        public async Task SendPassword(CommandContext ctx)
        {
            var DMPass = await ctx.Member.CreateDmChannelAsync();
            HttpClient client = new HttpClient();
            string guid = Guid.NewGuid().ToString()[..8];
            UserDTO user = new UserDTO()
            {
                Name = ctx.Member.Username,
                Password = guid,
                Validator = 765123

            };
            var usertemp = await client.PostAsync("http://127.0.0.1:5184/User/registration", JsonContent.Create(user));

            await DMPass.SendMessageAsync($"Твой Логин: {ctx.Member.Username}\nТвой пароль: {guid}");

        }


        [Command("help")]
        public async Task GetCommands(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Команды который существуют" +
                "\n!dayparol - для получение своего логина и пароля" +
                "\n!sila - для получение рандомной АУФ цитатки добавленные пользователями" +
                "\n!daygazu - для получение анекдота(рандомно)" +
                "\n");
        }

    }
}