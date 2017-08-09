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
    public class ActivityEditingViewModel : BaseViewModel
    {
        APIService api = new APIService();

        public async void Initialize(Activity activity = null)
        {
            Persons = new ObservableCollection<Person>(await api.GetPersonList());
            SourceActivity = activity;

            string[] date = SourceActivity.Date.Split('-');
            Date = new DateTimeOffset(new DateTime(Int16.Parse(date[0]), Int16.Parse(date[1]), Int16.Parse(date[2])));


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
                if(SourceActivity != null)
                    SourceActivity.Date = String.Format("{0}-{1}-{2}", value.Year, value.Month, value.Day);
                this._date = value;
                OnPropertyChanged();
            }
        }

        private DateTimeOffset _time;
        /// <summary>
        /// 当前活动的时间
        /// </summary>
        public DateTimeOffset Time {
            get {
                return _time;
            }
            set {
                if (SourceActivity != null)
                    SourceActivity.Date = String.Format("{0}:{1}", value.Hour, value.Minute);
                this._date = value;
                OnPropertyChanged();
            }
        }
    }
}
