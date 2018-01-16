using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SuperBot_1_0.Modules.help
{
    public class help : ModuleBase
    {
        private readonly CommandService _service;
        public help(CommandService service)
        {
            _service = service;
        }

        [Command("help")]
        public async Task HelpAsync(string command = null)
        {
            var builder = new EmbedBuilder();
            try
            {
                if (command == null)
                {
                    string prefix = "%";
                    builder.Description = "These are the commands you can use";
                    builder.Color = new Color(114, 137, 218);

                    foreach (var module in _service.Modules)
                    {
                        string description = null;
                        foreach (var cmd in module.Commands)
                        {
                            var result = await cmd.CheckPreconditionsAsync(Context);
                            if (result.IsSuccess)
                                description += $"{prefix}{cmd.Aliases.First()}\n";
                        }
                        builder.AddField(x =>
                        {
                            x.Name = module.Name + " " + module.Commands.Count;
                            x.Value = description;
                            x.IsInline = true;
                        });
                    }

                    await ReplyAsync("", false, builder.Build());
                }
                else
                {
                    var result = _service.Search(Context, command);

                    if (!result.IsSuccess)
                    {
                        await ReplyAsync($"Sorry, I couldn't find a command like **{command}**.");
                        return;
                    }

                    foreach (var match in result.Commands)
                    {
                        var cmd = match.Command;

                        builder.AddField(x =>
                        {
                            x.Name = string.Join(", ", cmd.Aliases);
                            x.Value = $"Parameters: {string.Join(", ", cmd.Parameters.Select(p => p.Name))}\n" +
                                      $"Summary: {cmd.Summary}";
                            x.IsInline = false;
                        });
                    }

                    await ReplyAsync("", false, builder.Build());
                }
            }
            catch (Exception ex)
            {
                builder.Color = Color.Red;
                builder.AddField(x =>
                {
                    x.Name = "error";
                    x.Value = ex.Message;
                    x.IsInline = true;
                });
                await ReplyAsync("", false, builder.Build());
            }
        }

        //[Command("help")]
        //public async Task HelpAsync(string command)
        //{
        //    var result = _service.Search(Context, command);

        //    if (!result.IsSuccess)
        //    {
        //        await ReplyAsync($"Sorry, I couldn't find a command like **{command}**.");
        //        return;
        //    }

        //    var builder = new EmbedBuilder()
        //    {
        //        Color = new Color(114, 137, 218),
        //        Description = $"Here are some commands like **{command}**"
        //    };

        //    foreach (var match in result.Commands)
        //    {
        //        var cmd = match.Command;

        //        builder.AddField(x =>
        //        {
        //            x.Name = string.Join(", ", cmd.Aliases);
        //            x.Value = $"Parameters: {string.Join(", ", cmd.Parameters.Select(p => p.Name))}\n" +
        //                      $"Summary: {cmd.Summary}";
        //            x.IsInline = false;
        //        });
        //    }

        //    await ReplyAsync("", false, builder.Build());
        //}

        [Command("module")]
        public async Task Helpasync(string module)
        {
            var res = _service.Modules;
            string prefix = "%"; 

            var builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                Description = $"This are the commands in **{module}**"
            };

            foreach (var match in res)
            {
                if (match.Name.ToLower() == module.ToLower())
                {
                    string description = null;
                    foreach (var cmd in match.Commands)
                    {
                        var result = await cmd.CheckPreconditionsAsync(Context);
                        if (result.IsSuccess)
                            description += $"{prefix}{cmd.Aliases.First()}\n";
                    }
                    builder.AddField(x =>
                    {
                        x.Name = match.Name;
                        x.Value = description;
                        x.IsInline = false;
                    });
                }
                
            }

            await ReplyAsync("", false, builder.Build());
        }
    }
}
