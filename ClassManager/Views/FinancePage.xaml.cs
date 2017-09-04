using ClassManager.Controls;
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
    /// 用于展示班费收支的页面
    /// </summary>
    public sealed partial class FinancePage : Page
    {
        private FinanceViewModel vm;
        
        /// <summary>
        /// 委托
        /// </summary>
        private delegate void FinancePageDelegate();

        /// <summary>
        /// 事件-<see cref="vm"/>更新
        /// </summary>
        private event FinancePageDelegate OnRefreshed;

        public FinancePage()
        {
            this.InitializeComponent();
            vm = new FinanceViewModel();
            this.OnRefreshed += new FinancePageDelegate(AddDelegate);
        }

        /// <summary>
        /// 导航至<see cref="FinancePage"/>时，进行初始化。
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await vm.InitializeAsync();
            OnRefreshed?.Invoke();

            if (App.IsAdmin)
            {
                AddButton.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// 更新<see cref="vm"/>
        /// </summary>
        /// <param name="result"></param>
        public async void Refresh(bool result)
        {
            await vm.InitializeAsync();
            OnRefreshed?.Invoke();
        }

        /// <summary>
        /// 为<see cref="FinanceListView"/>中的Controls注册Delegate(Refresh)
        /// </summary>
        private void AddDelegate()
        {
            for (int i = 0; i < FinanceListView.Items.Count; i++)
            {
                var container = FinanceListView.ContainerFromIndex(i);
                FinanceItemTemplate item = MyVisualTreeHelper.SearchVisualTree<FinanceItemTemplate>(container);
                if(item != null)
                    item.OnSubmitedSuccess += new FinanceItemTemplate.FinanceItemDelegate(Refresh);
                
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AddingFianceControl.OnSubmitedSuccess += new Controls.EditingFinanceTemplate.EditingFinanceDelegate(Refresh);
        }
    }
}
