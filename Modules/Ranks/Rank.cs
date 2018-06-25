﻿using System.Threading.Tasks;
using System.Xml;
using Discord.Commands;
using SuperBot_2._0.Services;
using SuperBotDLL1_0.RankingSystem;
using Discord;
using Discord.WebSocket;
using SuperBot_2_0;
using System;
using System.IO;
using System.Collections.Generic;

namespace SuperBot_2._0.Modules.Ranks
{
    public class Rank : ModuleBase
    {
        DiscordSocketClient client = Program._client;

        [Command("rank")]
        public async Task GetRank(IUser user = null)
        {
            await ReplyAsync("", false, Ranking.GetRank(Context, user));
        }

        [Command("status"), RequireOwner]
        public async Task Status(IUser user = null)
        {
            EmbedBuilder builder = new EmbedBuilder
            {
                Color = Color.Blue
            };
            builder = Ranking.GetMineRank(builder, Context, user);
            builder = Ranking.GetPickRank(builder, Context, user);
            builder = Ranking.GetCraftRank(builder, Context, user);
            await ReplyAsync("", false, builder.Build());
        }

        [Command("leaderboard"), Alias("lb")]
        public async Task LeaderBoard()
        {
            string[] golb = RankUtils.GloLeaderBoard;
            IUser first = client.GetUser(ulong.Parse(golb[0]));
            IUser second = client.GetUser(ulong.Parse(golb[2]));
            IUser third = client.GetUser(ulong.Parse(golb[4]));
            EmbedBuilder builder = new EmbedBuilder();
            builder.AddField("global Leaderboard", $"#1  {first.Username} lvl: {golb[1]}" + $"\n#2 {second.Username} lvl: {golb[3]}" + $"\n#3 {third.Username} lvl: {golb[5]}");
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
            LevelUser user = new LevelUser();
            user.Load(Context.User.Id);
            await ReplyAsync(user.Daily());
            user.Save(Context.User.Id);
        }

        [Command("resetdaily"), RequireOwner]
        public async Task Reset()
        {
            if (Context.User.Id == 245140333330038785)
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
        public async Task Mine(int amount = 1)
        {
            if (amount >= 0 && amount <= 10)
            {
                EmbedBuilder embed = new EmbedBuilder
                {
                    Color = Color.Green
                };
                LevelUser user = new LevelUser();
                user.Load(Context.User.Id);
                Random rand = new Random();
                string path = Program.levelpath + Context.User.Id + ".xml";
                user.GainXpMine(embed, Context.User);
                user.Save(Context.User.Id);
                await ReplyAsync("", false, Ranking.Miner(embed, path, Context.User.Id.ToString(), amount, Program.mineinv).Build());
                if (File.Exists($"./{Context.User.Id}.png"))
                    File.Delete($"./{Context.User.Id}.png");
            }
            else
                await ReplyAsync("Sorry but I can't pick that much stuf");
        }

        [Command("pick")]
        public async Task Pick(int amount = 1)
        {
            if (amount >= 1 && amount <= 100)
            {
                EmbedBuilder embed = new EmbedBuilder
                {
                    Color = Color.Green
                };
                LevelUser user = new LevelUser();
                user.Load(Context.User.Id);
                Random rand = new Random();
                string path = Program.levelpath + Context.User.Id + ".xml";
                user.GainXpPick(embed, Context.User);
                user.Save(Context.User.Id);
                await ReplyAsync("", false, Ranking.Picker(embed ,path, Context.User.Id.ToString(), amount, Program.baginv).Build());
            }
            else
                await ReplyAsync("Sorry but I can't pick that much stuf");
        }

        [Command("bag"), Alias("inventory")]
        public async Task Bag(string type = "types")
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Program.levelpath + Context.User.Id + ".xml");
            XmlNode node = doc.SelectSingleNode($"user/ID{Context.User.Id}");
            EmbedBuilder builder = Ranking.Bag(node, type);
            await ReplyAsync("", false, builder.Build());
        }

        [Command("give")]
        public async Task Give(IUser user, double amount)
        {
            if (Context.User == user)
                await ReplyAsync("You have it already");
            else
            {
                LevelUser user1 = new LevelUser();
                LevelUser user2 = new LevelUser();
                user1.Load(Context.User.Id);
                user2.Load(user.Id);
                var UserList = new List<IUser>
                {
                    await Context.Client.GetUserAsync(user1.UserId),
                    await Context.Client.GetUserAsync(user2.UserId)
                };
                await ReplyAsync("", false, user1.Give(user2, amount, UserList).Build());
                user1.Save(user1.UserId);
                user2.Save(user2.UserId);
            }
        }

        [Command("forcelevel"), RequireOwner]
        public async Task LevelUpUser()
        {
            LevelUser user = new LevelUser();
            user.Load(Context.User.Id);
            user.LevelUpUser(Context.User);
            user.Save(Context.User.Id);
            await ReplyAsync("succes!");
        }
    }
}
