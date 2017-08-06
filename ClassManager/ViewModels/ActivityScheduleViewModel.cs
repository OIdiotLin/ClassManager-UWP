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
    public class ActivityScheduleViewModel: BaseViewModel
    {
        APIService api = new APIService();

        /// <summary>
        /// 活动
        /// </summary>
        private ObservableCollection<Activity> _activities;
        public ObservableCollection<Activity> Activities {
            get {
                return this._activities;
            }
            set {
                this._activities = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 请求并更新<seealso cref="Activities"/>
        /// </summary>
        /// <returns></returns>
        public async Task GetActivities()
        {
            var list = await api.GetActivityList();
            Activities = new ObservableCollection<Activity>();
            foreach(var item in list)
            {
                Activities.Add(item);
            }
        }

    }
}
