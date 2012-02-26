using System;
using System.IO;
using System.Net;
using System.Text;

namespace KimiStudio.BgmOnWp.Api
{
    public abstract class Executor
    {
        public abstract void ExecuteAsync(RequestData request);
        public event ExecuteCompletedHandler ExecuteCompleted;

        private void OnExecuteCompleted(ExecuteCompletedEventArgs e)
        {
            var handler = ExecuteCompleted;
            if (handler != null) handler(e);
        }


        protected void OnCompleted(string result)
        {
            OnExecuteCompleted(new ExecuteCompletedEventArgs(result,false,null));
        }

        protected void OnError(Exception error)
        {
            OnExecuteCompleted(new ExecuteCompletedEventArgs(null, true, error));
        }

    }

    public delegate void ExecuteCompletedHandler(ExecuteCompletedEventArgs e);

    public class ExecuteCompletedEventArgs:EventArgs
    {
        public ExecuteCompletedEventArgs(string result, bool cancelled, Exception error)
        {
            Cancelled = cancelled;
            Result = result;
            Error = error;
        }

        public string Result { get; private set; }
        public bool Cancelled { get; private set; }
        public Exception Error { get; private set; }
    }

    public sealed class HttpPostExecutor : Executor
    {
        private string postData;
        
        public override void ExecuteAsync(RequestData request)
        {
            postData = request.Data;
            var webRequest = (HttpWebRequest) WebRequest.Create(request.BuildUri());
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";

            webRequest.BeginGetRequestStream(GetRequestStreamCallback, webRequest);
        }

        private void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            var webRequest = (HttpWebRequest) asynchronousResult.AsyncState;
            using (var postStream = webRequest.EndGetRequestStream(asynchronousResult))
            using (var streamWriter = new StreamWriter(postStream, Encoding.UTF8))
            {
                streamWriter.Write(postData);
            }
            webRequest.BeginGetResponse(GetResponseCallback, webRequest);
        }

        private void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            string result = null;
            try
            {
                var webRequest = (HttpWebRequest) asynchronousResult.AsyncState;

                using (var response = (HttpWebResponse) webRequest.EndGetResponse(asynchronousResult))
                using (var streamResponse = response.GetResponseStream())
                using (var streamReader = new StreamReader(streamResponse))
                {
                    result = streamReader.ReadToEnd();
                }
                
            }
            catch (Exception err)
            {
                OnError(err);
                return;
            }
            OnCompleted(result);
        }

    }

    public sealed class HttpGetExecutor : Executor
    {
        public override void ExecuteAsync(RequestData request)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(request.BuildUri());
            webRequest.BeginGetResponse(GetResponseCallback, webRequest);
        }

        private void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            string result = null;
            try
            {
                var webRequest = (HttpWebRequest)asynchronousResult.AsyncState;
                using (var response = (HttpWebResponse)webRequest.EndGetResponse(asynchronousResult))
                using (var streamResponse = response.GetResponseStream())
                using (var streamReader = new StreamReader(streamResponse))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            catch (Exception err)
            {
                OnError(err);
                return;
            }
            OnCompleted(result);
        }
    }
}