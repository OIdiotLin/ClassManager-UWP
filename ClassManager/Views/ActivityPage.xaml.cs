﻿using ClassManager.ViewModels;
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
using ClassManager.Utils;
using Windows.ApplicationModel.Appointments;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ClassManager.Views
{
    /// <summary>
    /// <see cref="ActivityPage"/>的主页面，其中的<see cref="ActivityMainFrame"/>可包含
    /// <see cref="ActivityDetailsPage"/>、<see cref="ActivitySchedulePage"/>、
    /// <see cref="ActivityEditingPage"/>。
    /// </summary>
    public sealed partial class ActivityPage : Page
    {
        public ActivityPage()
        {
            this.InitializeComponent();

            ToolTipService.SetToolTip(CreateAppointmentButton,
                                      ResourceLoader.GetString("CreateAppointmentButton_ToolTip"));
            ToolTipService.SetPlacement(CreateAppointmentButton, PlacementMode.Top);

            SearchBox.PlaceholderText =
                ResourceLoader.GetString("ActivitySearchBox_PlaceholderText");
            CreateAppointmentButton_Flyout_ConfirmButton.Content =
                ResourceLoader.GetString("CreateAppointmentButton_Flyout_ConfirmButton");
            CreateAppointmentButton_Flyout_Body.Text =
                ResourceLoader.GetString("CreateAppointmentButton_Flyout_Body");
        }

        private Type _frame_page_type;
        public Type FramePageType {
            get {
                return _frame_page_type;
            }
            set {
                _frame_page_type = value;
                SearchBox.Text = "";

                GoBackButton.Visibility = (_frame_page_type == typeof(ActivitySchedulePage) ? Visibility.Collapsed : Visibility.Visible);
                SearchBox.Visibility = (_frame_page_type == typeof(ActivitySchedulePage) ? Visibility.Visible : Visibility.Collapsed);
                CreateAppointmentButton.Visibility = (_frame_page_type == typeof(ActivityDetailsPage) ? Visibility.Visible : Visibility.Collapsed);

                if (App.IsAdmin)
                {
                    if(_frame_page_type == typeof(ActivitySchedulePage))
                    {
                        AddButton.Visibility = Visibility.Visible;
                        ConfirmButton.Visibility = Visibility.Collapsed;
                        DeleteButton.Visibility = Visibility.Collapsed;
                        UpdateButton.Visibility = Visibility.Collapsed;
                        Divider.Visibility = Visibility.Collapsed;
                    }
                    else if(_frame_page_type == typeof(ActivityDetailsPage))
                    {
                        AddButton.Visibility = Visibility.Collapsed;
                        ConfirmButton.Visibility = Visibility.Collapsed;
                        DeleteButton.Visibility = Visibility.Visible;
                        UpdateButton.Visibility = Visibility.Visible;
                        Divider.Visibility = Visibility.Visible;
                    }
                    else if(_frame_page_type == typeof(ActivityEditingPage))
                    {
                        AddButton.Visibility = Visibility.Collapsed;
                        ConfirmButton.Visibility = Visibility.Visible;
                        DeleteButton.Visibility = Visibility.Collapsed;
                        UpdateButton.Visibility = Visibility.Collapsed;
                        Divider.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            FramePageType = typeof(ActivitySchedulePage);
        }
        
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
        /// <see cref="AddButton"/>被点击，切换至<see cref="ActivityEditingPage"/>，并传入新<see cref="Activity"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            this.ActivityMainFrame.Navigate(typeof(ActivityEditingPage), new Activity());
            FramePageType = typeof(ActivityEditingPage);
            ConfirmButton.Tag = "Add";
        }

        /// <summary>
        /// <see cref="DeleteButton"/>被点击，调用<see cref="ActivityDetailsPage.DeleteActivity"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if(await(this.ActivityMainFrame.Content as ActivityDetailsPage).DeleteActivity())
            {
                ActivityMainFrame.Navigate(typeof(ActivitySchedulePage));
                FramePageType = typeof(ActivitySchedulePage);
            }
        }

        /// <summary>
        /// <see cref="UpdateButton"/>被点击，切换至<see cref="ActivityEditingPage"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            this.ActivityMainFrame.Navigate(typeof(ActivityEditingPage),
                (ActivityMainFrame.Content as ActivityDetailsPage).SourceActivity);
            FramePageType = typeof(ActivityEditingPage);
            ConfirmButton.Tag = "Update";
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FramePageType = typeof(ActivitySchedulePage);
        }

        /// <summary>
        /// <see cref="ConfirmButton"/>被点击，根据<see cref="ActivityMainFrame"/>内容物决定操作类型：
        /// 若是添加操作则调用<see cref="ActivityDetailsPage.ConfirmAdd"/>，
        /// 若是修改操作则调用<see cref="ActivityDetailsPage.ConfirmUpdate"/>。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var tag = (sender as Button).Tag as string;
            if ((sender as Button).Tag as string =="Add" ||(sender as Button).Tag as string == "Update")
            {
                UploadingProgressRingPanel.Visibility = Visibility.Visible;
                var result = await (this.ActivityMainFrame.Content as ActivityEditingPage).Confirm(tag);
                UploadingProgressRingPanel.Visibility = Visibility.Collapsed;
                if (result)
                {
                    await new ContentDialog()
                    {
                        Title = ResourceLoader.GetString(
                            tag == "Add" ? "AddActivitySuccessDialog_Title" : "UpdateActivitySuccessDialog_Title"),
                        PrimaryButtonText = ResourceLoader.GetString(
                            tag == "Add" ? "AddActivitySuccessDialog_PrimaryButtonText" : "UpdateActivitySuccessDialog_PrimaryButtonText")
                    }.ShowAsync();

                    ActivityMainFrame.Navigate(typeof(ActivitySchedulePage));   // 如果成功则导航至 SchedulePage
                    FramePageType = typeof(ActivitySchedulePage);
                }
                else
                {
                    await new ContentDialog()
                    {
                        Title = ResourceLoader.GetString(
                            tag == "Add" ? "AddActivityFailDialog_Title" : "UpdateActivityFailDialog_Title"),
                        Content = ResourceLoader.GetString(
                            tag == "Add" ? "AddActivityFailDialog_Content" : "UpdateActivityFailDialog_Content"),
                        PrimaryButtonText = ResourceLoader.GetString(
                            tag == "Add" ? "AddActivityFailDialog_PrimaryButtonText" : "UpdateActivityFailDialog_PrimaryButtonText")
                    }.ShowAsync();
                }
            }
        }
        
        private async void CreateAppointmentButton_Flyout_ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var activity = (this.ActivityMainFrame.Content as ActivityDetailsPage).SourceActivity;
            var appointment = new Appointment();

            var dateArr = (from x in activity.Date.Split('-') select Int16.Parse(x)).ToArray();
            var timeArr = (from x in activity.Time.Split(':') select Int16.Parse(x)).ToArray();
            var datetime = new DateTime(year: dateArr[0], month: dateArr[1], day: dateArr[2],
                                        hour: timeArr[0], minute: timeArr[1], second: 0);

            appointment.StartTime = new DateTimeOffset(datetime);
            appointment.Subject = activity.Name;
            appointment.Location = activity.Place;
            appointment.Details = activity.Content;
            appointment.Duration = TimeSpan.FromHours(1);
            appointment.Reminder = TimeSpan.FromDays(1);
            
            String appointmentId = await AppointmentManager.ShowAddAppointmentAsync(appointment, new Rect(), Windows.UI.Popups.Placement.Default);

        }

        private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            (this.ActivityMainFrame.Content as ActivitySchedulePage)
                .Search((sender as AutoSuggestBox).Text);
        }
    }
}
