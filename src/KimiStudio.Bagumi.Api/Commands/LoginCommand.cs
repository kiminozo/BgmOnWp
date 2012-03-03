using System;
using System.Globalization;
using System.Net;
using KimiStudio.Bagumi.Api.Models;

namespace KimiStudio.Bagumi.Api.Commands
{
    public sealed class LoginCommand : Command<AuthUser>
    {
        private readonly string userName;
        private readonly string password;
        private const string AuthUrl = @"http://api.bgm.tv/auth";

        public LoginCommand(string userName, string password)
        {
            this.userName = userName;
            this.password = password;
        }

        //private const string PostFormat =
        //    @"auth=0&password={1}&sysbuild=201107272200&username={0}&sysuid=0&sysusername=0&source=onAir";
        
        #region Overrides of Command<AuthUser>

        protected override RequestData CreateRequestData()
        {
            var request = new RequestData(AuthUrl);//, string.Format(PostFormat, c, password));
            //var rnd = new Random();
            request.AddQueryString("source", ApiKeyNames.Source);

            request.AddBody("auth","0");
            request.AddBody("password", password);
            request.AddBody("sysbuild", ApiKeyNames.Sysbuild);
            request.AddBody("username", userName);
            request.AddBody("sysuid", "0");
            request.AddBody("sysusername", "0");
            request.AddBody("source", ApiKeyNames.Source);
           // request.AddParameter("rand", rnd.NextDouble().ToString(CultureInfo.InvariantCulture));
            return request;
        }

        #endregion
    }


}
