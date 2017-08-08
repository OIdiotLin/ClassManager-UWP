using ClassManager.Utils;
using ClassManager.ViewModels;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
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
        /// <summary>
        /// ViewModel
        /// </summary>
        private LoginViewModel vm;

        /// <summary>
        /// 清除 token
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            App.token = string.Empty;
        }

        public LoginPage()
        {
            this.InitializeComponent();

            vm = new LoginViewModel();

            LoginAsVisitorEnterButton.FontSize = 20;
            LoginAsAdminEnterButton.FontSize = 20;
        }

        /// <summary>
        /// 载入页面时的动画效果，渐变下沉
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            float centerX = (float)Window.Current.Bounds.Width / 2;
            float centerY = (float)Window.Current.Bounds.Width / 2;

            await BackgroundPic.Scale(scaleX: 1.5f, scaleY: 1.5f, centerX: centerX, centerY: centerY, duration: 0).StartAsync();
            var animBG = BackgroundPic.Scale(scaleX: 1f, scaleY: 1f, centerX: centerX, centerY: centerY)
                                 .Blur(value: 15);
            animBG.SetDurationForAll(10000);
            animBG.Start();

            await Logo.Fade(value: 0, duration: 0).StartAsync();
            await LoginAsVisitorPanel.Fade(value: 0, duration: 0).StartAsync();
            await LoginAsAdminPanel.Fade(value: 0, duration: 0).StartAsync();

            var animLogo = Logo.Offset(offsetY: 20).Fade(value: 1).SetDurationForAll(3000).SetDelayForAll(1000);
            var animAdminPanel = LoginAsAdminPanel.Offset(offsetY: 20).Fade(value: 1).SetDurationForAll(3000).SetDelayForAll(2000);
            var animVisitorPanel = LoginAsVisitorPanel.Offset(offsetY: 20).Fade(value: 1).SetDurationForAll(3000).SetDelayForAll(2500);

            animLogo.Start();
            animVisitorPanel.Start();
            animAdminPanel.Start();

            PasswordTextBox.Focus(FocusState.Pointer);
        }

        /// <summary>
        /// 跳转到主页面
        /// </summary>
        private void NavigateToMainPage()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage));
        }
                
        private async void LoginAsAdminEnterButton_Click(object sender, RoutedEventArgs e)
        {
            await Login();
        }

        /// <summary>
        /// 登录
        /// </summary>
        private async Task Login()
        {
            LoginAsAdminProgressRing.IsActive = true;
            await vm.Login(PasswordTextBox.Password);
            if (vm.IsLoginSuccess)
            {
                NavigateToMainPage();
            }
            else
            {
                LoginAsAdminProgressRing.IsActive = false;
                await DisplayLoginAsAdminFailDialog();
                PasswordTextBox.Password = string.Empty;
                PasswordTextBox.Focus(FocusState.Pointer);
            }
        }

        /// <summary>
        /// 管理员登录失败时弹出对话框
        /// </summary>
        private async Task DisplayLoginAsAdminFailDialog()
        {
            var dialog = new ContentDialog()
            {
                Content = ResourceLoader.GetString("LoginFailDialog_Content"),
                Title = ResourceLoader.GetString("LoginFailDialog_Title"),
                PrimaryButtonText = ResourceLoader.GetString("LoginFailDialog_ButtonText")
            };
            await dialog.ShowAsync();
        }

        private void LoginAsVisitorEnterButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToMainPage();
        }

        /// <summary>
        /// 捕获输入密码时的键盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasswordTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case VirtualKey.Enter:
                    Login();
                    break;
                default:
                    break;
            }
        }
    }
}
