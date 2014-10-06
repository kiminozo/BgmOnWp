using System;
using System.Diagnostics;
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
        private const string CacheDirectory = "CachedImages";

        public bool IsLoaded { get; private set; }
        
        static StorageCachedImage()
        {

            //创建缓存目录
            using (var isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!isolatedStorageFile.DirectoryExists(CacheDirectory))
                {
                    isolatedStorageFile.CreateDirectory(CacheDirectory);
                }
            }
        }

        /// <summary>
        /// 创建一个独立存储缓存的图片源
        /// </summary>
        /// <param name="uriSource"></param>
        public StorageCachedImage(Uri uriSource)
        {
            if (uriSource.Scheme != "http" || uriSource.Scheme != "https")
            {
                this.uriSource = new Uri("http://" + uriSource.Host + uriSource.PathAndQuery);
            }
            else
            {
                this.uriSource = uriSource;    
            }
            
            //文件路径
            filePath = Path.Combine(CacheDirectory, uriSource.AbsolutePath.TrimStart('/').Replace('/', '_'));
            OpenCatchSource();
        }

        /// <summary>
        /// 打开缓存源
        /// </summary>
        private void OpenCatchSource()
        {
            bool exist;
            using (var isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                exist = isolatedStorageFile.FileExists(filePath);
            }
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
            using (var isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                try
                {
                    using (var stream = isolatedStorageFile.OpenFile(filePath, FileMode.Open, FileAccess.Read))
                    {
                        SetSource(stream);
                    }
                }
                catch (IsolatedStorageException)
                {
                    SetSource(new MemoryStream());
                }
            }
            IsLoaded = true;
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
                using (var isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication())
                using (var fileStream = isolatedStorageFile.OpenFile
                    (filePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    stream.CopyTo(fileStream);
                }
                Dispatcher.BeginInvoke(SetCacheStreamSource);
            }
            catch(Exception err)
            {
                Debug.WriteLine(err.Message);
            }
        }
    }

   
}
