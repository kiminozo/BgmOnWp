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
            Uri smallImage, mediumImage;
            if (!WriteImage(subject, true, out smallImage)) return;
            if (!WriteImage(subject, false, out mediumImage)) return;

            var uri = PageLink(subject);

            //如果存在
            ShellTile oldTile = ShellTile.ActiveTiles.FirstOrDefault
                (e => e.NavigationUri == uri);

            //生成Tile
            var newTile = new FlipTileData
            {
                Title = subject.CnName,
                BackgroundImage = mediumImage,
                BackContent = subject.Name,
                BackTitle = subject.CnName,
                SmallBackgroundImage = smallImage,
            };
            try
            {
                if (oldTile != null)
                {
                    oldTile.Update(newTile);
                }
                else
                {
                    //固定到开始界面
                    ShellTile.Create(uri, newTile, false);
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        private static bool WriteImage(SubjectModel subject, bool isSmall, out Uri sharedImage)
        {
            sharedImage = null;
            if (subject.ImageSource == null) return false;


            int size = isSmall ? 159 : 336;
            string imageName = isSmall ? "subject-small-" : "subject-medium-";
            Uri imageSrc = subject.ImageSource;//isSmall ? subject.ImageSource : subject.LargeImageSource;

            var cachedImage = new StorageCachedImage(imageSrc);
            if (!cachedImage.IsLoaded) return false;

            var writeableBitmap = new WriteableBitmap(cachedImage);
            //IF PixelWidth Min
            int mSize = writeableBitmap.PixelWidth;
            writeableBitmap = writeableBitmap.Crop(0, (writeableBitmap.PixelHeight - mSize) / 2, mSize, mSize);
            writeableBitmap = writeableBitmap.Resize(size, size, WriteableBitmapExtensions.Interpolation.NearestNeighbor);
            var filename = string.Format("/Shared/ShellContent/{0}{1}.jpg", imageName, subject.Id);
            sharedImage = new Uri("isostore:" + filename, UriKind.Absolute);
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var st = new IsolatedStorageFileStream(filename, FileMode.Create, FileAccess.Write, store))
                {
                    writeableBitmap.SaveJpeg(st, size, size, 0, 100);
                }
            }
            return true;
        }
    }
}
