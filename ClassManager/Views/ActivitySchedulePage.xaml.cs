using ClassManager.ViewModels;
using ClassManager.Models;
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
using ClassManager.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ClassManager.Views
{
    /// <summary>
    /// <see cref="Activity"/>的一览页面
    /// </summary>
    public sealed partial class ActivitySchedulePage : Page
    {
        private ActivityScheduleViewModel vm;

        public ActivitySchedulePage()
        {
            this.InitializeComponent();
            vm = new ActivityScheduleViewModel();
        }

        /// <summary>
        /// 导航至<see cref="ActivityPage"/>时所触发的函数
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            LoadingProgressRing.IsActive = true;
            await vm.GetActivities();
            LoadingProgressRing.IsActive = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActivityGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var Root = ((Parent as Frame).Parent as Grid).Parent as ActivityPage;
            var sourceActivity = e.ClickedItem as Activity;
            Root.NavigateFrame(typeof(ActivityDetailsPage), sourceActivity);
        }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        /// <param name="s">关键字（活动名、活动地点）</param>
        public void Search(string s)
        {
            vm.FilterActivated(s);
        }
    }

}
