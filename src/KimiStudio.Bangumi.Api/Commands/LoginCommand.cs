﻿using KimiStudio.Bangumi.Api.Models;

namespace KimiStudio.Bangumi.Api.Commands
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
            request.AddBody("username", userName);
            request.AddBody("password", password);
           // request.AddParameter("rand", rnd.NextDouble().ToString(CultureInfo.InvariantCulture));
            return request;
        }

        protected override AuthUser ValidateResult(AuthUser result)
        {
            if (result.AuthEncode == null) throw new ApiException("登录失败，请检查用户名和密码");

            return result;
        }

        #endregion
    }


}
