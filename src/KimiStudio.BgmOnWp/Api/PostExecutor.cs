using System;
using System.IO;
using System.Net;
using System.Text;

namespace KimiStudio.BgmOnWp.Api
{
    public interface IExecutor
    {
        void Execute(RequestData uri);
    }

    public sealed class PostExecutor : IExecutor
    {
        private readonly Action<string> resultAction;
        private string postData;

        public PostExecutor(Action<string> resultAction)
        {
            this.resultAction = resultAction;
        }

        public void Execute(RequestData uri)
        {
            postData = uri.Data;
            var webRequest = (HttpWebRequest)WebRequest.Create(uri.BuildUri());
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";

            webRequest.BeginGetRequestStream(GetRequestStreamCallback, webRequest);
        }

        private void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            var webRequest = (HttpWebRequest)asynchronousResult.AsyncState;
            Stream postStream = webRequest.EndGetRequestStream(asynchronousResult);


            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Add the post data to the web request
            postStream.Write(byteArray, 0, byteArray.Length);
            postStream.Close();

            // Start the web request
            webRequest.BeginGetResponse(GetResponseCallback, webRequest);
        }

        private void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                var webRequest = (HttpWebRequest)asynchronousResult.AsyncState;

                string result;
                using (var response = (HttpWebResponse)webRequest.EndGetResponse(asynchronousResult))
                using (var streamResponse = response.GetResponseStream())
                using (var streamReader = new StreamReader(streamResponse))
                {
                    result = streamReader.ReadToEnd();
                }
                CallBackPost(result);
            }
            catch (WebException e)
            {
                // Error treatment
            }
        }

        private void CallBackPost(string result)
        {
            resultAction(result);
        }
    }

    public sealed class GetExecutor : IExecutor
    {
        private readonly Action<string> resultAction;

        public GetExecutor(Action<string> resultAction)
        {
            this.resultAction = resultAction;
        }

        public void Execute(RequestData uri)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(uri.BuildUri());
            webRequest.BeginGetResponse(GetResponseCallback, webRequest);
        }

        private void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                var webRequest = (HttpWebRequest)asynchronousResult.AsyncState;

                string result;
                using (var response = (HttpWebResponse)webRequest.EndGetResponse(asynchronousResult))
                using (var streamResponse = response.GetResponseStream())
                using (var streamReader = new StreamReader(streamResponse))
                {
                    result = streamReader.ReadToEnd();
                }
                CallBackPost(result);
            }
            catch (WebException e)
            {
                // Error treatment
            }
        }

        private void CallBackPost(string result)
        {
            resultAction(result);
        }
    }
}