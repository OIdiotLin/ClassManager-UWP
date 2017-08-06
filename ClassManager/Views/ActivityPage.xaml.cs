using ClassManager.ViewModels;
using ClassManager.Models;
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
    /// <see cref="ActivityPage"/>的主页面，其中的<see cref="ActivityMainFrame"/>是对<see cref="Activity"/>的UI重心
    /// </summary>
    public sealed partial class ActivityPage : Page
    {
        public ActivityPage()
        {
            this.InitializeComponent();
        }

        private Type _frame_page_type;
        public Type FramePageType {
            get {
                return _frame_page_type;
            }
            set {
                _frame_page_type = value;
                if (App.IsAdmin)
                {
                    GoBackButton.Visibility = (_frame_page_type == typeof(ActivityDetailsPage) ? Visibility.Visible : Visibility.Collapsed);
                    AddButton.Visibility = (_frame_page_type == typeof(ActivitySchedulePage) ? Visibility.Visible : Visibility.Collapsed);
                    DeleteButton.Visibility = (_frame_page_type == typeof(ActivityDetailsPage) ? Visibility.Visible : Visibility.Collapsed);
                    UpdateButton.Visibility = (_frame_page_type == typeof(ActivityDetailsPage) ? Visibility.Visible : Visibility.Collapsed);
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            FramePageType = typeof(ActivitySchedulePage);
        }

        ///// <summary>
        ///// 由<see cref="ActivityMainFrame"/>的内容物决定<see cref="AddButton"/>的<see cref="Visibility"/>：
        ///// 当内容为<see cref="ActivitySchedulePage"/>时，<see cref="AddButton"/>为可见，
        ///// 当内容为<see cref="ActivityDetailsPage"/>时，<see cref="AddButton"/>为不可见。
        ///// </summary>
        //private Visibility AddButtonVisibility {
        //    get {
        //        return ActivityMainFrame.SourcePageType == typeof(ActivitySchedulePage) && App.IsAdmin ? Visibility.Visible : Visibility.Collapsed;
        //    }
        //}

        ///// <summary>
        ///// 由<see cref="ActivityMainFrame"/>的内容物决定<see cref="DeleteButton"/>的<see cref="Visibility"/>：
        ///// 当内容为<see cref="ActivitySchedulePage"/>时，<see cref="DeleteButton"/>为不可见，
        ///// 当内容为<see cref="ActivityDetailsPage"/>时，<see cref="DeleteButton"/>为可见。
        ///// </summary>
        //private Visibility DeleteButtonVisibility {
        //    get {
        //        return ActivityMainFrame.SourcePageType == typeof(ActivityDetailsPage) && App.IsAdmin ? Visibility.Visible : Visibility.Collapsed;
        //    }
        //}

        ///// <summary>
        ///// 由<see cref="ActivityMainFrame"/>的内容物决定<see cref="UpdateButton"/>的<see cref="Visibility"/>：
        ///// 当内容为<see cref="ActivitySchedulePage"/>时，<see cref="UpdateButton"/>为不可见，
        ///// 当内容为<see cref="ActivityDetailsPage"/>时，<see cref="UpdateButton"/>为可见。
        ///// </summary>
        //private Visibility UpdateButtonVisibility {
        //    get {
        //        return ActivityMainFrame.SourcePageType == typeof(ActivityDetailsPage) && App.IsAdmin ? Visibility.Visible : Visibility.Collapsed;
        //    }
        //}

        /// <summary>
        /// <see cref="ActivityMainFrame"/>跳转到<paramref name="sourcePageType"/>，并展示<paramref name="sourceActivity"/>
        /// </summary>
        /// <param name="sourcePageType">目标页面</param>
        /// <param name="sourceActivity">传递给目标页面的<see cref="Activity"/></param>
        public void NavigateFrame(Type sourcePageType, Activity sourceActivity = null)
        {
            this.ActivityMainFrame.Navigate(sourcePageType, sourceActivity);
            FramePageType = sourcePageType;
        }

        /// <summary>
        /// <see cref="GoBackButton"/>被点击，返回<see cref="ActivitySchedulePage"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            this.ActivityMainFrame.Navigate(typeof(ActivitySchedulePage));
            FramePageType = typeof(ActivitySchedulePage);
        }

        /// <summary>
        /// <see cref="AddButton"/>被点击，打开<see cref="ActivityDetailsPage"/>，并传入新<see cref="Activity"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            this.ActivityMainFrame.Navigate(typeof(ActivityDetailsPage), new Activity());
            FramePageType = typeof(ActivityDetailsPage);
        }

        /// <summary>
        /// <see cref="DeleteButton"/>被点击，调用<see cref="ActivityDetailsPage.DeleteActivity"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            (this.ActivityMainFrame.Content as ActivityDetailsPage).DeleteActivity();
        }

        /// <summary>
        /// <see cref="UpdateButton"/>被点击，调用<see cref="ActivityDetailsPage.UpdateActivity"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            (this.ActivityMainFrame.Content as ActivityDetailsPage).UpdateActivity();
        }
    }
}
