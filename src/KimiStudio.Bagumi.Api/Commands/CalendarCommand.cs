using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KimiStudio.Bagumi.Api.Models;

namespace KimiStudio.Bagumi.Api.Commands
{
    public class CalendarCommand : Command<List<Calendar>>
    {
        private const string Uri = @"http://api.bgm.tv/calendar";
        private readonly AuthUser auth;

        public CalendarCommand(AuthUser auth)
        {
            this.auth = auth;
        }

        protected override RequestData CreateRequestData()
        {
            var request = new RequestData(Uri);
            request.AddQueryString("sysuid", auth.Id);
            request.AddQueryString("source",ApiKeyNames.Source);
            request.AddQueryString("sysusername", auth.UserName);
            request.AddQueryString("sysbuild", ApiKeyNames.Sysbuild);
            request.AddQueryString("auth", auth.AuthEncode);
            return request;
        }

        
    }
}
