using System.Threading.Tasks;
using System.Xml;
using Discord.Commands;
using SuperBot_1_0.Services;
using SuperBotDLL1_0.RankingSystem;
using Discord;
using Discord.WebSocket;

namespace SuperBot_1_0.Modules.Ranks
{
    public class Rank : ModuleBase
    {
        DiscordSocketClient client = Program._client;

        [Command("rank")]
        public async Task GetRank(IUser user = null)
        {
            await ReplyAsync(RankUtils.Rank(Context, user));
        }

        [Command("leaderboard"), Alias("lb")]
        public async Task LeaderBoard()
        {
            string[] golb = RankUtils.GloLeaderBoard;
            IUser first = client.GetUser(ulong.Parse(golb[0]));
            IUser second = client.GetUser(ulong.Parse(golb[2]));
            IUser third = client.GetUser(ulong.Parse(golb[4]));
            EmbedBuilder builder = new EmbedBuilder();
            builder.AddField("global Leaderboard", $"#1  {first.Mention} lvl: {golb[1]}" + $"\n#2 {second.Mention} lvl: {golb[3]}" + $"\n#3 {third.Mention} lvl: {golb[5]}");
            //string[] lolb = RankUtils.LocLeaderBoard(Context, client);
            //IUser lofirst = client.GetUser(ulong.Parse(lolb[0]));
            //IUser losecond = client.GetUser(ulong.Parse(lolb[2]));
            //IUser lothird = client.GetUser(ulong.Parse(lolb[4]));
            //builder.AddField("global Leaderboard", $"#1  {lofirst.Mention} lvl: {lolb[1]}" + $"\n#2 {losecond.Mention} lvl: {lolb[3]}" + $"\n#3 {lothird.Mention} lvl: {lolb[5]}");
            await ReplyAsync("", false, builder.Build());
        }
        
        [Command("daily")]
        public async Task Daily()
        {
            await ReplyAsync(RankUtils.Daily(Context));
        }

        [Command("resetdaily"), RequireOwner]
        public async Task Reset()
        {
            if(Context.User.Id == 245140333330038785)
            {
                RankUtils.ResetDaily();
                await ReplyAsync("all dailys have been reset");
            }
            else
            {
                await ReplyAsync("Sorry but only the bot owner can use this command");
            }
        }

        [Command("mine")]
        public async Task Mine()
        {
            string path = "./file/ranks/ranking.xml";
            string[] mineinv = { "stone", "goldore", "ironore", "gem", "coal", "oil", "sand" };
            await ReplyAsync(Ranking.Miner(path, Context.User.Id.ToString(), mineinv));
        }

        [Command("pick")]
        public async Task Pick()
        {
            string path = "./file/ranks/ranking.xml";
            string[] baginv = { "apple", "avocado", "banana", "carrot", "cherries", "chillies", "corn",
            "cucumber", "egg", "eggplant", "grain", "grape", "kiwi", "lemon", "melon", "milk", "orange",
            "peach", "peanuts", "pear", "pineapple", "potato", "strawberry", "sugarcane", "tomato" };
            await ReplyAsync(Ranking.Picker(path, Context.User.Id.ToString(), baginv));
    }

        [Command("bag"), Alias("inventory")]
        public async Task Bag(string type = "types")
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("./file/ranks/ranking.xml");
            XmlNode node = doc.SelectSingleNode($"root/users/ID{Context.User.Id}");
            EmbedBuilder builder = Ranking.Bag(node, type);
            await ReplyAsync("", false, builder.Build());
        }

        //public static void Level(DiscordSocketClient client)
        //{
        //    Random rand = new Random();
        //    client.MessageReceived += (arg =>
        //    {
        //        if (arg.Author.IsBot) return null;
        //        else
        //        {
        //            RankUtils.MessageRecieved(arg, rand);
        //        }
        //        return null;
        //    });
        //}
    }
}
