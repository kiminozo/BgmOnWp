using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Caliburn.Micro;
using KimiStudio.Bangumi.Api.Commands;
using KimiStudio.BgmOnWp.Storages;
using KimiStudio.BgmOnWp.Toolkit;

namespace KimiStudio.BgmOnWp.ViewModels
{
    public class LoginViewModel : Screen
    {
        private readonly INavigationService navigation;
        private readonly IPromptManager promptManager;
        private readonly ILoadingService loadingService;

        private string userName;
        private string password;

        public LoginViewModel(IPromptManager promptManager, ILoadingService loadingService, INavigationService navigation)
        {
            this.promptManager = promptManager;
            this.loadingService = loadingService;
            this.navigation = navigation;
#if DEBUG
            userName = "piova";
            password = "test";
#endif
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            if (AuthStorage.UserName != null)
            {
                navigation.UriFor<MainPageViewModel>().Navigate();
            }
        }

        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                NotifyOfPropertyChange(() => UserName);
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                NotifyOfPropertyChange(() => Password);
            }
        }

        public void Login()
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
            {
                promptManager.ShowToast("请输入用户名和密码");
                return;
            }

            loadingService.Show("登录中\u2026");

            var task = CommandTaskFactory.Create(new LoginCommand(UserName, Password));
            task.Result(auth =>
                            {
                                loadingService.Hide();
                                AuthStorage.Auth = auth;
                                AuthStorage.Authed = true;
                                AuthStorage.UserName = UserName;
                                AuthStorage.Password = Password;
                                navigation.UriFor<MainPageViewModel>()
                                    .WithParam(x => x.Authed, true)
                                    .Navigate();
                            });
            task.Exception(err =>
                               {
                                   loadingService.Hide();
                                   promptManager.ToastError(err);
                               });
            task.Start();
        }

       
    }
}
