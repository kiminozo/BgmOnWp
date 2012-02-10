using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace KimiStudio_BgmOnWp
{
    public partial class App : Application
    {
        private static MainViewModel viewModel = null;

        /// <summary>
        /// 视图在绑定时使用的静态 ViewModel。
        /// </summary>
        /// <returns>MainViewModel 对象。</returns>
        public static MainViewModel ViewModel
        {
            get
            {
                // 将视图模型的创建延迟到必需时才进行
                if (viewModel == null)
                    viewModel = new MainViewModel();

                return viewModel;
            }
        }

        /// <summary>
        /// 提供对 Phone 应用程序的根框架的轻松访问。
        /// </summary>
        /// <returns>Phone 应用程序的根框架。</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Application 对象的构造函数。
        /// </summary>
        public App()
        {
            // 针对未捕获的异常的全局处理程序。
            // 请注意 ApplicationBarItem 引发了异常。在此将不会捕获 Click。
            UnhandledException += Application_UnhandledException;

            // 在调试时显示图形分析信息。
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // 显示当前帧率计数器。
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // 显示在每帧中重绘的应用程序区域。
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // 启用非生产性分析可视化模式，
                // 此模式显示页面上正由 GPU 加速且具有彩色叠加的区域。
                //Application.Current.Host.Settings.EnableCacheVisualization = true;
            }

            // 标准 Silverlight 初始化
            InitializeComponent();

            // 特定于 Phone 的初始化
            InitializePhoneApplication();
        }

        // 在启动(例如从“开始”启动)应用程序时要执行的代码
        // 在重新激活应用程序时将不执行此代码
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        }

        // 在激活(推向前台)应用程序时要执行的代码
        // 在初次启动应用程序时将不执行此代码
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        // 在取消激活(发送到后台)应用程序时要执行的代码
        // 在关闭应用程序时将不执行此代码
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // 在关闭(例如用户单击“后退”)应用程序时要执行的代码
        // 在取消激活应用程序时将不执行此代码
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // 在导航失败时要执行的代码
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // 导航已失败; 进入调试程序
                System.Diagnostics.Debugger.Break();
            }
        }

        // 在未处理的异常上执行的代码
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // 已发生未处理的异常; 进入调试程序
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone 应用程序初始化

        // 避免双重初始化
        private bool phoneApplicationInitialized = false;

        // 不向此方法添加任何附加代码
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // 创建框架但不将其设置为 RootVisual; 这允许初始
            // 屏幕保持活动状态，直至应用程序准备呈现为止。
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // 处理失败的导航
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // 确保我们未重新初始化
            phoneApplicationInitialized = true;
        }

        // 不向此方法添加任何附加代码
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // 设置根 Visual 以允许应用程序呈现
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // 删除此处理程序，因为不再需要它
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}