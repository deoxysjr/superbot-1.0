using System;
using System.Threading.Tasks;
using Discord.Commands;
using SuperBot_1_0.Services;
using System.Collections.Generic;
using StrawPollNET.Enums;
using SuperBotDLL1_0.RankingSystem;
using Discord;

namespace SuperBot_1_0.Modules
{
    public class commands : ModuleBase
    {
        [Command("test"), RequireOwner]
        public async Task Test()
        {
            IUser user = Program._client.GetUser(Context.User.Id);
            await ReplyAsync("Hi " + user.Mention);
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
                    Options = new List<string>() { options[0], options[1], options[2] },
                    Dupcheck = DupCheck.Disabled

                };

                var p = await poll.CreatePollAsync(obj);
                var pp = await poll.GetPollAsync(1);

                await ReplyAsync(p.PollUrl);

            }
            catch (Exception ex)
            {
                await ReplyAsync("error: " + ex.Message.ToString());
            }
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
    }
}
