﻿using Discord;
using Discord.Commands;
using SuperBotDLL1_0.Gambling;
using SuperBotDLL1_0.RankingSystem;
using System;
using System.Threading.Tasks;
using System.Xml;

namespace SuperBot_2._0.Modules.Gambling
{
    public class Gambling : ModuleBase
    {
        [Command("roll")]
        public async Task Roll(int max, int guess, double bid, int times = 1)
        {
            LevelUser user = new LevelUser();
            user.Load(Context.User.Id);
            if (times > 10)
                await ReplyAsync("I can't handle that many times");
            else if (max <= 1)
                await ReplyAsync("I won't allowe you to do that");
            else if (guess < 1 || guess > max)
                await ReplyAsync($"the number you guessed isn't even betwean 1-{max}");
            else if (user.Credits < bid * double.Parse(times.ToString()))
                await ReplyAsync("you don't even have that much credits");
            else
                await ReplyAsync("", false, Gamble.Roll(Context.User, max, guess, bid, times).Build());
        }

        [Command("slot")]
        public async Task Slot(double amount)
        {
            LevelUser user = new LevelUser();
            user.Load(Context.User.Id);
            if (user.Credits < amount)
                await ReplyAsync("you don't even have that many credits");
            else
            {
                await ReplyAsync("", false, Gamble.Slot(user, amount));
                user.Save(Context.User.Id);
            }
        }

        [Command("horserace create")]
        public async Task Create()
        {
            await ReplyAsync(await Races.CreateRaceAsync(Context.Guild.Id.ToString(), "3"));
        }

        [Command("horserace join")]
        public async Task Join(double bid, int racer)
        {
            LevelUser user = new LevelUser();
            user.Load(Context.User.Id);
            if (user.Credits < bid)
                await ReplyAsync("you don't even have that much credits");
            else
                await ReplyAsync(Races.JoinRace(Context.Guild.Id.ToString(), Context.User.Id.ToString(), bid, racer));
        }

        [Command("horserace start")]
        public async Task Start()
        {
            await ReplyAsync("not done yet");
        }
    }
}
