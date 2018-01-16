using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.WebSocket;
using System.Xml;
using Discord.Commands;
using Discord;
using SuperBotDLL1_0.RankingSystem;

namespace SuperBot_1_0.Services
{
    class RankUtils
    {
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
                    bool found = false;
                    foreach (XmlNode node in doc.SelectNodes("root/users/UserID"))
                    {
                        if (node.Attributes[0].InnerText == u.Id.ToString())
                        {
                            found = true;
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
                    }
                    if (found == false)
                    {
                        return $"sorry but {u.Mention} does not have a rank";
                    }
                }
                else
                {
                    return "Bots don't have ranks";
                }
                return "error!!";
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
                    bool found = false;
                    foreach (XmlNode node in doc.SelectNodes("root/users/UserID"))
                    {
                        if (node.Attributes[0].InnerText == u.Id.ToString())
                        {
                            found = true;
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
                    }
                    if (found == false)
                    {
                        return $"sorry but {u.Mention} does not have a rank";
                    }
                }
                else
                {
                    return "Bots don't have ranks";
                }
                return "error!!";
            }
        }
        
        public static string[] getLeaderBoard()
        {
            string first = ""; int firstlvl = 0;
            string second = ""; int secondlvl = 0;
            string third = ""; int thirdlvl = 0;
            XmlDocument doc = new XmlDocument();
            doc.Load("./file/ranks/ranking.xml");
            foreach (XmlNode node in doc.SelectNodes("root/users/UserID"))
            {
                if (int.Parse(node.SelectSingleNode("level/currentlvl").Attributes[0].InnerText) > firstlvl && firstlvl == 0)
                {
                    first = $"{node.Attributes[0].InnerText}";
                    firstlvl = int.Parse(node.SelectSingleNode("level/currentlvl").Attributes[0].InnerText);
                }
                else if (int.Parse(node.SelectSingleNode("level/currentlvl").Attributes[0].InnerText) > firstlvl)
                {
                    third = second;
                    thirdlvl = secondlvl;
                    second = first;
                    secondlvl = firstlvl;
                    first = $"{node.Attributes[0].InnerText}";
                    firstlvl = int.Parse(node.SelectSingleNode("level/currentlvl").Attributes[0].InnerText);
                }
                else if (int.Parse(node.SelectSingleNode("level/currentlvl").Attributes[0].InnerText) > secondlvl && secondlvl == 0)
                {
                    second = $"{node.Attributes[0].InnerText}";
                    secondlvl = int.Parse(node.SelectSingleNode("level/currentlvl").Attributes[0].InnerText);
                }
                else if (int.Parse(node.SelectSingleNode("level/currentlvl").Attributes[0].InnerText) > secondlvl)
                {
                    third = second;
                    thirdlvl = secondlvl;
                    second = $"{node.Attributes[0].InnerText}";
                    secondlvl = int.Parse(node.SelectSingleNode("level/currentlvl").Attributes[0].InnerText);
                }
                else if (int.Parse(node.SelectSingleNode("level/currentlvl").Attributes[0].InnerText) > thirdlvl)
                {
                    third = $"{node.Attributes[0].InnerText}";
                    thirdlvl = int.Parse(node.SelectSingleNode("level/currentlvl").Attributes[0].InnerText);
                }
            }
            string[] lb = {first, second, third};
            return lb;
        }

        public static string daily(ICommandContext context)
        {
            string path = "./file/ranks/ranking.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            foreach (XmlNode user in doc.SelectNodes("root/users/UserID"))
            {
                if (context.User.Id.ToString() == user.Attributes[0].InnerText)
                {
                    XmlNode node = user.SelectSingleNode("level");
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
                        return $"You have gained daily �**100** you current bal is �**{newcredits}**";
                    }
                }
            }
            return "error";
        }

        public async static void MessageRecieved(SocketMessage arg, Random rand)
        {
            try
            {
                bool rankexist = false;
                string path = "./file/ranks/ranking.xml";
                XmlDocument doc = new XmlDocument();
                doc.Load(path);

                foreach (XmlNode user in doc.SelectNodes("root/users/UserID"))
                {
                    if (user.Attributes[0].InnerText == arg.Author.Id.ToString())
                    {
                        rankexist = true;
                        XmlNode node = user.SelectSingleNode("level");
                        //Checkrank(doc, user);
                        string currentlvl = node.SelectSingleNode("currentlvl").Attributes[0].InnerText;
                        string Xpneeded = node.SelectSingleNode("needxp").Attributes[0].InnerText;
                        int xp = rand.Next(1, 5);
                        string oldcurrentxp = node.SelectSingleNode("currentxp").Attributes[0].InnerText;
                        int newcurrentxp = int.Parse(oldcurrentxp) + xp;
                        double credits = double.Parse(node.SelectSingleNode("credits").Attributes[0].InnerText);
                        int nextlvl = int.Parse(node.SelectSingleNode("nextlvl").Attributes[0].InnerText);
                        if (newcurrentxp >= int.Parse(Xpneeded))
                        {
                            credits += 20;
                            int current = newcurrentxp - int.Parse(Xpneeded);
                            int newneedxp = Newxpneed(nextlvl, rand);
                            node.SelectSingleNode("currentlvl").Attributes[0].InnerText = nextlvl.ToString();
                            node.SelectSingleNode("nextlvl").Attributes[0].InnerText = (nextlvl + 1).ToString();
                            node.SelectSingleNode("currentxp").Attributes[0].InnerText = current.ToString();
                            node.SelectSingleNode("needxp").Attributes[0].InnerText = newneedxp.ToString();
                            node.SelectSingleNode("credits").Attributes[0].InnerText = credits.ToString();
                            await arg.Channel.SendMessageAsync(arg.Author.Mention + ", you reached level: " + nextlvl);
                            doc.Save(path);
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine($"{DateTime.Now,-19} [{arg.Author.Username}] reached Level {nextlvl}");
                            Console.ResetColor();
                        }
                        else
                        {
                            node.SelectSingleNode("currentxp").Attributes[0].InnerText = newcurrentxp.ToString();
                            doc.Save(path);
                        }
                        break;
                    }
                }
                if (rankexist == false)
                    Ranking.AddNewUserRank(path, arg.Author.Id.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString() + " " + ex.InnerException);
            }
        }

        public static void ResetDaily()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("file/ranks/ranking.xml");
            foreach(XmlNode node in doc.SelectNodes("root/users/UserID"))
            {
                node.SelectSingleNode("level/lastdaily").Attributes[0].InnerText = "";
            }
            doc.Save("file/ranks/ranking.xml");
        }

        private static int Newxpneed(int nextlvl, Random rand)
        {
            XmlDocument docu = new XmlDocument();
            docu.Load(@"./file/ranks/levelxp.xml");
            try
            {
                int newlvl = int.Parse(docu.SelectSingleNode($"root/lvl{nextlvl}").InnerText);
                return newlvl;
            }
            catch (Exception)
            {
                int prev = nextlvl - 1;
                int prevlvl = int.Parse(docu.SelectSingleNode($"root/lvl{prev}").InnerText);
                int addxp = rand.Next(10, 24);
                int newlvl = prevlvl + addxp;
                XmlElement Nextlvl = docu.CreateElement($"lvl{nextlvl}");
                Nextlvl.InnerText = newlvl.ToString();
                docu.Save(@"./file/ranks/levelxp.xml");
                return newlvl;
            }
        }
    }
}
