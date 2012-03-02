﻿using System;
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
            request.AddParameter("sysuid", auth.Id);
            request.AddParameter("source",ApiKeyNames.Source);
            request.AddParameter("sysusername", auth.UserName);
            request.AddParameter("sysbuild", ApiKeyNames.Sysbuild);
            request.AddParameter("auth", auth.AuthEncode);
            return request;
        }

        
    }
}
