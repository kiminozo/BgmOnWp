using System;

namespace KimiStudio.BgmOnWp.Models
{
    public class SoftStaffModel
    {
        public string Name { get; set; }

        public string AtName { get; set; }

        public string Job { get; set; }

        public Uri Image { get; set; }

        public Uri Url { get; set; }

        public static SoftStaffModel Kiminozo;
        public static SoftStaffModel Sai;

        static SoftStaffModel()
        {
            Kiminozo = new SoftStaffModel
                           {
                               Name = "kiminozo",
                               AtName = "@kiminozo",
                               Job = "程序开发/界面设计",
                               Url = new Uri(@"http://bgm.tv/user/kiminozo"),
                               Image = new Uri(@"http://lain.bgm.tv/pic/user/l/000/01/07/10747.jpg")
                           };
            Sai = new SoftStaffModel
                      {
                          Name = "Sai",
                          AtName = "@sai",
                          Job = "站长/协议接口",
                          Url = new Uri(@"http://bgm.tv/user/sai"),
                          Image = new Uri(@"http://lain.bgm.tv/pic/user/l/000/00/00/1.jpg")
                      };
        }
    }
}