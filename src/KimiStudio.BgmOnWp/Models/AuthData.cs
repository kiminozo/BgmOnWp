using System;

namespace KimiStudio.BgmOnWp.Models
{
    public class AuthData
    {
        public string Auth { get; set; }
        //public string AuthEncode { get; set; }
        public Avatar Avatar { get; set; }
        public string Id { get; set; }
        public string NickName { get; set; }
        public string Sign { get; set; }
        public Uri Url { get; set; }
        public string UserName { get; set; }
    }
}