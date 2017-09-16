using ClassManager.Networks;
using ClassManager.Utils;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ClassManager.Controls
{
    public sealed partial class EditingFinanceTemplate : UserControl
    {
        APIService api = new APIService();

        /// <summary>
        /// 绑定到<see cref="EditingFinanceTemplate"/>的数据源
        /// </summary>
        public Models.Finance Finance {
            get {
                return this.DataContext as Models.Finance;
            }
            set {
                this.DataContext = value;
            }
        }

        public delegate void EditingFinanceDelegate(bool result);

        public event EditingFinanceDelegate OnSubmitedSuccess;

        public EditingFinanceTemplate()
        {
            this.InitializeComponent();

            this.DataContextChanged += (s, e) => Bindings.Update();
        }

        /// <summary>
        /// 提交更改，api取决于Tag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            SubmitProgressRing.IsActive = true;
            SubmitButton.IsEnabled = false;

            bool result = (string)this.Tag == "Add" ?
                await api.AddFinance(Finance) :
                await api.UpdateFinance(Finance.Id, Finance);

            SubmitProgressRing.IsActive = false;
            SubmitButton.IsEnabled = true;

            if (result)
            {
                await new ContentDialog
                {
                    Title = ResourceLoader.GetString((string)this.Tag == "Add" ?
                                                     "AddFinanceSuccessDialog_Title" :
                                                     "UpdateFinanceSuccessDialog_Title"),
                    PrimaryButtonText = ResourceLoader.GetString((string)this.Tag == "Add" ?
                                                                 "AddFinanceSuccessDialog_PrimaryButtonText" :
                                                                 "UpdateFinanceSuccessDialog_PrimaryButtonText"),
                    //DefaultButton = ContentDialogButton.Primary
                }.ShowAsync();

                OnSubmitedSuccess?.Invoke(true);    // 调用委托，更新页面
            }
            else
            {
                await new ContentDialog
                {
                    Title = ResourceLoader.GetString((string)this.Tag == "Add" ?
                                                     "AddFinanceFailDialog_Title" :
                                                     "UpdateFinanceFailDialog_Title"),
                    Content = ResourceLoader.GetString((string)this.Tag == "Add" ?
                                                       "AddFinanceFailDialog_Content" :
                                                       "UpdateFinanceFailDialog_Content"),
                    PrimaryButtonText = ResourceLoader.GetString((string)this.Tag == "Add" ?
                                                                 "AddFinanceFailDialog_PrimaryButtonText" :
                                                                 "UpdateFinanceFailDialog_PrimaryButtonText"),
                    //DefaultButton = ContentDialogButton.Primary
                }.ShowAsync();
            }
        }
    }
}
