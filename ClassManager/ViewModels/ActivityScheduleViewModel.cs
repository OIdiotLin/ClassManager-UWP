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

        private ObservableCollection<Activity> _activities_on_display;
        /// <summary>
        /// 用于展示的（经过过滤的）活动
        /// </summary>
        public ObservableCollection<Activity> ActivitiesOnDisplay {
            get {
                return this._activities_on_display;
            }
            set {
                this._activities_on_display = value;
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
            Activities = new ObservableCollection<Activity>(list.OrderByDescending(a => a.Date));
            ActivitiesOnDisplay = new ObservableCollection<Activity>(Activities);
        }
        
        /// <summary>
        /// 按关键字进行筛选，并更新<see cref="ActivitiesOnDisplay"/>。
        /// </summary>
        /// <param name="keywords">关键字</param>
        public void FilterActivated(string keywords)
        {
            if (keywords == "")
            {
                ActivitiesOnDisplay = new ObservableCollection<Activity>(Activities);
                return;
            }
            ActivitiesOnDisplay.Clear();
            foreach(var item in Activities)
            {
                if (item.Name.Contains(keywords) || item.Place.Contains(keywords))
                {
                    ActivitiesOnDisplay.Add(item);
                }
            }
        }
    }
}
