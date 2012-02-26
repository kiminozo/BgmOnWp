using System;
using System.Collections;
using KimiStudio.BgmOnWp.ModelMessages;
using KimiStudio.BgmOnWp.Models;
using Newtonsoft.Json;

namespace KimiStudio.BgmOnWp.Api
{
    public sealed class LoginCommand : ApiCommand
    {
        private readonly string userName;
        private readonly string password;
        private readonly Action<LoginMessage> publisher;
        private const string AuthUrl = @"http://api.bgm.tv/auth?source=onAir";

        public LoginCommand(string userName, string password, Action<LoginMessage> publisher)
        {
            this.userName = userName;
            this.password = password;
            this.publisher = publisher;
        }

        private const string PostFormat =
            @"auth=0&password={1}&sysbuild=201107272200&username={0}&sysuid=0&sysusername=0&source=onAir";

        public override void Execute()
        {
            var executor = new HttpPostExecutor();
            executor.ExecuteCompleted += args => publisher(ToMessage(args));
            executor.ExecuteAsync(new RequestData(AuthUrl, string.Format(PostFormat, userName, password)));
        }

        private LoginMessage ToMessage(ExecuteCompletedEventArgs args)
        {
            if (args.Cancelled) return new LoginMessage { Cancelled = true, ErrorMessage = args.Error.ToString() };
            return new LoginMessage
                       {
                           AuthUser = JsonConvert.DeserializeObject<AuthUser>(args.Result),
                       };
        }
    }
}