using System;
using System.Collections.Generic;
using System.IO;
using Discord.WebSocket;
using System.Xml;
using Discord.Commands;
using Discord;
using SuperBotDLL1_0.RankingSystem;

namespace SuperBot_1_0.Services
{
    class RankUtils
    {
        DiscordSocketClient client = Program._client;
        public static string[] levellist = { "currentxp", "needxp", "currentlvl", "nextlvl", "currentminelvl", "nextminelvl", "currentminexp",
                                             "needminexp", "currentpicklvl", "nextpicklvl", "currentpickxp", "needpickxp", "prestige", "credits", "lastdaily" };
        public static string[] mineinv = { "stone", "goldore", "ironore", "gem", "coal", "oil", "sand" };
        public static string[] baginv = { "apple", "avocado", "banana", "carrot", "cherries", "chillies", "corn",
            "cucumber", "egg", "eggplant", "grain", "grape", "kiwi", "lemon", "melon", "milk", "orange",
            "peach", "peanuts", "pear", "pineapple", "potato", "strawberry", "sugarcane", "tomato" };
        public static string[] craftlist = { "gold", "iron", "ring", "crown", "bakedegg", "flower", "sugar", "glass", "refinedoil" };

        public static string Rank(ICommandContext context, IUser user)
        {
            var list = new List<string>();
            string path = "./file/ranks/ranking.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            if (user == null)
            {
                IUser u = context.User;
                if (!u.IsBot)
                {
                    try
                    {
                        XmlNode node = doc.SelectSingleNode("root/users/ID" + u.Id.ToString());
                        string needed = node.SelectSingleNode("level/needxp").Attributes[0].InnerText;
                        string Current = node.SelectSingleNode("level/currentxp").Attributes[0].InnerText;
                        string Credits = node.SelectSingleNode("level/credits").Attributes[0].InnerText;
                        list.Add(u.Mention + "'s Level is " + node.SelectSingleNode("level/currentlvl").Attributes[0].InnerText);
                        double precent = Math.Round(double.Parse(Current) / double.Parse(needed) * 100, 0);
                        list.Add($"<{Current}/{needed}>XP {precent}%");
                        if (precent == 0)
                            list.Add("0%(----------)100%");
                        if (precent >= 1 && precent <= 9)
                            list.Add("0%(=---------)100%");
                        if (precent >= 10 && precent <= 19)
                            list.Add("0%(==--------)100%");
                        if (precent >= 20 && precent <= 29)
                            list.Add("0%(===-------)100%");
                        if (precent >= 30 && precent <= 39)
                            list.Add("0%(====------)100%");
                        if (precent >= 40 && precent <= 49)
                            list.Add("0%(=====-----)100%");
                        if (precent >= 50 && precent <= 59)
                            list.Add("0%(======----)100%");
                        if (precent >= 60 && precent <= 69)
                            list.Add("0%(=======---)100%");
                        if (precent >= 70 && precent <= 79)
                            list.Add("0%(========--)100%");
                        if (precent >= 80 && precent <= 89)
                            list.Add("0%(=========-)100%");
                        if (precent >= 90 && precent <= 99)
                            list.Add("0%(==========)100%");
                        list.Add($"�{Credits}");
                        return string.Join("\n", list);
                    }
                    catch (Exception)
                    {
                        return $"sorry but {u.Mention} does not have a rank";
                    }
                }
                else
                {
                    return "Bots don't have ranks";
                }
            }
            else
            {
                if (user.Id == context.Client.CurrentUser.Id)
                {
                    string BankCredits = doc.SelectSingleNode("root/bank/credits").Attributes[0].InnerText;
                    return $"The bank has �{BankCredits}";
                }
                IUser u = user;
                if (!u.IsBot)
                {
                    try
                    {
                        XmlNode node = doc.SelectSingleNode("root/users/ID" + u.Id.ToString());
                        string needed = node.SelectSingleNode("level/needxp").Attributes[0].InnerText;
                        string Current = node.SelectSingleNode("level/currentxp").Attributes[0].InnerText;
                        string Credits = node.SelectSingleNode("level/credits").Attributes[0].InnerText;
                        list.Add(u.Mention + "'s Level is " + node.SelectSingleNode("level/currentlvl").Attributes[0].InnerText);
                        double precent = Math.Round(double.Parse(Current) / double.Parse(needed) * 100, 0);
                        list.Add($"<{Current}/{needed}>XP {precent}%");
                        if (precent == 0)
                            list.Add("0%(----------)100%");
                        if (precent >= 1 && precent <= 9)
                            list.Add("0%(=---------)100%");
                        if (precent >= 10 && precent <= 19)
                            list.Add("0%(==--------)100%");
                        if (precent >= 20 && precent <= 29)
                            list.Add("0%(===-------)100%");
                        if (precent >= 30 && precent <= 39)
                            list.Add("0%(====------)100%");
                        if (precent >= 40 && precent <= 49)
                            list.Add("0%(=====-----)100%");
                        if (precent >= 50 && precent <= 59)
                            list.Add("0%(======----)100%");
                        if (precent >= 60 && precent <= 69)
                            list.Add("0%(=======---)100%");
                        if (precent >= 70 && precent <= 79)
                            list.Add("0%(========--)100%");
                        if (precent >= 80 && precent <= 89)
                            list.Add("0%(=========-)100%");
                        if (precent >= 90 && precent <= 99)
                            list.Add("0%(==========)100%");
                        list.Add($"�{Credits}");
                        return string.Join("\n", list);
                    }
                    catch (Exception)
                    {
                        return $"sorry but {u.Mention} does not have a rank";
                    }
                }
                else
                {
                    return "Bots don't have ranks";
                }
            }
        }

        public static string[] GloLeaderBoard
        {
            get
            {
                string first = ""; int firstlvl = 0;
                string second = ""; int secondlvl = 0;
                string third = ""; int thirdlvl = 0;
                XmlDocument doc = new XmlDocument();
                doc.Load("./file/ranks/ranking.xml");
                XmlNode root = doc.SelectSingleNode("root/users");
                foreach (XmlNode node in root)
                {
                    if (int.Parse(node.SelectSingleNode("level/currentlvl").Attributes[0].InnerText) > firstlvl && firstlvl == 0)
                    {
                        first = $"{node.Name.Replace("ID", "")}";
                        firstlvl = int.Parse(node.SelectSingleNode("level/currentlvl").Attributes[0].InnerText);
                    }
                    else if (int.Parse(node.SelectSingleNode("level/currentlvl").Attributes[0].InnerText) > firstlvl)
                    {
                        third = second;
                        thirdlvl = secondlvl;
                        second = first;
                        secondlvl = firstlvl;
                        first = $"{node.Name.Replace("ID", "")}";
                        firstlvl = int.Parse(node.SelectSingleNode("level/currentlvl").Attributes[0].InnerText);
                    }
                    else if (int.Parse(node.SelectSingleNode("level/currentlvl").Attributes[0].InnerText) > secondlvl && secondlvl == 0)
                    {
                        second = $"{node.Name.Replace("ID", "")}";
                        secondlvl = int.Parse(node.SelectSingleNode("level/currentlvl").Attributes[0].InnerText);
                    }
                    else if (int.Parse(node.SelectSingleNode("level/currentlvl").Attributes[0].InnerText) > secondlvl)
                    {
                        third = second;
                        thirdlvl = secondlvl;
                        second = $"{node.Name.Replace("ID", "")}";
                        secondlvl = int.Parse(node.SelectSingleNode("level/currentlvl").Attributes[0].InnerText);
                    }
                    else if (int.Parse(node.SelectSingleNode("level/currentlvl").Attributes[0].InnerText) > thirdlvl)
                    {
                        third = $"{node.Name.Replace("ID", "")}";
                        thirdlvl = int.Parse(node.SelectSingleNode("level/currentlvl").Attributes[0].InnerText);
                    }
                }
                string[] lb = { first, firstlvl.ToString(), second, secondlvl.ToString(), third, thirdlvl.ToString() };
                return lb;
            }
        }

        public static string[] LocLeaderBoard(ICommandContext Context, DiscordSocketClient client)
        {
            string first = ""; int firstlvl = 0;
            string second = ""; int secondlvl = 0;
            string third = ""; int thirdlvl = 0;
            XmlDocument doc = new XmlDocument();
            doc.Load("./file/ranks/ranking.xml");
            XmlNode root = doc.SelectSingleNode("root/users");
            var list = new List<ulong>();
            foreach(var guild in client.Guilds)
            {
                if (guild.Id == Context.Guild.Id)
                {
                    foreach (var user in guild.Users)
                    {
                        list.Add(user.Id);
                    }
                }
            }

            foreach (ulong user in list)
            {
                if (int.Parse(root.SelectSingleNode($"{user}/level/currentlvl").Attributes[0].InnerText) > firstlvl && firstlvl == 0)
                {
                    first = $"{user}";
                    firstlvl = int.Parse(root.SelectSingleNode($"{user}/level/currentlvl").Attributes[0].InnerText);
                }
                else if (int.Parse(root.SelectSingleNode($"{user}/level/currentlvl").Attributes[0].InnerText) > firstlvl)
                {
                    third = second;
                    thirdlvl = secondlvl;
                    second = first;
                    secondlvl = firstlvl;
                    first = $"{user}";
                    firstlvl = int.Parse(root.SelectSingleNode($"{user}/level/currentlvl").Attributes[0].InnerText);
                }
                else if (int.Parse(root.SelectSingleNode($"{user}/level/currentlvl").Attributes[0].InnerText) > secondlvl && secondlvl == 0)
                {
                    second = $"{user}";
                    secondlvl = int.Parse(root.SelectSingleNode($"{user}/level/currentlvl").Attributes[0].InnerText);
                }
                else if (int.Parse(root.SelectSingleNode($"{user}/level/currentlvl").Attributes[0].InnerText) > secondlvl)
                {
                    third = second;
                    thirdlvl = secondlvl;
                    second = $"{user}";
                    secondlvl = int.Parse(root.SelectSingleNode($"{user}/level/currentlvl").Attributes[0].InnerText);
                }
                else if (int.Parse(root.SelectSingleNode($"{user}/level/currentlvl").Attributes[0].InnerText) > thirdlvl)
                {
                    third = $"{user}";
                    thirdlvl = int.Parse(root.SelectSingleNode($"{user}/level/currentlvl").Attributes[0].InnerText);
                }
            }
            string[] lb = { first, firstlvl.ToString(), second, secondlvl.ToString(), third, thirdlvl.ToString() };
            return lb;
        }

        public static string Daily(ICommandContext context)
        {
            string path = "./file/ranks/ranking.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNode node = doc.SelectSingleNode("root/users/ID" + context.User.Id.ToString() + "/level");
            if (node.SelectSingleNode("lastdaily").Attributes[0].InnerText == (DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day))
            {
                string timeleft = $"{23 - DateTime.Now.Hour}:{60 - DateTime.Now.Minute}:{60 - DateTime.Now.Second}";
                return $"You already used daily today now you need to wait {TimeSpan.Parse(timeleft)} for you next daily";
            }
            else
            {
                string curcredits = node.SelectSingleNode("credits").Attributes[0].InnerText;
                string newcredits = (int.Parse(curcredits) + 100).ToString();
                node.SelectSingleNode("credits").Attributes[0].InnerText = newcredits;
                node.SelectSingleNode("lastdaily").Attributes[0].InnerText = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                doc.Save(path);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"{DateTime.Now,-19} [{context.User.Username}] new bal is {newcredits}");
                return $"You have gained daily �**100** you current bal is �**{newcredits}**";
            }
        }

        public async static void MessageRecieved(SocketMessage arg, Random rand)
        {
            bool rankexists = false;
            string path = "./file/ranks/ranking.xml";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode node = doc.SelectSingleNode("root/users/ID" + arg.Author.Id.ToString() + "/level");
                rankexists = true;
                int Xpneeded = int.Parse(node.SelectSingleNode("needxp").Attributes[0].InnerText);
                int newcurrentxp = int.Parse(node.SelectSingleNode("currentxp").Attributes[0].InnerText) + rand.Next(1, 5);
                if (newcurrentxp >= Xpneeded)
                {
                    Ranking.LevelUp(doc, rand, arg.Author, path, Xpneeded, newcurrentxp);
                    CommandUsed.TotalLvlAdd();
                    await arg.Channel.SendMessageAsync(arg.Author.Mention + $", you reached level: {node.SelectSingleNode("currentlvl").Attributes[0].InnerText}");
                }
                else
                {
                    node.SelectSingleNode("currentxp").Attributes[0].InnerText = newcurrentxp.ToString();
                    doc.Save(path);
                }
                Ranking.CheckUser(path, arg.Author.Id.ToString(), levellist, mineinv, baginv, craftlist);
            }
            catch (Exception ex)
            {
                if (rankexists == false)
                {
                    Console.WriteLine(Ranking.AddNewUserRank(path, arg.Author.Id.ToString()));
                    CommandUsed.TotalLvlAdd();
                    File.AppendAllText("./file/ranks/LevelLog.txt", $"{DateTime.Now,-19} [{arg.Author.Username}] reached Level 1\r\n");
                }
                else
                    Console.WriteLine(ex.Message.ToString() + " " + ex.InnerException);
            }
        }

        public static void ResetDaily()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("file/ranks/ranking.xml");

            foreach (XmlNode node in doc.SelectSingleNode("root/users"))
            {
                try
                {
                    node.SelectSingleNode("level/lastdaily").Attributes[0].Value = "";
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            doc.Save("file/ranks/ranking.xml");
        }
    }
}
