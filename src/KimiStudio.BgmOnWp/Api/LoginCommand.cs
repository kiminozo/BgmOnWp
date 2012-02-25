using System;
using System.Collections;
using KimiStudio.BgmOnWp.Models;
using Newtonsoft.Json;

namespace KimiStudio.BgmOnWp.Api
{
    public sealed class LoginCommand : ApiCommand
    {
        private readonly string userName;
        private readonly string password;
        private readonly Action action;
        private const string AuthUrl = @"http://api.bgm.tv/auth?source=onAir";

        public LoginCommand(string userName, string password,Action action)
        {
            this.userName = userName;
            this.password = password;
            this.action = action;
        }

        private const string PostFormat =
            @"auth=0&password={1}&sysbuild=201107272200&username={0}&sysuid=0&sysusername=0&source=onAir";

        public override void Execute()
        {
            var executor = new PostExecutor(CallBackPost);
            executor.Execute(new RequestData(AuthUrl,string.Format(PostFormat, userName, password)));
        }

        private void CallBackPost(string result)
        {
            BagumiService.Auth = JsonConvert.DeserializeObject<AuthUser>(result);
            action();
        }
    }
}