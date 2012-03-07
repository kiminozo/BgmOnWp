using System;
using System.IO.IsolatedStorage;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using KimiStudio.Bangumi.Api.Models;
using Newtonsoft.Json;

namespace KimiStudio.BgmOnWp.Storages
{
    public static class AuthStorage
    {
        private const string AuthKey = "authKey";
        private const string UserNameKey = "usernameKey";
        private const string PasswordKey = "usernameKey";

        private static AuthUser _authUser;

        public static AuthUser Auth
        {
            get
            {
                if (_authUser == null)
                {
                    ReadAuth();
                }
                return _authUser;

            }
            set
            {
                _authUser = value;
                string authString = JsonConvert.SerializeObject(value);
                IsolatedStorageSettings.ApplicationSettings[AuthKey] = authString;
            }
        }

        public static bool Authed { get; set; }



        [MethodImpl(MethodImplOptions.Synchronized)]
        private static void ReadAuth()
        {
            string authString;
            if (IsolatedStorageSettings.ApplicationSettings.TryGetValue(AuthKey, out authString))
            {
                _authUser = JsonConvert.DeserializeObject<AuthUser>(authString);
            }
        }

        private static string _userName;
        private static string _password;

        public static string UserName
        {
            get
            {
                if (IsolatedStorageSettings.ApplicationSettings.TryGetValue(UserNameKey, out _userName))
                {
                    return _userName;
                }
                return null;
            }
            set { IsolatedStorageSettings.ApplicationSettings[UserNameKey] = value; }
        }

        public static string Password
        {
            get
            {
                return IsolatedStorageSettings.ApplicationSettings.TryGetValue(PasswordKey, out _password) ? _password : null;
            }
            set { IsolatedStorageSettings.ApplicationSettings[PasswordKey] = value; }
        }

    }
}
