using ClassManager.Networks;
using ClassManager.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class FinanceItemTemplate : UserControl
    {
        private APIService api = new APIService();

        public delegate void FinanceItemDelegate(bool result);

        /// <summary>
        /// 事件 - 提交成功
        /// </summary>
        public event FinanceItemDelegate OnSubmitedSuccess;

        /// <summary>
        /// 单笔收支数据源
        /// </summary>
        public Models.Finance Finance {
            get {
                return this.DataContext as Models.Finance;
            }
        }

        public FinanceItemTemplate()
        {
            this.InitializeComponent();

            this.DataContextChanged += (s, e) => Bindings.Update();

            EditingFinanceControl.OnSubmitedSuccess += new EditingFinanceTemplate.EditingFinanceDelegate(ChildEventPostUp);
        }

        /// <summary>
        /// 子控件事件沿父链上传
        /// </summary>
        /// <param name="result"></param>
        private void ChildEventPostUp(bool result)
        {
            OnSubmitedSuccess?.Invoke(true);
        }

        /// <summary>
        /// 当指针进入时，显示操作按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (App.IsAdmin)
            {
                DeleteButton.Visibility = Visibility.Visible;
                EditButton.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// 当指针退出时，隐藏操作按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (App.IsAdmin)
            {
                DeleteButton.Visibility = Visibility.Collapsed;
                EditButton.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 单击标题栏，显示详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventTextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(DetailsTextBlock.Visibility == Visibility.Visible)
            {
                DetailsTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                DetailsTextBlock.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// <see cref="DeleteButton"/>被点击，进入删除操作。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var confirmResult = await new ContentDialog
            {
                Title = ResourceLoader.GetString("DeleteFinanceDialog_Title"),
                Content = String.Format(ResourceLoader.GetString("DeleteFinanceDialog_Content"), Finance.Event),
                PrimaryButtonText = ResourceLoader.GetString("DeleteFinanceDialog_PrimaryButtonText"),
                SecondaryButtonText = ResourceLoader.GetString("DeleteFinanceDialog_SecondaryButtonText"),
                DefaultButton = ContentDialogButton.Primary
            }.ShowAsync();

            if(confirmResult == ContentDialogResult.Primary)
            {
                var result = await api.DeleteFinance(Finance);
                if (result)
                {
                    await new ContentDialog
                    {
                        Title = ResourceLoader.GetString("DeleteFinanceSuccessDialog_Title"),
                        PrimaryButtonText = ResourceLoader.GetString("DeleteFinanceSuccessDialog_PrimaryButtonText"),
                        DefaultButton = ContentDialogButton.Primary
                    }.ShowAsync();

                    OnSubmitedSuccess?.Invoke(true);    // 调用委托，更新页面
                }
                else
                {
                    await new ContentDialog
                    {
                        Title = ResourceLoader.GetString("DeleteFinanceFailDialog_Title"),
                        Content = ResourceLoader.GetString("DeleteFinanceFailDialog_Content"),
                        PrimaryButtonText = ResourceLoader.GetString("DeleteFinanceFailDialog_PrimaryButtonText"),
                        DefaultButton = ContentDialogButton.Primary
                    }.ShowAsync();
                }
            }
        }

        /// <summary>
        /// <see cref="DetailsButton"/>被点击，打开详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (DetailsTextBlock.Visibility == Visibility.Visible)
            {
                DetailsTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                DetailsTextBlock.Visibility = Visibility.Visible;
            }
        }
    }
}
