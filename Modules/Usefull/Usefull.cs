using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using Discord;
using Discord.Commands;
using SuperBotDLL1_0.color;
using SuperBotDLL1_0.Untils;
using SuperBot_1_0.Services;
using System.Net.Http;
using Newtonsoft.Json;

namespace SuperBot_1_0.Modules.Usefull
{
    public class Usefull : ModuleBase<ICommandContext>
    {
        string appkey = "c411f4d6f0e7476ec59d40ab7000fbdb";

        [Command("weather")]
        public async Task weather([Remainder]string city)
        {
            if (city.Contains("_"))
                city.Replace('_', ' ');
            var builder = new EmbedBuilder();
            try
            {
                string lon; string lat; string country; string sunrise; string sunset; string sunuptime; string[] avetemp; string[] mintemp; string[] maxtemp; string humidity; string pressure; string windspeedname;
                string windspeed; string clouds; string visibility; string precipitation; string weatherID; string weathername; string weatherEmoji; string[] lastupdate; string cityname; string cityid;
                XmlDocument doc = new XmlDocument();
                doc.Load($"http://api.openweathermap.org/data/2.5/weather?q={city}&type=accurate&mode=xml&appid={appkey}");
                XmlNode node = doc.SelectSingleNode("current");
                cityid = node.SelectSingleNode("city").Attributes[0].InnerText;
                cityname = node.SelectSingleNode("city").Attributes[1].InnerText;
                lon = node.SelectSingleNode("city/coord").Attributes[0].InnerText;
                lat = node.SelectSingleNode("city/coord").Attributes[1].InnerText;
                country = node.SelectSingleNode("city/country").InnerText;
                sunrise = node.SelectSingleNode("city/sun").Attributes[0].InnerText;
                sunset = node.SelectSingleNode("city/sun").Attributes[1].InnerText;
                sunuptime = Weathercmd.SunUptime(sunrise, sunset);
                avetemp = Weathercmd.TempAll(double.Parse(node.SelectSingleNode("temperature").Attributes[0].InnerText)).Split('_');
                mintemp = Weathercmd.TempAll(double.Parse(node.SelectSingleNode("temperature").Attributes[1].InnerText)).Split('_');
                maxtemp = Weathercmd.TempAll(double.Parse(node.SelectSingleNode("temperature").Attributes[2].InnerText)).Split('_');
                humidity = node.SelectSingleNode("humidity").Attributes[0].InnerText + "%";
                pressure = node.SelectSingleNode("pressure").Attributes[0].InnerText + " hPa";
                windspeedname = node.SelectSingleNode("wind/speed").Attributes[1].InnerText;
                windspeed = node.SelectSingleNode("wind/speed").Attributes[0].InnerText;
                var winddirct = new List<string>();
                for (int i = 0; i <= 2; i++)
                    winddirct.Add(node.SelectSingleNode("wind/direction").Attributes[i].InnerText);
                clouds = node.SelectSingleNode("clouds").Attributes[0].InnerText + "%, " + node.SelectSingleNode("clouds").Attributes[1].InnerText;
                visibility = node.SelectSingleNode("visibility").Attributes[0].InnerText;
                precipitation = Weathercmd.Precipitationweather(node.SelectSingleNode("precipitation"));
                Dictionary<int, string> emoji = new Dictionary<int, string>() { {200, ":thunder_cloud_rain:" }, {201, ":thunder_cloud_rain:" }, {202, ":thunder_cloud_rain:" }, {210, ":thunder_cloud_rain:" }, {211, ":thunder_cloud_rain:" }, {212, ":thunder_cloud_rain:" }, {221, ":thunder_cloud_rain:" }, {230, ":thunder_cloud_rain:" }, {231, ":thunder_cloud_rain:" }, {232, ":thunder_cloud_rain:" },
                        { 300, ":cloud_rain:" }, {301, ":cloud_rain:" }, {302, ":cloud_rain:" }, {310, ":cloud_rain:" }, {311, ":cloud_rain:" }, {312, ":cloud_rain:" }, {313, ":cloud_rain:" }, {314, ":cloud_rain:" }, {321, ":cloud_rain:" },
                        { 500, ":white_sun_rain_cloud:" }, {501, ":white_sun_rain_cloud:" }, {502, ":white_sun_rain_cloud:" }, {503, ":white_sun_rain_cloud:" }, {504, ":white_sun_rain_cloud:" }, {511, ":cloud_snow:" }, {520, ":cloud_rain:" }, {521, ":cloud_rain:" }, {522, ":cloud_rain:" }, {531, ":cloud_rain:" },
                        { 600, ":cloud_snow:" }, {601, ":cloud_snow:" }, {602, ":cloud_snow:" }, {611, ":cloud_snow:" }, {612, ":cloud_snow:" }, {615, ":cloud_snow:" }, {616, ":cloud_snow:" }, {620, ":cloud_snow:" }, {621, ":cloud_snow:" }, {622, ":cloud_snow:" },
                        { 701, ":fog:" }, {711, ":fog:" }, {721, ":fog:" }, {731, ":fog:" }, {741, ":fog:" }, {751, ":fog:" }, {761, ":fog:" }, {762, ":fog:" }, {771, ":fog:" }, {781, ":fog:" }, {800, ":sunny:" }, {900, ":cloud_tornado: Get out of there" }, {903, ":flame:" }, {904, ":snowflake:" } };
                weatherID = node.SelectSingleNode("weather").Attributes[0].InnerText;
                weathername = node.SelectSingleNode("weather").Attributes[1].InnerText;
                weatherEmoji = Weathercmd.Emoji(emoji, weatherID, weathername);
                lastupdate = node.SelectSingleNode("lastupdate").Attributes[0].InnerText.Split('T');
                builder.Color = Discord.Color.Green;
                builder.Title = $"Weather for {cityname}, {country}  :flag_{country.ToLower()}:";
                builder.AddField(x =>
                {
                    x.Name = $"cityinfo";
                    x.Value = $"City id = {cityid}"
                    + $"\nLon = {lon}, Lat = {lat}"
                    + $"\nSunrise = {sunrise.Split('T')[1]}"
                    + $"\nSunup = {sunuptime}"
                    + $"\nSunset = {sunset.Split('T')[1]}";
                    x.IsInline = false;
                });
                builder.AddInlineField("Temperature", $"Average = {avetemp[1]}°C, {avetemp[2]}°F, {avetemp[0]}°K"
                    + $"\nminimum = {mintemp[1]}°C, {mintemp[2]}°F, {mintemp[0]}°K"
                    + $"\nmaximum = {maxtemp[1]}°C, {maxtemp[2]}°F, {maxtemp[0]}°K"
                    + $"\nHumidity = {humidity}"
                    + $"\nPressure = {pressure}");
                builder.AddField(x =>
                {
                    x.Name = "Wind";
                    x.Value = $"speed = {windspeedname}, {windspeed}m/s, {(double.Parse((windspeed).Replace(".", ",")) * 3.6).ToString().Replace(",", ".")}km/h"
                    + $"\ndirection = {winddirct[1]}, {winddirct[2]}, {winddirct[0]}°";
                    x.IsInline = false;
                });
                builder.AddInlineField("clouds", $"Clouds = {clouds}"
                    + $"Visibility = {visibility}m, {double.Parse(visibility) / 1000}km");
                builder.AddField(x =>
                {
                    x.Name = "Precipitation";
                    x.Value = $"precipitation = {precipitation}";
                    x.IsInline = false;
                });
                builder.AddInlineField("weather icon", weatherEmoji + $"\n weatherId = {weatherID}");
                builder.AddField(x =>
                {
                    x.Name = "Last up date";
                    x.Value = $"{lastupdate[0]} at {lastupdate[1]}";
                    x.IsInline = false;
                });

                await ReplyAsync("", false, builder.Build());
            }
            catch (Exception ex)
            {
                builder.Color = Discord.Color.Red;
                builder.AddField(x =>
                {
                    x.Name = "Error";
                    x.Value = ex.Message.ToString();
                    x.IsInline = false;
                });
                await ReplyAsync("", false, builder.Build());
            }

        }

        [Command("forecast")]
        public async Task forecast([Remainder]string city)
        {
            var builder = new EmbedBuilder();
            try
            {
                builder.Color = Discord.Color.Green;
                XmlDocument doc = new XmlDocument();
                doc.Load($"http://api.openweathermap.org/data/2.5/forecast?q={city}&mode=xml&appid={appkey}");
                XmlNode fore = doc.SelectSingleNode("weatherdata/forecast");
                foreach (XmlNode node in fore)
                {
                    string[] temp; //string precipitation; //string clouds; string winddir; string windspeed;
                    temp = Weathercmd.TempAll(double.Parse(node.SelectSingleNode("temperature").Attributes[1].InnerText)).Split('_');
                    //precipitation = Weathercmd.Precipitationforecast(node.SelectSingleNode("precipitation"));
                    //builder.AddInlineField("test", precipitation);

                    builder.AddInlineField($"{node.Attributes[0].InnerText.Replace('T', ' ')} to {node.Attributes[1].InnerText.Replace('T', ' ')}", $"Temperature = {temp[1]}°C, {temp[2]}°F");
                    //+ $"\nPrecipitation = {precipitation}");
                }
                await ReplyAsync("", false, builder.Build());
            }
            catch (Exception ex)
            {
                builder.Color = Discord.Color.Red;
                builder.AddField(x =>
                {
                    x.Name = "Error";
                    x.Value = ex.Message.ToString();
                    x.IsInline = false;
                });
                await ReplyAsync("", false, builder.Build());
            }
        }

        [Command("color")]
        public async Task color(params string[] arg)
        {
            string img = @".\1024x1024.jpg";
            Bitmap bmp = new Bitmap(img);
            string arg1 = arg[0];
            int width = bmp.Width;
            int height = bmp.Height;

            if (arg1 == "red")
            {
                Sendcolor.ColorRed(width, height, Context, bmp);
                return;
            }
            if (arg1 == "lightred")
            {
                Sendcolor.ColorLightRed(width, height, Context, bmp);
                return;
            }
            if (arg1 == "green")
            {
                Sendcolor.ColorGreen(width, height, Context, bmp);
                return;
            }
            if (arg1 == "lightgreen")
            {
                Sendcolor.ColorLightGreen(width, height, Context, bmp);
                return;
            }
            if (arg1 == "blue")
            {
                Sendcolor.ColorBlue(width, height, Context, bmp);
                return;
            }
            if (arg1 == "lightblue")
            {
                Sendcolor.ColorLightBlue(width, height, Context, bmp);
                return;
            }
            if (arg1 == "black" || arg1 == "nigger")
            {
                Sendcolor.ColorBlack(width, height, Context, bmp);
                return;
            }
            if (arg1 == "White")
            {
                Sendcolor.ColorWhite(width, height, Context, bmp);
                return;
            }
            if (arg1 == "lightgray")
            {
                Sendcolor.ColorLightGray(width, height, Context, bmp);
                return;
            }
            if (arg1 == "gray")
            {
                Sendcolor.ColorGray(width, height, Context, bmp);
                return;
            }
            if (arg1 == "yellow")
            {
                Sendcolor.ColorYellow(width, height, Context, bmp);
                return;
            }
            if (arg1 == "orange")
            {
                Sendcolor.ColorOrange(width, height, Context, bmp);
                return;
            }
            if (arg1 == "purple")
            {
                Sendcolor.ColorPurple(width, height, Context, bmp);
                return;
            }
            if (arg1 == "random")
            {
                Sendcolor.ColorRandom(width, height, Context, bmp);
                return;
            }
            if (arg1 == "randomall")
            {
                Sendcolor.ColorRandomAll(width, height, Context, bmp);
                return;
            }
            if (arg1 == "randomvret")
            {
                Sendcolor.ColorRandomVret(width, height, Context, bmp);
                return;
            }
            if (arg1 == "randomhor")
            {
                Sendcolor.ColorRandomHor(width, height, Context, bmp);
                return;
            }

            if (arg[0].Contains("#"))
            {
                string colorcode = arg[0];
                var code = colorcode.Replace("#", "#ff");
                System.Drawing.Color color = ColorTranslator.FromHtml($"{code}");

                int R = color.R;
                int G = color.G;
                int B = color.B;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        System.Drawing.Color p = bmp.GetPixel(x, y);

                        int a = p.A;

                        bmp.SetPixel(x, y, System.Drawing.Color.FromArgb(a, R, G, B));
                    }
                }
                try
                {
                    bmp.Save(@".\\color.png");
                }
                catch (Exception Ex)
                {
                    Console.WriteLine(Ex);
                }
                await Context.Channel.SendFileAsync(@".\color.png");
                if (File.Exists(@"./color.png"))
                {
                    File.Delete(@"./color.png");
                }
                return;
            }
            else if (arg[1] != "")
            {
                int R = int.Parse(arg[0]);
                int G = int.Parse(arg[1]);
                int B = int.Parse(arg[2]);

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        System.Drawing.Color p = bmp.GetPixel(x, y);

                        int a = p.A;

                        bmp.SetPixel(x, y, System.Drawing.Color.FromArgb(a, R, G, B));
                    }
                }
                try
                {
                    bmp.Save(@".\\color.png");
                }
                catch (Exception Ex)
                {
                    Console.WriteLine(Ex);
                }
                await Context.Channel.SendFileAsync(@".\color.png");
                if (File.Exists(@"./color.png"))
                {
                    File.Delete(@"./color.png");
                }
            }
        }

        [Command("convert")]
        public async Task convert(params string[] code)
        {
            if (code[0].Contains("#"))
            {
                System.Drawing.Color color = ColorTranslator.FromHtml($"{code[0]}");
                int R = color.R;
                int G = color.G;
                int B = color.B;

                await ReplyAsync($"R:{R} G:{G} B:{B}");
            }
            else
            {
                int r = int.Parse(code[0]);
                int g = int.Parse(code[1]);
                int b = int.Parse(code[2]);

                System.Drawing.Color color = System.Drawing.Color.FromArgb(r, g, b);

                string hex = color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");

                await ReplyAsync($"%color #{hex}");
            }
        }

        [Command("plot")]
        public async Task plot()
        {
            Bitmap myBitmap = new Bitmap(@"./1024x1024.jpg");
            Graphics g = Graphics.FromImage(myBitmap);

            g.DrawLine(Pens.Gray, 1, 1, 200, 400);
            myBitmap.Save(@"./text.png");
            await Context.Channel.SendFileAsync(@"./text.png");
            if (File.Exists(@"./text.png"))
            {
                File.Delete(@"./text.png");
            }
        }

        [Command("draw")]
        public async Task draw(string arg)
        {
            Bitmap myBitmap = new Bitmap(@"./1024x1024.jpg");
            Graphics g = Graphics.FromImage(myBitmap);

            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.DrawString($"{arg}", new Font("Alien Encounters", 30), Brushes.Black, new PointF(50, 50));
            myBitmap.Save(@"./text.png");
            await Context.Channel.SendFileAsync(@"./text.png");
            if (File.Exists(@"./text.png"))
            {
                File.Delete(@"./text.png");
            }
        }

        [Command("anime")]
        public async Task searchAnime([Remainder]string anime)
        {
            string url = $@"https://aniapi.nadekobot.me/anime/{anime}";
            string res;
            var list = new List<string>();
            var builder = new EmbedBuilder()
            {
                Color = new Discord.Color(0, 255, 0)
            };

            try
            {
                using (var http = new HttpClient())
                {
                    res = await http.GetStringAsync(url).ConfigureAwait(false);
                    var json = JsonConvert.DeserializeObject<dynamic>(res);
                    list.Add("English Title: " + json.title_english.ToString());
                    list.Add("Japanese title: " + json.title_japanese.ToString());
                    list.Add("Romaji title: " + json.title_romaji.ToString());
                    list.Add("Synonyms: " + string.Join(", ", json.synonyms));
                    list.Add("Type: " + json.type.ToString());
                    list.Add("Average score: " + json.average_score.ToString());
                    list.Add("status: " + json.airing_status.ToString());
                    list.Add("Episodes: " + json.total_episodes.ToString());
                    list.Add("Duration: " + json.duration.ToString());
                    list.Add("Total time: " + TotalTime(json.total_episodes.ToString(), json.duration.ToString()));
                    list.Add("Adult: " + json.adult);
                    string description = json.description.ToString().Replace("<br>", "");
                    builder.AddField(x =>
                    {
                        x.Name = $"**{anime}**";
                        x.Value = string.Join("\n", list);
                        x.IsInline = false;
                    });
                    builder.AddField(x =>
                    {
                        x.Name = "**Description**";
                        x.Value = description;
                        x.IsInline = false;
                    });
                    //builder.WithImageUrl(json.image_url_lge.ToString());
                    builder.ImageUrl = json.image_url_lge.ToString();
                }
            }
            catch (Exception ex)
            {
                builder.Color = new Discord.Color(255, 0, 0);
                builder.AddField(x =>
                {
                    x.Name = "**Error**";
                    x.Value = ex.Message.ToString();
                    x.IsInline = false;
                });
                await ReplyAsync("", false, builder.Build());
            }

            await ReplyAsync("", false, builder.Build());
        }

        [Command("Encrypt"), RequireOwner]
        public async Task encrypt([Remainder]string text)
        {
            File.AppendAllText("./encryptedmessage.txt", Encrypt.EncryptText(text));
            await Context.Channel.SendFileAsync("./encryptedmessage.txt");
            if (File.Exists("./encryptedmessage.txt"))
                File.Delete("./encryptedmessage.txt");
        }

        [Command("Decrypt"), RequireOwner]
        public async Task decrypt([Remainder]string text)
        {
            try
            {
                string output = Decrypt.ToText(text);
                if (output.Contains("error"))
                    await ReplyAsync(output);
                else
                {
                    File.AppendAllText("./decryptedmessage.txt", output);
                    await Context.Channel.SendFileAsync("./decryptedmessage.txt");
                    if (File.Exists("./decryptedmessage.txt"))
                        File.Delete("./decryptedmessage.txt");
                }
            }
            catch (Exception ex)
            {
                await ReplyAsync(ex.Message);
            }
        }

        public string TotalTime(string eps, string dur)
        {
            int time_in_sec = int.Parse(dur) * 60;
            int Totaltime = time_in_sec * int.Parse(eps);
            return Utils.CalculateTimeWithSeconds(Totaltime);
        }
    }
}
