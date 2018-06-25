using System;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using Discord;
using Discord.Commands;
using SuperBot_2_0;
using SuperBotDLL1_0.Untils;

namespace SuperBot_2._0.Modules.Admin
{
    [Name("Admin")]
    public class Admin : ModuleBase
    {
        static List<float> AvailableCPU = new List<float>();
        static List<float> AvailableRAM = new List<float>();
        protected static PerformanceCounter cpuCounter;
        protected static PerformanceCounter ramCounter;
        static List<PerformanceCounter> cpuCounters = new List<PerformanceCounter>();
        static List<PerformanceCounter> core = new List<PerformanceCounter>();

        [Command("playing")]
        [Alias("play")]
        public async Task Play(string game)
        {
            if (Context.User.Id == 245140333330038785)
            {
                if (game == "time")
                {
                    var time = $"{DateTime.Now,-19}";
                    await Program._client.SetGameAsync(time);
                }
                else
                    await Program._client.SetGameAsync(game);
            }
            else
            {
                await ReplyAsync("Sorry but only the bot owner can use this command");
            }
        }

        [Command("clear"), RequireBotPermission(GuildPermission.ManageMessages), RequireContext(ContextType.Guild)]
        [Alias("clr")]
        public async Task Deleteasync(string count = null)
        {
            try
            {
                if (count == null)
                {
                    var messageList = await Context.Channel.GetMessagesAsync().Flatten();
                    int num = messageList.Count();
                    await Context.Channel.DeleteMessagesAsync(messageList);
                    await ReplyAsync($"Deleted the last {num} messages.");
                    Services.CommandUsed.ClearAdd(num + 1);
                    var message = await Context.Channel.GetMessagesAsync(1).Flatten();
                    await Task.Delay(1000);
                    await Context.Channel.DeleteMessagesAsync(message);
                }
                else if (int.Parse(count) < 101)
                {
                    var messageList = await Context.Channel.GetMessagesAsync(int.Parse(count)).Flatten();
                    int num = messageList.Count();
                    await Context.Channel.DeleteMessagesAsync(messageList);
                    await ReplyAsync($"Deleted the last {num} messages.");
                    Services.CommandUsed.ClearAdd(num + 1);
                    var message = await Context.Channel.GetMessagesAsync(1).Flatten();
                    await Task.Delay(1000);
                    await Context.Channel.DeleteMessagesAsync(message);
                }
                else
                {
                    await ReplyAsync("sorry but 100 is the maximum");
                }
            }
            catch
            {
                await ReplyAsync("Couldn't delete messages: Insufficient role");
            }
        }

        [Command("invite"), RequireBotPermission(GuildPermission.CreateInstantInvite), RequireUserPermission(GuildPermission.CreateInstantInvite)]
        public async Task CreateInvite()
        {
            await ReplyAsync("https://discord.gg/nZFVvTW");
        }

        [Command("uptime"), Alias("status")]
        public async Task UpTime()
        {
            EmbedBuilder builder = new EmbedBuilder
            {
                Title = "Status",
                Color = Color.LightGrey
            };

            var uptime = DateTime.Now - Program.StartupTime;
            builder.AddField("Uptime", $"I'm now up for \n{Other.CalculateTimeWithSeconds((int)Math.Round(uptime.TotalSeconds, 0))}");
            //builder = Other.GetCpuPreformance(builder);
            await ReplyAsync("", false, builder.Build());
        }

        [Command("guild"), RequireUserPermission(GuildPermission.KickMembers)]
        public async Task Users(int days)
        {
            int users = await Context.Guild.PruneUsersAsync(days, true);
            await ReplyAsync($"{users} users have not been online for {days} days");
        }

        [Command("addchannel"), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task AddChannel(IChannel channel)
        {
            try
            {
                GuildChannel guild = new GuildChannel(Context.Guild.Id);
                guild.Add(Context.Guild.Id, channel.Id);
                await ReplyAsync($"{channel.Name} has been added to the list");
            }
            catch (Exception ex)
            {
                await ReplyAsync(ex.Message);
            }
        }

        [Command("removechannel"), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task RemChannel(IChannel channel)
        {
            try
            {
                GuildChannel guild = new GuildChannel(Context.Guild.Id);
                guild.Remove(Context.Guild.Id, channel.Id);
                await ReplyAsync($"{channel.Name} has been removed from the list");
            }
            catch (Exception ex)
            {
                await ReplyAsync(ex.Message);
            }
        }

        [Command("disable"), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task DisGuild()
        {
            try
            {
                GuildChannel guild = new GuildChannel(Context.Guild.Id);
                guild.Disable(Context.Guild.Id);
                await ReplyAsync("this feature is now disabled on this guild");
            }
            catch (Exception ex)
            {
                await ReplyAsync(ex.Message);
            }
        }

        [Command("enable"), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task EnGuild()
        {
            try
            {
                GuildChannel guild = new GuildChannel(Context.Guild.Id);
                guild.Enable(Context.Guild.Id);
                await ReplyAsync("this feature is now enabled on this guild");
            }
            catch (Exception ex)
            {
                await ReplyAsync(ex.Message);
            }
        }

        [Command("list"), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task ListChannels()
        {
            List<string> list = new List<string>();
            EmbedBuilder builder = new EmbedBuilder();
            GuildChannel guild = new GuildChannel(Context.Guild.Id);
            foreach (ulong channelid in guild.Channels)
            {
                var channel = await Context.Client.GetChannelAsync(channelid);
                list.Add("#" + channel.Name);
            }
            builder.Title = "this is the list of channels";
            builder.Description = string.Join("\n", list);
            await ReplyAsync("", false, builder.Build());
        }
    }
}
