using System.Globalization;
using KimiStudio.Bangumi.Api.Models;

namespace KimiStudio.Bangumi.Api.Commands
{
    public sealed class GetSubjectCommand : Command<Subject>
    {
        private readonly string subjectId;
        private const string Uri = @"http://api.bgm.tv/subject/";

        public GetSubjectCommand(int subjectId)
        {
            this.subjectId = subjectId.ToString(CultureInfo.InvariantCulture);
        }

        protected override RequestData CreateRequestData()
        {
            var request = new RequestData(Uri + subjectId);
            request.AddQueryString("responseGroup", "large");
            request.AddQueryString("source", ApiKeyNames.Source);
            return request;
        }
    }
}