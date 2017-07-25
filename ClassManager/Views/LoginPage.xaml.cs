using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ClassManager.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            float centerX = (float)Window.Current.Bounds.Width / 2;
            float centerY = (float)Window.Current.Bounds.Width / 2;

            await Background.Scale(scaleX: 1.5f, scaleY: 1.5f, centerX: centerX, centerY: centerY, duration: 0).StartAsync();
            var animBG = Background.Scale(scaleX: 1f, scaleY: 1f, centerX: centerX, centerY: centerY)
                                 .Blur(value: 15);
            animBG.SetDurationForAll(10000);
            animBG.Start();

            await Logo.Fade(value: 0, duration: 0).StartAsync();
            await LoginAsVisitorPanel.Fade(value: 0, duration: 0).StartAsync();
            await LoginAsAdminPanel.Fade(value: 0, duration: 0).StartAsync();

            var animLogo = Logo.Offset(offsetY: 20).Fade(value: 1).SetDurationForAll(3000).SetDelayForAll(1500);
            var animVisitorPanel = LoginAsVisitorPanel.Offset(offsetY: 20).Fade(value: 1).SetDurationForAll(3000).SetDelayForAll(3500);
            var animAdminPanel = LoginAsAdminPanel.Offset(offsetY: 20).Fade(value: 1).SetDurationForAll(3000).SetDelayForAll(4000);

            animLogo.Start();
            animVisitorPanel.Start();
            animAdminPanel.Start();
        }

        private void LoginAsVisitorButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void LoginAsAdminButton_Click(object sender, RoutedEventArgs e)
        {
            Background.Blur(value: 15, duration: 10000).Start();
            await Background.Scale(scaleX: 1, scaleY: 1, centerX: 350, centerY: 236, duration: 10000).StartAsync();
        }
    }
}
