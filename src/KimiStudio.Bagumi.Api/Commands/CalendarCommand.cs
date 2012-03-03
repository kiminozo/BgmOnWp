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

        public CalendarCommand()
        {
           
        }

        protected override RequestData CreateRequestData()
        {
            var request = new RequestData(Uri);
            request.AddQueryString("source",ApiKeyNames.Source);
            request.AddQueryString("sysbuild", ApiKeyNames.Sysbuild);
            return request;
        }

        
    }
}
