using System.Collections.Generic;
using KimiStudio.Bangumi.Api.Models;

namespace KimiStudio.Bangumi.Api.Commands
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
            return request;
        }

        
    }
}
