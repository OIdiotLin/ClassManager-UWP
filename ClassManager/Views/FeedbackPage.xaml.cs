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
    public sealed partial class FeedbackPage : Page
    {
        private FeedbackViewModel vm;

        public FeedbackPage()
        {
            this.InitializeComponent();

            vm = new FeedbackViewModel();
            vm.Initialize();
        }

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            vm.SourceFeedback.Category = CategoryAdviceButton.IsChecked == true ? "Advice" : "Bug";
            var result = await vm.SumbitFeedback();
            if (result)
            {
                await new ContentDialog
                {
                    Title = ResourceLoader.GetString("AddFeedbackSuccessDialog_Title"),
                    Content = ResourceLoader.GetString("AddFeedbackSuccessDialog_Content"),
                    PrimaryButtonText = ResourceLoader.GetString("AddFeedbackSuccessDialog_PrimaryButtonText")
                }.ShowAsync();
            }
            else
            {
                await new ContentDialog
                {
                    Title = ResourceLoader.GetString("AddFeedbackFailDialog_Title"),
                    Content = ResourceLoader.GetString("AddFeedbackFailDialog_Content"),
                    PrimaryButtonText = ResourceLoader.GetString("AddFeedbackFailDialog_PrimaryButtonText")
                }.ShowAsync();
            }
        }

        private void SummaryTextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            SummaryWordCounterTextBlock.Text = string.Format("{0}/50", (sender as TextBox).Text.Length);
        }

        private void DetailsTextBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            DetailsWordCounterTextBlock.Text = string.Format("{0}/500", (sender as TextBox).Text.Length);
        }
    }
}
