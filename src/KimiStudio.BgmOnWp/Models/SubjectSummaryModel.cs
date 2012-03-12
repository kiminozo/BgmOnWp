using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using KimiStudio.Bangumi.Api.Models;
using KimiStudio.Controls;
using Microsoft.Expression.Interactivity.Core;
using Microsoft.Phone.Shell;

namespace KimiStudio.BgmOnWp.Models
{
    public class SubjectSummaryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CnName { get; set; }
        public string Doing { get; set; }
        public Uri Image { get; set; }
        public Uri SmallImage { get; set; }
        public Uri LargeImage { get; set; }
        public Uri RemoteUrl { get; set; }

        public static SubjectSummaryModel FromBagumiData(SubjectSummary subject)
        {
            return new SubjectSummaryModel
            {
                Id = subject.Id,
                Name = subject.Name,
                CnName = subject.NameCn,
                Doing = subject.Collection != null ? string.Format("{0}���ڿ�", subject.Collection.Doing) : null,
                Image = subject.Images != null ? subject.Images.Common : null,
                SmallImage = subject.Images != null ? subject.Images.Small : null,
                LargeImage = subject.Images != null ? subject.Images.Large : null,
                RemoteUrl = subject.Url,
            };
        }

        //public SubjectSummaryModel()
        //{
        //    Pin = new ActionCommand(OnPin);
        //}



        //public ICommand Pin { get; private set; }

        //public void OnPin()
        //{
        //    var bmp = new WriteableBitmap(173, 173);
        //    var logo = new BitmapImage(LargeImage);
        //    var img = new Image { Source = logo };

        //    // Force the bitmapimage to load it's properties so the transform will work
        //  //  logo.CreateOptions = BitmapCreateOptions.None;
        //   // var bl = new TextBlock();
        ////    bl.Foreground = new SolidColorBrush(Colors.White);
        ////    bl.FontSize = 24.0;
        ////    bl.Text = "any text we want!";

        // //   bmp.Render(bl, null);

        //    var tt = new TranslateTransform();
        //    tt.X = 173 - logo.PixelWidth;
        //    tt.Y = 173 - logo.PixelHeight;

        //    bmp.Render(img, tt);

        //    bmp.Invalidate();
        //    var filename = string.Format("/Shared/ShellContent/subject-{0}.jpg", Id);
        //    var image = new Uri("isostore:" + filename, UriKind.Absolute);
                
        //    using (var store = IsolatedStorageFile.GetUserStoreForApplication())
        //    {
        //        using (var st = new IsolatedStorageFileStream(filename, FileMode.Create, FileAccess.Write, store))
        //        {
        //            bmp.SaveJpeg(st, 173, 173, 0, 100);
        //        }
        //    }
        //    var uri = new Uri(string.Format("/Views/SubjectView.xaml?Id={0}", Id), UriKind.Relative);
        //    //���������ɾ����������������Pin������
        //    ShellTile oldTile = ShellTile.ActiveTiles.FirstOrDefault
        //        (e => e.NavigationUri == uri);
        //    if (oldTile != null)
        //    {
        //        oldTile.Delete();
        //    }

        //    //����Tile
        //    var myTile = new StandardTileData
        //                     {
        //                         BackgroundImage = image,
        //                         Title = this.Name,
        //                         Count = 0,
        //                     };
        //    //�̶�����ʼ����
        //    ShellTile.Create(uri, myTile);
        //}

    }
}