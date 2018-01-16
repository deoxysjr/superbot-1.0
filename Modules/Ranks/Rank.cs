using System;
using System.Threading.Tasks;
using Discord.Commands;
using SuperBot_1_0.Services;
using Discord;
using Discord.WebSocket;

namespace SuperBot_1_0.Modules.Ranks
{
    public class Rank : ModuleBase
    {
        [Command("rank")]
        public async Task rank(IUser user = null)
        {
            await ReplyAsync(RankUtils.Rank(Context, user));
        }

        [Command("daily")]
        public async Task daily()
        {
            await ReplyAsync(RankUtils.daily(Context));
        }

        [Command("resetdaily"), RequireOwner]
        public async Task reset()
        {
            RankUtils.ResetDaily();
            await ReplyAsync("all dailys have been reset");
        }

        public static void level(DiscordSocketClient client)
        {
            Random rand = new Random();
            client.MessageReceived += (arg =>
            {
                if (arg.Author.IsBot) return null;
                else
                {
                    RankUtils.MessageRecieved(arg, rand);
                    //Console.WriteLine(arg.ToString());
                }
                return null;
            });
        }
    }
}
