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
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ActivityEditingPage : Page
    {
        private ActivityEditingViewModel vm;

        public ActivityEditingPage()
        {
            this.InitializeComponent();
            vm = new ActivityEditingViewModel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await vm.Initialize(e.Parameter as Activity);

            foreach (var item in PersonGridView.Items)
            {
                if (vm.SourceActivity.Participators.Contains((item as Person).StudentNumber))
                    PersonGridView.SelectedItems.Add(item);
            }

            if(vm.SourceActivity.ImagesUrl == "")
            {
                ChooseUploadImageFileHintTextBlock.Text = ResourceLoader.GetString("ChooseUploadImageFile_Hint");
            }
        }

        /// <summary>
        /// 当<see cref="PersonGridView"/>的所选项发生变化时，更新<see cref="vm"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PersonGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vm.RefreshChosenParticipators(e.AddedItems, e.RemovedItems);
        }
        
        /// <summary>
        /// <see cref="ShuffleButton"/>被点击，进行随机选择，选择人数来自<see cref="ShuffleDemandTextBox"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShuffleButton_Click(object sender, RoutedEventArgs e)
        {
            PersonGridView.SelectedItems.Clear();
            foreach (var item in vm.RandomSelectItems())
            {
                PersonGridView.SelectedItems.Add(item);
            }
        }

        /// <summary>
        /// <see cref="SelectAllButton"/>被点击，更新<see cref="PersonGridView"/>是否全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectAllButton_Click(object sender, RoutedEventArgs e)
        {
            if((sender as ToggleButton).IsChecked == true)
            { 
                PersonGridView.SelectAll();
            }
            else
            {
                PersonGridView.SelectedItems.Clear();
            }
        }

        /// <summary>
        /// <see cref="ChooseImagesButton"/>被点击，
        /// 调用<see cref="Windows.Storage.Pickers.FileOpenPicker"/>选择要上传的文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ChooseImagesButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker()
            {
                ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail,
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary
            };
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".gif");

            vm.AddFiles(await picker.PickMultipleFilesAsync());
        }


        private void ImagesListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            vm.RemoveFile(e.ClickedItem as UploadingImageFile);
        }
    }
}
