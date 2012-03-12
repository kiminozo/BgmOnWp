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
using KimiStudio.Controls;
using Microsoft.Phone.Shell;

namespace KimiStudio.BgmOnWp.Storages
{
    public static class TileHelper
    {
        public static void PinTile(this SubjectSummaryModel subject)
        {
            var writeableBitmap = new WriteableBitmap(new StorageCachedImage(subject.Image));
            //IF PixelWidth Min
            int mSize = writeableBitmap.PixelWidth;
            writeableBitmap = writeableBitmap.Crop(0, (writeableBitmap.PixelHeight - mSize) / 2, mSize, mSize);
            writeableBitmap = writeableBitmap.Resize(173, 173, WriteableBitmapExtensions.Interpolation.NearestNeighbor);
            writeableBitmap.FillRectangle(0, 133, 173, 173, Color.FromArgb(0xFF, 0xF1, 0x91, 0x99));
            var filename = string.Format("/Shared/ShellContent/subject-{0}.jpg", subject.Id);
            var image = new Uri("isostore:" + filename, UriKind.Absolute);
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var st = new IsolatedStorageFileStream(filename, FileMode.Create, FileAccess.Write, store))
                {
                    writeableBitmap.SaveJpeg(st, 173, 173, 0, 100);
                }
            }

            var uri = new Uri(string.Format("/Views/SubjectView.xaml?Id={0}&DisplayName={1}&FromPin=true", subject.Id, subject.Name), UriKind.Relative);
            //如果存在则删除，并在下面重新Pin到桌面
            ShellTile oldTile = ShellTile.ActiveTiles.FirstOrDefault
                (e => e.NavigationUri == uri);
            if (oldTile != null)
            {
                oldTile.Delete();
            }

            //生成Tile
            var myTile = new StandardTileData
            {
                BackgroundImage = image,
                Title = subject.Name,
                Count = 0,
            };
            //固定到开始界面
            ShellTile.Create(uri, myTile);
        }
    }
}
