using ClassManager.Models;
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
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class PersonPage : Page
    {
        private PersonViewModel vm;

        public PersonPage()
        {
            this.InitializeComponent();
            vm = new PersonViewModel();
        }

        /// <summary>
        /// 当导航到PersonPage的时候进行的操作
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            LoadingDataProgressRing.IsActive = true;
            await vm.Init();    // 更新 viewmodel
            LoadingDataProgressRing.IsActive = false;
        }

        private void AdaptiveGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vm.personOnDisplay = e.AddedItems.FirstOrDefault() as Person;
        }
    }
    
}
