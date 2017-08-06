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

        /// <summary>
        /// 由<see cref="ActivityMainFrame"/>的内容物决定<see cref="AddButton"/>的<see cref="Visibility"/>：
        /// 当内容为<see cref="ActivitySchedulePage"/>时，<see cref="AddButton"/>为可见，
        /// 当内容为<see cref="ActivityDetailsPage"/>时，<see cref="AddButton"/>为不可见。
        /// </summary>
        private Visibility AddButtonVisibility {
            get {
                return ActivityMainFrame.SourcePageType == typeof(ActivitySchedulePage) && App.IsAdmin ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 由<see cref="ActivityMainFrame"/>的内容物决定<see cref="DeleteButton"/>的<see cref="Visibility"/>：
        /// 当内容为<see cref="ActivitySchedulePage"/>时，<see cref="DeleteButton"/>为不可见，
        /// 当内容为<see cref="ActivityDetailsPage"/>时，<see cref="DeleteButton"/>为可见。
        /// </summary>
        private Visibility DeleteButtonVisibility {
            get {
                return ActivityMainFrame.SourcePageType == typeof(ActivityDetailsPage) && App.IsAdmin ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 由<see cref="ActivityMainFrame"/>的内容物决定<see cref="UpdateButton"/>的<see cref="Visibility"/>：
        /// 当内容为<see cref="ActivitySchedulePage"/>时，<see cref="UpdateButton"/>为不可见，
        /// 当内容为<see cref="ActivityDetailsPage"/>时，<see cref="UpdateButton"/>为可见。
        /// </summary>
        private Visibility UpdateButtonVisibility {
            get {
                return ActivityMainFrame.SourcePageType == typeof(ActivityDetailsPage) && App.IsAdmin ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// <see cref="ActivityMainFrame"/>跳转到<paramref name="sourcePageType"/>，并展示<paramref name="sourceActivity"/>
        /// </summary>
        /// <param name="sourcePageType">目标页面</param>
        /// <param name="sourceActivity">传递给目标页面的<see cref="Activity"/></param>
        public void NavigateFrame(Type sourcePageType, Activity sourceActivity)
        {
            this.ActivityMainFrame.Navigate(sourcePageType, sourceActivity);
        }

    }
}
