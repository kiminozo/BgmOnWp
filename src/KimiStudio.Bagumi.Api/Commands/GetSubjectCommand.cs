using System.Collections.Generic;
using System.Globalization;
using KimiStudio.Bagumi.Api.Models;

namespace KimiStudio.Bagumi.Api.Commands
{
    public sealed class GetSubjectCommand : Command<Subject>
    {
        private readonly string subjectId;
        private readonly AuthUser auth;
        private const string Uri = @"http://api.bgm.tv/subject/";

        public GetSubjectCommand(int subjectId, AuthUser auth)
        {
            this.auth = auth;
            this.subjectId = subjectId.ToString(CultureInfo.InvariantCulture);
        }

        protected override RequestData CreateRequestData()
        {
            var request = new RequestData(Uri + subjectId);
            request.AddParameter("responseGroup", "large");
            request.AddParameter("source", "OnAir");
            request.AddParameter("sysbuild", "201107272200");
            request.AddParameter("sysuid", auth.Id);
            request.AddParameter("sysusername", auth.UserName);
            request.AddParameter("auth", auth.Auth);
            return request;
        }
    }
}