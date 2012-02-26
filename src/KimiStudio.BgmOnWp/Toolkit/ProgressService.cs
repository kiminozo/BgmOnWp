using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace KimiStudio.BgmOnWp.Toolkit
{
    public class ProgressService : IProgressService
    {
        private readonly ProgressIndicator progressIndicator;

        public ProgressService(PhoneApplicationFrame rootFrame)
        {
            progressIndicator = new ProgressIndicator();

            rootFrame.Navigated += RootFrameOnNavigated;
        }

        

        private void RootFrameOnNavigated(object sender, NavigationEventArgs args)
        {
            var page = args.Content as PhoneApplicationPage;
            if (page == null)return;

            page.SetValue(SystemTray.ProgressIndicatorProperty, progressIndicator);
        }


        #region Implementation of IProgressService

        public void Show()
        {
            Show(null);
        }

        public void Show(string text)
        {
            if(progressIndicator.Dispatcher.CheckAccess())
            {
                OnShow(text);
            }
            else
            {
                progressIndicator.Dispatcher.BeginInvoke(() => OnShow(text));
            }
          
        }

        private void OnShow(string text)
        {
            progressIndicator.Text = text;
            progressIndicator.IsIndeterminate = true;
            progressIndicator.IsVisible = true;
        }

        public void Hide()
        {
            if (progressIndicator.Dispatcher.CheckAccess())
            {
                OnHide();
            }
            else
            {
                progressIndicator.Dispatcher.BeginInvoke(OnHide);
            }
        }

        private void OnHide()
        {
            progressIndicator.IsIndeterminate = false;
            progressIndicator.IsVisible = false;
        }

        #endregion


    }
}