using EmbeddedBiliWebHelper;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Win32;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics;
using WinRT.Interop;
using Microsoft.Web.WebView2;
using Microsoft.Web.WebView2.Core;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BiliBiliWebPortal
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            ScalingFactor = User32Packaged.GetScalingFactor(WindowNative.GetWindowHandle(this));
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(CustomTitleBar);
            MainWebAppView.Source = BiliBiliWebUri.BiliBiliMain;
            _ = MainWebAppView.EnsureCoreWebView2Async();
            MainWebAppView.CoreWebView2Initialized += CoreWebView2Actived;
            this.Activated += OnLoaded;
            this.SizeChanged += OnSizeChanged;
        }

        private float ScalingFactor { get; set; } = 1;

        private RectInt32[] MainWindowTitleBarDragRects
        {
            get =>
            [
                new ()
                {
                    X = (int)((CustomTitleBarDragArea.ActualOffset.X + CustomTitleBarDragArea.ActualWidth) * ScalingFactor),
                    Y = 0,
                    Width = (int)((CustomTitleBar.ActualOffset.X + CustomTitleBar.ActualWidth) * ScalingFactor),
                    Height = (int)(CustomTitleBarDragArea.ActualHeight * ScalingFactor)
                }
            ];
        }

        private void CoreWebView2Actived(object sender, CoreWebView2InitializedEventArgs e)
        {
            InitMainWebAppViewCoreWebView2Binding();
            InitWebAppTransparentControl();
            (sender as WebView2).CoreWebView2Initialized -= CoreWebView2Actived;
        }

        private void OnSizeChanged(object sender, WindowSizeChangedEventArgs e)
            => UpdateDragRects();

        private void OnLoaded(object sender, WindowActivatedEventArgs e)
            => UpdateDragRects();

        private void UpdateDragRects()
        {
            AppWindow.TitleBar.SetDragRectangles(MainWindowTitleBarDragRects);
        }

        private void InitMainWebAppViewCoreWebView2Binding()
        {
            MainWebAppView.CoreWebView2.NewWindowRequested += WebView2Tweak.CoreWebView2NewWindowReflectToCurrent;
            MainWebAppView.CoreWebView2.NavigationCompleted += WebAppLoaded;
        }

        private void InitWebAppTransparentControl()
        {
            GoBackButton.Click += GoBackClicked;
            RefreshButton.Click += RefreshClicked;
        }

        private void WebAppLoaded(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            Title = (sender as CoreWebView2).DocumentTitle;
        }

        private void GoBackClicked(object sender, RoutedEventArgs e)
        {
            MainWebAppView.CoreWebView2.GoBack();
        }

        private void RefreshClicked(object sender, RoutedEventArgs e)
        {
            MainWebAppView.CoreWebView2.Reload();
        }
    }
}
