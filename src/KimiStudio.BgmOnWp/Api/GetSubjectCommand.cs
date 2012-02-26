using System;
using System.Globalization;
using KimiStudio.BgmOnWp.ModelMessages;
using KimiStudio.BgmOnWp.Models;
using Newtonsoft.Json;

namespace KimiStudio.BgmOnWp.Api
{
    public sealed class GetSubjectCommand : ApiCommand<SubjectMessage>
    {
        private readonly string subjectId;
        private const string Uri = @"http://api.bgm.tv/subject/";

        #region Overrides of ApiCommand

        public GetSubjectCommand(int subjectId)
        {
            this.subjectId = subjectId.ToString(CultureInfo.InvariantCulture);
        }

        public override void Execute(Action<SubjectMessage> callbackHandler)
        {
            var authData = BagumiService.Auth;
            var request = new RequestData(Uri + subjectId);
            request.AddParameter("responseGroup", "large");
            request.AddParameter("source", "OnAir");
            request.AddParameter("sysbuild", "201107272200");
            request.AddParameter("sysuid", authData.Id);
            request.AddParameter("sysusername", authData.UserName);
            request.AddParameter("auth", authData.Auth);

            var executor = new HttpGetExecutor();
            executor.ExecuteCompleted += args => callbackHandler(ToMessage(args));
            executor.ExecuteAsync(request);
        }

        private SubjectMessage ToMessage(ExecuteCompletedEventArgs args)
        {
            if (args.Cancelled) return new SubjectMessage {Cancelled = true, ErrorMessage = args.Error.ToString()};
            return new SubjectMessage
                       {
                           Subject = JsonConvert.DeserializeObject<Subject>(args.Result),
                       };
        }

        #endregion
    }
}