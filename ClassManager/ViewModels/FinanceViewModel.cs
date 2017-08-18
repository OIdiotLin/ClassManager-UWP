using ClassManager.Models;
using ClassManager.Networks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManager.ViewModels
{
    public class FinanceViewModel: BaseViewModel
    {
        APIService api = new APIService();

        private Finance _new_finance;
        /// <summary>
        /// 新建收支
        /// </summary>
        public Finance NewFinance {
            get {
                return _new_finance;
            }
            set {
                this._new_finance = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Finance> _finances;
        /// <summary>
        /// 班费收支集合
        /// </summary>
        public ObservableCollection<Finance> Finances {
            get {
                return _finances;
            }
            set {
                this._finances = value;
                OnPropertyChanged();
            }
        }

        private float _balance;
        /// <summary>
        /// 班费余额
        /// </summary>
        public float Balance {
            get {
                return _balance;
            }
            set {
                this._balance = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public async Task InitializeAsync()
        {
            var list = await api.GetFinanceList();
            Finances = new ObservableCollection<Finance>(list.OrderByDescending(f => f.Date));
            Balance = await api.GetBalance();
            NewFinance = new Finance();
        }
    }
}
