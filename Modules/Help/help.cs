using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SuperBot_2._0.Modules.Help
{
    public class Help : ModuleBase
    {
        private readonly CommandService _service;
        public Help(CommandService service)
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
                    builder.Title = "These are the modules you can use";
                    builder.Description = "To see the commands in a module use %module (module name)";
                    builder.Color = new Color(114, 137, 218);
                    string modules = null;
                    foreach (ModuleInfo module in _service.Modules)
                    {
                        modules += $"{module.Name}\n";
                    }
                    builder.AddField(x =>
                    {
                        x.Name = _service.Modules.Count() + " Modules";
                        x.Value = modules;
                        x.IsInline = true;
                    });
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

                    foreach (CommandMatch match in result.Commands)
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
