using KimiStudio.Controls;

namespace KimiStudio.BgmOnWp.Toolkit
{
    public class LoadingService : ILoadingService
    {
        private LoadingOverlay loadingOverlay;

        #region Implementation of ILoadingService

        public void Show()
        {
            Show(null);
        }

        public void Show(string message)
        {
            if (loadingOverlay != null)
            {
                loadingOverlay.Hide();
            }
            loadingOverlay = new LoadingOverlay {Title = message};
            loadingOverlay.Show();
        }

        public void Hide()
        {
            if(loadingOverlay == null)return;
            loadingOverlay.Hide();
        }

        #endregion
    }
}