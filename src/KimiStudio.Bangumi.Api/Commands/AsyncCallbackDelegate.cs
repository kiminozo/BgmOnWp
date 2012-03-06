using System;

namespace KimiStudio.Bangumi.Api.Commands
{
    /// <summary>
    /// �첽�ص�����
    /// </summary>
    internal sealed class AsyncCallbackDelegate
    {
        private static readonly AsyncCallbackDelegate Empty = new AsyncCallbackDelegate(null, null);

        /// <summary>
        /// ���� �첽�ص�����
        /// </summary>
        /// <param name="callback">��Ҫ������첽�ص�</param>
        /// <param name="delegateObject"></param>
        public static AsyncCallbackDelegate Create<T>(AsyncCallback callback, T delegateObject) where T : class
        {
            if (callback == null) return Empty;

            //��AsyncCallbackDelegate��װAsyncCallback
            return new AsyncCallbackDelegate(callback, delegateObject);
        }

        public static T GetDelegateObject<T>(IAsyncResult asyncResult, out IAsyncResult ownedAsyncResult) where T : class
        {
            var asyncResultDelegate = asyncResult as AsyncResultDelegate;//��ԭAsyncResultDelegate

            if (asyncResultDelegate == null)
            {
                ownedAsyncResult = null;
                return null;
            }
            ownedAsyncResult = asyncResultDelegate.AsyncResult;
            return (T)asyncResultDelegate.Delegate;//���ԭʼ����
        }

        public AsyncCallback CreateCallback()
        {
            if (this == Empty) return null;
            return AsyncCallBack;
        }

        public IAsyncResult CreateResult(IAsyncResult asyncResult)
        {
            return new AsyncResultDelegate(asyncResult, Delegate);
        }

        /// <summary>
        /// ��Ҫ������첽�ص�
        /// </summary>
        private readonly AsyncCallback asyncCallback;

        /// <summary>
        /// ԭʼ���ö���
        /// </summary>
        public object Delegate { get; set; }


        private AsyncCallbackDelegate(AsyncCallback asyncCallback, object delegateObject)
        {
            this.asyncCallback = asyncCallback;
            this.Delegate = delegateObject;
        }

        /// <summary>
        /// ��װ����첽�ص�
        /// </summary>
        /// <param name="asyncResult"></param>
        private void AsyncCallBack(IAsyncResult asyncResult)
        {
            //��AsyncResultDelegate��װasyncResult
            asyncCallback(CreateResult(asyncResult));//����ԭʼ�첽�ص�
        }

        /// <summary>
        /// �첽״̬����
        /// </summary>
        private sealed class AsyncResultDelegate : IAsyncResult
        {
            /// <summary>
            /// ԭʼ�첽״̬
            /// </summary>
            private readonly IAsyncResult asyncResult;


            /// <summary>
            /// ԭʼ���ö���
            /// </summary>
            public object Delegate { get; set; }

            public AsyncResultDelegate(IAsyncResult asyncResult, object delegateObject)
            {
                this.asyncResult = asyncResult;
                Delegate = delegateObject;
            }

            #region װ��ģʽ��װ

            public object AsyncState
            {
                get { return AsyncResult.AsyncState; }
            }

            public System.Threading.WaitHandle AsyncWaitHandle
            {
                get { return AsyncResult.AsyncWaitHandle; }
            }

            public bool CompletedSynchronously
            {
                get { return AsyncResult.CompletedSynchronously; }
            }

            public bool IsCompleted
            {
                get { return AsyncResult.IsCompleted; }
            }

            public IAsyncResult AsyncResult
            {
                get { return asyncResult; }
            }
            #endregion
        }
    }
}