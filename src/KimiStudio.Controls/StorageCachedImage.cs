using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Windows.Media.Imaging;

namespace KimiStudio.Controls
{
    /// <summary>
    /// 独立存储缓存的图片源
    /// </summary>
    public sealed class StorageCachedImage : BitmapSource
    {
        private readonly Uri uriSource;
        private readonly string filePath;
        private readonly IsolatedStorageFile isolatedStorageFile;
        private const string CacheDirectory = "cachedImages";
        
        /// <summary>
        /// 创建一个独立存储缓存的图片源
        /// </summary>
        /// <param name="uriSource"></param>
        public StorageCachedImage(Uri uriSource)
        {
            this.uriSource = uriSource;

            //创建缓存目录
            isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
            if (!isolatedStorageFile.DirectoryExists(CacheDirectory))
            {
                isolatedStorageFile.CreateDirectory(CacheDirectory);
            }
            //文件路径
            filePath = Path.Combine(CacheDirectory, uriSource.AbsolutePath.TrimStart('/').Replace('/', '_'));
            OpenCatchSource();
        }

        //private void OnImageOpenCompleted(ImageOpenCompleteEventArgs eventArgs)
        //{
        //    var handler = ImageOpenCompleted;
        //    if (handler == null) return;

        //    Thread.Sleep(10000);
        //    handler.Invoke(this, eventArgs);
        //}

        /// <summary>
        /// 打开缓存源
        /// </summary>
        private void OpenCatchSource()
        {
            bool exist = isolatedStorageFile.FileExists(filePath);
            if (exist)
            {
                SetCacheStreamSource();
            }
            else
            {
                SetWebStreamSource();
            }
        }

        /// <summary>
        /// 设置缓存流到图片
        /// </summary>
        private void SetCacheStreamSource()
        {
            using (var stream = isolatedStorageFile.OpenFile(filePath, FileMode.Open, FileAccess.Read))
            {
                SetSource(stream);
            }
        }

        /// <summary>
        /// 下载Uri中的图片
        /// </summary>
        private void SetWebStreamSource()
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(uriSource);
            httpWebRequest.AllowReadStreamBuffering = true;
            httpWebRequest.BeginGetResponse(ResponseCallBack, httpWebRequest);
        }


        /// <summary>
        /// 下载回调
        /// </summary>
        /// <param name="asyncResult"></param>
        private void ResponseCallBack(IAsyncResult asyncResult)
        {
            var httpWebRequest = asyncResult.AsyncState as HttpWebRequest;
            if(httpWebRequest == null)return;
            try
            {
                var response = httpWebRequest.EndGetResponse(asyncResult);
                using(var stream = response.GetResponseStream())
                using (var fileStream = isolatedStorageFile.OpenFile
                    (filePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    stream.CopyTo(fileStream);
                }
                Dispatcher.BeginInvoke(SetCacheStreamSource);
            }
            catch
            {
            }
        }
    }

   
}
