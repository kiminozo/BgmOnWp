using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace KimiStudio.Bagumi.Api.Commands
{
    public abstract class Command
    {


        private void BeginSendPostMethod(RequestData request, AsyncCallback asyncCallback, object state)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(request.BuildUri());
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";

            var callbackDelegate = AsyncCallbackDelegate.Create(asyncCallback, webRequest);
            webRequest.BeginGetRequestStream(RequestStreamCallBack,
                                             new StateObj { Delegate = callbackDelegate, PostData = request.BuildBody(), State = state });

        }

        private void BeginSendGetMethod(RequestData request, AsyncCallback asyncCallback, object state)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(request.BuildUri());
            webRequest.CookieContainer = new CookieContainer();
            //webRequest.Accept =
            //    @"text/xml, application/xml, application/xhtml+xml, text/html;q=0.9, text/plain;q=0.8, text/css, image/png, image/jpeg, image/gif;q=0.8, application/x-shockwave-flash, video/mp4;q=0.9, flv-application/octet-stream;q=0.8, video/x-flv;q=0.7, audio/mp4, application/futuresplash, */*;q=0.5";
            //webRequest.KeepAlive = true;
            //webRequest.UserAgent =
            //    @"Mozilla/5.0 (Windows; U; zh-CN) AppleWebKit/533.19.4 (KHTML, like Gecko) AdobeAIR/3.1";
            ////webRequest.TransferEncoding = @"gzip,deflate";

            //webRequest.Referer = @"app:/onAir.swf";
            //webRequest.Headers["Accept-Encoding"] = @"gzip,deflate";
            //webRequest.Headers["x-flash-version"] = "11,1,102,58";

            var callbackDelegate = AsyncCallbackDelegate.Create(asyncCallback, webRequest);
            var asyncResult = webRequest.BeginGetResponse(callbackDelegate.CreateCallback(), state);
            callbackDelegate.CreateResult(asyncResult);

        }

        private class StateObj
        {
            public AsyncCallbackDelegate Delegate;
            public string PostData;
            public object State;
        }

        private static void RequestStreamCallBack(IAsyncResult asyncResult)
        {
            var stateObj = (StateObj)asyncResult.AsyncState;
            var webRequest = (HttpWebRequest)stateObj.Delegate.Delegate;
            using (var postStream = webRequest.EndGetRequestStream(asyncResult))
            using (var streamWriter = new StreamWriter(postStream, Encoding.UTF8))
            {
                streamWriter.Write(stateObj.PostData);
            }
            webRequest.BeginGetResponse(stateObj.Delegate.CreateCallback(), stateObj.State);
        }

        protected void BeginSend(RequestData request, AsyncCallback asyncCallback, object state)
        {
            if (request.IsPost)
            {
                BeginSendPostMethod(request, asyncCallback, state);
            }
            else
            {
                BeginSendGetMethod(request, asyncCallback, state);
            }
        }

        protected string EndSend(IAsyncResult asyncResult)
        {
            IAsyncResult ownedAsyncResult;
            var webRequest = AsyncCallbackDelegate.GetDelegateObject<HttpWebRequest>(asyncResult, out ownedAsyncResult);
            using (var response = (HttpWebResponse)webRequest.EndGetResponse(ownedAsyncResult))
            {
                if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.NotModified)
                    return null;

                using (var streamResponse = response.GetResponseStream())
                {
                    using (var streamReader = new StreamReader(streamResponse))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }
    }

    public abstract class Command<TResult> : Command where TResult : class
    {
        public void BeginExecute(AsyncCallback asyncCallback, object state)
        {
            var data = CreateRequestData();
            BeginSend(data, asyncCallback, state);
        }

        protected abstract RequestData CreateRequestData();

        public TResult EndExecute(IAsyncResult asyncResult)
        {
            var result = EndSend(asyncResult);
            if (string.IsNullOrEmpty(result)) return default(TResult);

            return JsonConvert.DeserializeObject<TResult>(result);
        }

    }
}