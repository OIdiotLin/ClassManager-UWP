using ClassManager.Models;
using ClassManager.Utils;
using ClassManager.ViewModels;
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
    /// 显示<see cref="Activity"/>详细信息或新建<see cref="Activity"/>的页面
    /// </summary>
    public sealed partial class ActivityDetailsPage : Page
    {
        private ActivityDetailsViewModel vm;

        public ActivityDetailsPage()
        {
            this.InitializeComponent();
            vm = new ActivityDetailsViewModel();
        }

        /// <summary>
        /// 当导航至<see cref="ActivityDetailsPage"/>时，更新<see cref="vm"/>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            vm.Initialize(e.Parameter as Activity);
        }

        /// <summary>
        /// 调用<see cref="ActivityDetailsViewModel.DeleteActivityAsync()"/>删除当前的活动，并处理删除结果
        /// </summary>
        public async void DeleteActivity()
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = ResourceLoader.GetString("DeleteActivityDialog_Title"),
                Content = String.Format(ResourceLoader.GetString("DeleteActivityDialog_Content"), vm.ActivityOnDisplay.Name),
                PrimaryButtonText = ResourceLoader.GetString("DeleteActivityDialog_PrimaryButtonText"),
                SecondaryButtonText = ResourceLoader.GetString("DeleteActivityDialog_SecondaryButtonText")
            };
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var deleteResult = await vm.DeleteActivityAsync();
                await new ContentDialog()
                {
                    Title = ResourceLoader.GetString(
                        deleteResult ? "DeleteActivitySuccessDialog_Title" : "DeleteActivityFailDialog_Title"),
                    Content = deleteResult ? null : ResourceLoader.GetString("DeleteActivityFailDialog_Content"),
                    PrimaryButtonText = ResourceLoader.GetString(
                        deleteResult ? "DeleteActivitySuccessDialog_PrimaryButtonText" : "DeleteActivityFailDialog_PrimaryButtonText"),
                }.ShowAsync();
            }
        }
        
    }
}
