using ClassManager.Models;
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
using Windows.UI;
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

            // admin则展示工具栏
            ToolsPanel.Visibility = App.IsAdmin ? Visibility.Visible : Visibility.Collapsed;

            EditButton.Tag = EditButtonState.ReadyForEdit;
            AddButton.Tag = AddButtonState.ReadyForAdd;

            LoadingDataProgressRing.IsActive = true;
            await vm.Init();    // 更新 viewmodel
            LoadingDataProgressRing.IsActive = false;

        }

        /// <summary>
        /// 当菜单中的item被选中时，切换右侧GridUnit的详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdaptiveGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vm.PersonOnDisplay = e.AddedItems.FirstOrDefault() as Person;
        }

        /// <summary>
        /// 修改信息按钮被点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        enum EditButtonState { ReadyForSave, ReadyForEdit }
        private string target_StudentNumber;
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            //((Button)sender).Tag = EditButtonState.ReadyForEdit
            var button = sender as Button;
            switch (button.Tag)
            {
                // 进入编辑模式
                case EditButtonState.ReadyForEdit:
                    target_StudentNumber = vm.PersonOnDisplay.StudentNumber;    // 保存原学号副本

                    button.Content = "\xE081";  // √
                    button.Tag = EditButtonState.ReadyForSave;
                    PersonInfoTextboxesEdit(true);

                    // 显示退出编辑模式按钮
                    Button cancelButton = new Button()
                    {
                        Name = "CancelButton",
                        Content = "\xE10A",
                        Style = (Style)this.Resources["SymbolButtonStyle"]
                    };
                    ToolsPanel.Children.Insert(1, cancelButton);
                    cancelButton.Click += CancelButton_Click;
                    cancelButton.SetValue(RelativePanel.AlignVerticalCenterWithPanelProperty, true);
                    cancelButton.SetValue(RelativePanel.LeftOfProperty, EditButton.Name);
                    DeleteButton.SetValue(RelativePanel.LeftOfProperty, cancelButton.Name);

                    AddButton.Visibility = Visibility.Collapsed;
                    AddButton.IsEnabled = false;
                    
                    break;
                
                // 保存、提交并退出编辑模式
                case EditButtonState.ReadyForSave:
                    ResetToolsButtonAfterEditing();
                    SubmitEditedInfo();
                    break;
            }
        }

        /// <summary>
        /// 退出编辑模式按钮被点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ResetToolsButtonAfterEditing();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ResetToolsButtonAfterEditing()
        {
            PersonInfoTextboxesEdit(false);
            DeleteButton.SetValue(RelativePanel.LeftOfProperty, EditButton.Name);
            ToolsPanel.Children.RemoveAt(1);

            AddButton.Visibility = Visibility.Visible;
            AddButton.IsEnabled = true;

            EditButton.Content = "\xE104";
            EditButton.Tag = EditButtonState.ReadyForEdit;
        }

        /// <summary>
        /// 修改表单的可编辑性
        /// </summary>
        /// <param name="editable">可修改</param>
        private void PersonInfoTextboxesEdit(bool editable = true)
        {
            foreach(var item in PersonInfoPanel.Children)
            {
                ((TextBox)item).IsReadOnly = !editable;
            }
            (PersonInfoPanel.Children[0] as TextBox).Focus(FocusState.Pointer);
        }

        /// <summary>
        /// 提交信息更改
        /// </summary>
        private async void SubmitEditedInfo()
        {
            if (await vm.UpdatePersonOnDisplay(target_StudentNumber))
            {
                await new ContentDialog()
                {
                    Title = ResourceLoader.GetString("UpdatePersonSuccessDialog_Title"),
                    PrimaryButtonText = ResourceLoader.GetString("UpdatePersonSuccessDialog_PrimaryButtonText"),
                    //DefaultButton = ContentDialogButton.Primary
                }.ShowAsync();
                await vm.Init();
            }
            else
            {
                await new ContentDialog()
                {
                    Content = ResourceLoader.GetString("UpdatePersonFailDialog_Content"),
                    Title = ResourceLoader.GetString("UpdatePersonFailDialog_Title"),
                    PrimaryButtonText = ResourceLoader.GetString("UpdatePersonFailDialog_PrimaryButtonText"),
                    //DefaultButton = ContentDialogButton.Primary
                }.ShowAsync();
            }
        }

        /// <summary>
        /// 删除按钮被点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var p = vm.PersonOnDisplay;

            var contentPanel = new StackPanel();
            var nameTextBlock = new TextBlock()
            {
                Text = string.Format(ResourceLoader.GetString("DeletePersonDialog_Content_Name"), p.Name)
            };
            var studentNumberTextBlock = new TextBlock()
            {
                Text = string.Format(ResourceLoader.GetString("DeletePersonDialog_Content_StudentNumber"), p.StudentNumber)
            };
            contentPanel.Children.Insert(0, nameTextBlock);
            contentPanel.Children.Insert(1, studentNumberTextBlock);
            contentPanel.Orientation = Orientation.Vertical;

            ContentDialog dialog = new ContentDialog()
            {
                Content = contentPanel,
                Title = ResourceLoader.GetString("DeletePersonDialog_Title"),
                PrimaryButtonText = ResourceLoader.GetString("DeletePersonDialog_PrimaryButtonText"),
                SecondaryButtonText = ResourceLoader.GetString("DeletePersonDialog_SecondaryButtonText"),
                //DefaultButton = ContentDialogButton.Primary
            };

            ContentDialogResult result = await dialog.ShowAsync();

            switch (result)
            {
                case ContentDialogResult.Primary:
                    if(await vm.DeletePersonOnDisplay())
                    {
                        await new ContentDialog()
                        {
                            Title = ResourceLoader.GetString("DeletePersonSuccessDialog_Title"),
                            PrimaryButtonText = ResourceLoader.GetString("DeletePersonSuccessDialog_PrimaryButtonText"),
                            //DefaultButton = ContentDialogButton.Primary
                        }.ShowAsync();

                    }
                    else
                    {
                        await new ContentDialog()
                        {
                            Content = ResourceLoader.GetString("DeletePersonFailDialog_Content"),
                            Title = ResourceLoader.GetString("DeletePersonFailDialog_Title"),
                            PrimaryButtonText = ResourceLoader.GetString("DeletePersonFailDialog_PrimaryButtonText"),
                            //DefaultButton = ContentDialogButton.Primary
                        }.ShowAsync();
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 添加新学生按钮被点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        enum AddButtonState { ReadyForAdd, ReadyForSave}
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            switch (button.Tag)
            {
                // 进入编辑模式
                case AddButtonState.ReadyForAdd:
                    vm.PersonOnDisplay = new Person();

                    button.Content = "\xE081";  // √
                    button.Tag = AddButtonState.ReadyForSave;
                    PersonInfoTextboxesEdit(true);

                    // 显示退出编辑模式按钮
                    Button cancelAddButton = new Button()
                    {
                        Name = "CancelAddButton",
                        Content = "\xE10A",
                        Style = (Style)this.Resources["SymbolButtonStyle"]
                    };
                    ToolsPanel.Children.Insert(3, cancelAddButton);
                    cancelAddButton.Click += CancelAddButton_Click;
                    cancelAddButton.SetValue(RelativePanel.AlignVerticalCenterWithPanelProperty, true);
                    cancelAddButton.SetValue(RelativePanel.RightOfProperty, AddButton.Name);

                    DeleteButton.Visibility = Visibility.Collapsed;
                    DeleteButton.IsEnabled = false;
                    EditButton.Visibility = Visibility.Collapsed;
                    EditButton.IsEnabled = false;

                    break;

                // 保存、提交并退出编辑模式
                case AddButtonState.ReadyForSave:
                    ResetToolsButtonAfterAdding();
                    SubmitAddedInfo();
                    break;
            }
        }
        
        /// <summary>
        /// 提交新建信息
        /// </summary>
        private async void SubmitAddedInfo()
        {
            if (await vm.AddPersonOnDisplay())
            {
                await new ContentDialog()
                {
                    Title = ResourceLoader.GetString("AddPersonSuccessDialog_Title"),
                    PrimaryButtonText = ResourceLoader.GetString("AddPersonSuccessDialog_PrimaryButtonText")
                }.ShowAsync();
                await vm.Init();
            }
            else
            {
                await new ContentDialog()
                {
                    Content = ResourceLoader.GetString("AddPersonFailDialog_Content"),
                    Title = ResourceLoader.GetString("AddPersonFailDialog_Title"),
                    PrimaryButtonText = ResourceLoader.GetString("AddPersonFailDialog_PrimaryButtonText")
                }.ShowAsync();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void ResetToolsButtonAfterAdding()
        {
            PersonInfoTextboxesEdit(false);
            ToolsPanel.Children.RemoveAt(3);

            DeleteButton.Visibility = Visibility.Visible;
            DeleteButton.IsEnabled = true;
            EditButton.Visibility = Visibility.Visible;
            EditButton.IsEnabled = true;

            AddButton.Content = "\xE109";
            AddButton.Tag = AddButtonState.ReadyForAdd;
        }

        private void CancelAddButton_Click(object sender, RoutedEventArgs e)
        {
            ResetToolsButtonAfterAdding();
            vm.PersonOnDisplay = AdaptiveGridView.SelectedItem as Person;
        }
        
        /// <summary>
        /// 搜索框文本变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            vm.GroupsFilter(sender.Text);
        }
    }
    
}
