using ClassManager.Models;
using ClassManager.Networks;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace ClassManager
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private APIService api = new APIService();
        private List<Person> persons;
        private List<Activity> activities;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button1_Click(object sender, RoutedEventArgs e)
        {
            activities = await api.GetActivityList();
            Activity newone = new Activity(activities[0]);
            newone.Participation = 5;
            bool status = await api.UpdateActivity(activities[0].Id, newone);
            await api.GetActivityList();
            if (status == true)
                img.Source = new BitmapImage { UriSource = new Uri(activities[0].ImagesUrl) };
        }
    }
}
