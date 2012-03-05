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

    public interface ICommand<TResult> where TResult : class
    {
        void BeginExecute(AsyncCallback asyncCallback, object state);
        TResult EndExecute(IAsyncResult asyncResult);
    }

    public abstract class Command<TResult> : Command, ICommand<TResult>
        where TResult : class
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