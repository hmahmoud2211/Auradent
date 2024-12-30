using System;
using System.Windows;
using System.Windows.Threading;

namespace Auradent.Core
{
    public class AutoRefreshWindow : Window
    {
        protected DispatcherTimer _refreshTimer;

        protected virtual void InitializeAutoRefresh(double intervalSeconds = 1)
        {
            _refreshTimer = new DispatcherTimer();
            _refreshTimer.Interval = TimeSpan.FromSeconds(intervalSeconds);
            _refreshTimer.Tick += (s, e) => RefreshData();
            _refreshTimer.Start();
        }

        protected virtual void RefreshData()
        {
            // Override this method in derived classes to implement specific refresh logic
        }

        protected override void OnClosed(EventArgs e)
        {
            if (_refreshTimer != null)
            {
                _refreshTimer.Stop();
            }
            base.OnClosed(e);
        }
    }
}