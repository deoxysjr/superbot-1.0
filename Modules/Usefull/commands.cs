using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Xml;
using SuperBot_1_0.Services;
using SuperBotDLL1_0.RankingSystem;

namespace SuperBot_1_0.Modules
{
    public class commands : ModuleBase
    {
        [Command("test")]
        public async Task Test()
        {
            //Ranking.AddNewUserRank(@"./file/ranks/ranking.xml", Context.User.Id.ToString());
            await ReplyAsync("Hi " + Context.Message.Author.Mention);
        }

        
    }
}
