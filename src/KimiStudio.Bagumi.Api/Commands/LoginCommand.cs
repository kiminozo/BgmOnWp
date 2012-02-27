using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using KimiStudio.Bagumi.Api.Models;

namespace KimiStudio.Bagumi.Api.Commands
{
    public sealed class LoginCommand : Command<AuthUser>
    {
        private readonly string userName;
        private readonly string password;
        private const string AuthUrl = @"http://api.bgm.tv/auth?source=onAir";

        public LoginCommand(string userName, string password)
        {
            this.userName = userName;
            this.password = password;
        }

        private const string PostFormat =
            @"auth=0&password={1}&sysbuild=201107272200&username={0}&sysuid=0&sysusername=0&source=onAir";
        
        #region Overrides of Command<AuthUser>

        protected override RequestData CreateRequestData()
        {
            return new RequestData(AuthUrl, string.Format(PostFormat, userName, password));
        }

        #endregion
    }


}
