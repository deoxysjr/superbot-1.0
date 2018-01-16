﻿using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord;
using System.Linq;

namespace SuperBot_1_0.Modules.Admin
{
    public class Admin : ModuleBase
    {
        [Command("playing"), RequireOwner]
        [Alias("play")]
        public async Task play(string game)
        {
            if (game == "time")
            {
                var time = $"{DateTime.Now,-19}";
                await Program._client.SetGameAsync(time);
            }
            else
                await Program._client.SetGameAsync(game);
        }

        [Command("clear"), RequireBotPermission(GuildPermission.ManageMessages), RequireContext(ContextType.Guild)]
        [Alias("clr")]
        public async Task Deleteasync(string count = null)
        {
            try
            {
                if(count == null)
                {
                    var messageList = await Context.Channel.GetMessagesAsync().Flatten();
                    int num = messageList.Count();
                    await Context.Channel.DeleteMessagesAsync(messageList);
                    await ReplyAsync($"Deleted the last {num} messages.");
                    Services.CommandUsed.ClearAdd(num);
                    var message = await Context.Channel.GetMessagesAsync(1).Flatten();
                    await Task.Delay(1000);
                    await Context.Channel.DeleteMessagesAsync(message);
                }
                else if(int.Parse(count) < 101)
                {
                    var messageList = await Context.Channel.GetMessagesAsync(int.Parse(count)).Flatten();
                    int num = messageList.Count();
                    await Context.Channel.DeleteMessagesAsync(messageList);
                    await ReplyAsync($"Deleted the last {num} messages.");
                    Services.CommandUsed.ClearAdd(num);
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
    }
}
