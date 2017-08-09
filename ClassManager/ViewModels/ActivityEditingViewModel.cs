using ClassManager.Models;
using ClassManager.Networks;
using ClassManager.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManager.ViewModels
{
    public class ActivityEditingViewModel : BaseViewModel
    {
        APIService api = new APIService();

        public async Task Initialize(Activity activity = null)
        {
            ChosenParticipators = new ObservableCollection<Person>();
            Persons = new ObservableCollection<Person>((await api.GetPersonList()).OrderBy(x => (x.Pinyin)));
            SourceActivity = activity;

            string[] date = SourceActivity.Date.Split('-');
            string[] time = SourceActivity.Time.Split(':');

            DateTime datetime = new DateTime(Int16.Parse(date[0]), Int16.Parse(date[1]), Int16.Parse(date[2]),
                                             Int16.Parse(time[0]), Int16.Parse(time[1]), 0);

            Time = new TimeSpan(datetime.Hour, datetime.Minute, 0);
            Date = new DateTimeOffset(datetime);
            
        }

        /// <summary>
        /// 更新<see cref="ChosenParticipators"/>
        /// </summary>
        /// <param name="addedList">新选择的人员</param>
        /// <param name="removedList">取消选择的人员</param>
        public void RefreshChosenParticipators(IList<object> addedList, IList<object> removedList)
        {
            if (addedList != null)
            {
                foreach (var person in addedList)
                {
                    ChosenParticipators.Add(person as Person);
                }
            }
            if (removedList != null)
            {
                foreach (var person in removedList)
                {
                    ChosenParticipators.Remove(person as Person);
                }
            }
        }

        /// <summary>
        /// 随机选择<see cref="ShuffleDemand"/>个人员，对<see cref="Person.Participation"/>较小的略有偏好。
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Person> RandomSelectItems()
        {
            // TODO 还是决定在backend上写个api吧，用推荐算法。
            return new ObservableCollection<Person>(Rand.RandomItems(Persons.ToList(), ShuffleDemand));
        }

        private Activity _source_activity;
        /// <summary>
        /// 该页面的源<see cref="Activity"/>
        /// </summary>
        public Activity SourceActivity {
            get {
                return _source_activity;
            }
            set {
                this._source_activity = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Person> _persons;
        /// <summary>
        /// 用于在选择<see cref="ChosenParticipators"/>时的备选人员
        /// </summary>
        public ObservableCollection<Person> Persons {
            get {
                return _persons;
            }
            set {
                this._persons = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Person> _chosen_participators;
        /// <summary>
        /// 已选择的活动参与者
        /// </summary>
        public ObservableCollection<Person> ChosenParticipators {
            get {
                return _chosen_participators;
            }
            set {
                this._chosen_participators = value;
                OnPropertyChanged();
            }
        }

        private DateTimeOffset _date;
        /// <summary>
        /// 当前活动的日期
        /// </summary>
        public DateTimeOffset Date {
            get {
                return _date;
            }
            set {
                if (SourceActivity != null)
                    SourceActivity.Date = String.Format("{0}-{1}-{2}", value.Year, value.Month, value.Day);
                this._date = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan _time;
        /// <summary>
        /// 当前活动的时间
        /// </summary>
        public TimeSpan Time {
            get {
                return _time;
            }
            set {
                if (value != _time)
                {
                    if (SourceActivity != null)
                        SourceActivity.Time = String.Format("{0}:{1}", value.Hours, value.Minutes);
                    this._time = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _shuffle_demand = 1;
        /// <summary>
        /// 当前活动所需人数，用于进行随机选取操作
        /// </summary>
        public int ShuffleDemand {
            get {
                return _shuffle_demand;
            }
            set {
                this._shuffle_demand = value;
                OnPropertyChanged();
            }
        }
    }
}
