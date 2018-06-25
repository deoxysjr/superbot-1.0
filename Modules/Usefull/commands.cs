﻿using System.Threading.Tasks;
using Discord.Commands;
using SuperBot_2._0.Services;
using System.Collections.Generic;
using StrawPollNET.Enums;
using RestSharp;
using System;
using System.Diagnostics;

namespace SuperBot_2._0.Modules.Usefull
{
    public class Commands : ModuleBase
    {
        [Command("test"), RequireOwner]
        public async Task Test()
        {
            Process.Start(@"D:\Super Bot\SuperBot_2.0\SuperBot_2.0\UserEditor\bin\Debug\UserEditor.exe");
            //doc.Load("./file/serversetings.xml");
            //XmlNodeList guild = doc.GetElementsByTagName("guild" + Context.Guild.Id.ToString());
            //await ReplyAsync(guild[0].Attributes["on"].InnerText);
            //IUser user = Program._client.GetUser(ulong.Parse(userid));
            await ReplyAsync("done");
        }

        [Command("strawpoll")]
        public async Task Strawpoll(string title, params string[] options)
        {
            try
            {
                var poll = new Strawpoll();

                var obj = new PollRequest()
                {
                    Title = title,
                    Options = new List<string>() { options[0], options[1] },
                    Multi = false,
                    Dupcheck = DupCheck.Normal,
                    Captcha = false
                };

                var p = await poll.CreatePollAsync(obj);
                //var pp = await poll.GetPollAsync(p.Id);

                await ReplyAsync(p.Id.ToString());

            }
            catch (Exception ex)
            {
                await ReplyAsync("error: " + ex.Message.ToString());
            }
        }

        public IRestResponse response(string url)
        {
            var client = new RestClient()
            {
                BaseUrl = new Uri(url)
            };
            var request = new RestRequest()
            {
                Method = Method.GET
            };
            return client.Execute(request);
        }

        //string[] Scopes = { DriveService.Scope.DriveReadonly };
        //string ApplicationName = "Drive API .NET Quickstart";

        //UserCredential credential;

        //using (var stream =
        //    new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
        //{
        //    string credPath = System.Environment.GetFolderPath(
        //        System.Environment.SpecialFolder.Personal);
        //    credPath = Path.Combine(credPath, ".credentials/drive-dotnet-quickstart.json");

        //    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
        //        GoogleClientSecrets.Load(stream).Secrets,
        //        Scopes,
        //        "user",
        //        CancellationToken.None,
        //        new FileDataStore(credPath, true)).Result;
        //    Console.WriteLine("Credential file saved to: " + credPath);
        //}

        //// Create Drive API service.
        //var service = new DriveService(new BaseClientService.Initializer()
        //{
        //    HttpClientInitializer = credential,
        //    ApplicationName = ApplicationName,
        //});

        //// Define parameters of request.
        //FilesResource.ListRequest listRequest = service.Files.List();
        //listRequest.PageSize = 10;
        //listRequest.Fields = "nextPageToken, files(id, name)";

        //// List files.
        //IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
        //    .Files;
        //Console.WriteLine("Files:");
        //if (files != null && files.Count > 0)
        //{
        //    foreach (var file in files)
        //    {
        //        Console.WriteLine("{0} ({1})", file.Name, file.Id);
        //    }
        //}
        //else
        //{
        //    Console.WriteLine("No files found.");
        //}

        //string key = "trnsl.1.1.20180304T105743Z.5a708ae4441191e9.692311a4087294698c179e05d0588c507654861d";
        //string UrlDetectSrsLanguage = $@"https://translate.yandex.net/api/v1.5/tr.json/detect?key={key}&text={text}";
        //string Urltranslate = $@"https://translate.yandex.net/api/v1.5/tr.json/translate?key={key}&text={text}&lang=en";
        //var Response = response(Urltranslate);
        //var json = JsonConvert.DeserializeObject<IDictionary>(Response.Content);
        //IUser user = Program._client.GetUser(Context.User.Id);
        //await ReplyAsync(json["text"].ToString().Replace("[", "").Replace("]", "").Replace("\"", ""));
    }
}
