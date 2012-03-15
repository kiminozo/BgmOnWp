using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using KimiStudio.BgmOnWp.Models;
using KimiStudio.BgmOnWp.ViewModels;
using KimiStudio.Controls;
using Microsoft.Phone.Shell;

namespace KimiStudio.BgmOnWp.Storages
{
    public static class TileHelper
    {
        private static Uri PageLink(SubjectModel subject)
        {
            return new Uri(string.Format("/Views/SubjectView.xaml?Id={0}&DisplayName={1}&FromPin=true", subject.Id, subject.Name), UriKind.Relative);
        }

        public static bool IsPined(this SubjectModel subject)
        {
            var uri = PageLink(subject);
            //如果存在则删除，并在下面重新Pin到桌面
            return ShellTile.ActiveTiles.Any(e => e.NavigationUri == uri);
        }

        public static void PinTile(this SubjectModel subject)
        {
            if(subject.ImageSource == null)return;
            var cachedImage = new StorageCachedImage(subject.ImageSource);
            if(!cachedImage.IsLoaded)return;

            var writeableBitmap = new WriteableBitmap(cachedImage);
            //IF PixelWidth Min
            int mSize = writeableBitmap.PixelWidth;
            writeableBitmap = writeableBitmap.Crop(0, (writeableBitmap.PixelHeight - mSize) / 2, mSize, mSize);
            writeableBitmap = writeableBitmap.Resize(173, 173, WriteableBitmapExtensions.Interpolation.NearestNeighbor);
            var filename = string.Format("/Shared/ShellContent/subject-{0}.jpg", subject.Id);
            var image = new Uri("isostore:" + filename, UriKind.Absolute);
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var st = new IsolatedStorageFileStream(filename, FileMode.Create, FileAccess.Write, store))
                {
                    writeableBitmap.SaveJpeg(st, 173, 173, 0, 100);
                }
            }

            var uri = PageLink(subject);

            //如果存在
            ShellTile oldTile = ShellTile.ActiveTiles.FirstOrDefault
                (e => e.NavigationUri == uri);

            //生成Tile
            var newTile = new StandardTileData
            {
                BackgroundImage = image,
                BackContent = subject.Name,
                BackTitle = subject.CnName
            };
            if (oldTile != null)
            {
                oldTile.Update(newTile);
            }
            else
            {
                //固定到开始界面
                ShellTile.Create(uri, newTile);    
            }
            
        }
    }
}
