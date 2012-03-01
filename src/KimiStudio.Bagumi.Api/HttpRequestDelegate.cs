using System;
using System.IO;
using System.Net;
using System.Text;


namespace KimiStudio.Bagumi.Api
{
    internal sealed class HttpRequestDelegate
    {

        private IAsyncResult BeginSendPostMethod(RequestData request, AsyncCallback asyncCallback, object state)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(request.BuildUri());
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";
            var callbackDelegate = AsyncCallbackDelegate.Create(asyncCallback, webRequest);
            return webRequest.BeginGetRequestStream(RequestStreamCallBack,
                                                    new StateObj { Delegate = callbackDelegate, PostData = request.Data, State = state });

        }

        private IAsyncResult BeginSendGetMethod(RequestData request, AsyncCallback asyncCallback, object state)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(request.BuildUri());
            var callbackDelegate = AsyncCallbackDelegate.Create(asyncCallback, webRequest);
            var asyncResult = webRequest.BeginGetResponse(callbackDelegate.CreateCallback(), state);
            return callbackDelegate.CreateResult(asyncResult);
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

        public IAsyncResult BeginSend(RequestData request, AsyncCallback asyncCallback, object state)
        {
            if (request.Data == null)
            {
                return BeginSendGetMethod(request, asyncCallback, state);
            }
            else
            {
                return BeginSendPostMethod(request, asyncCallback, state);
            }
        }

        public string EndSend(IAsyncResult asyncResult)
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
}
