using ClassManager.Models;
using ClassManager.Networks;
using ClassManager.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ClassManager.ViewModels
{
    public class ActivityEditingViewModel : BaseViewModel
    {
        APIService api = new APIService();

        public async Task Initialize(Activity activity = null)
        {
            ChosenParticipators = new ObservableCollection<Person>();
            Persons = new ObservableCollection<Person>((await api.GetPersonList()).OrderBy(x => (x.Pinyin)));
            UploadingImageFiles = new ObservableCollection<UploadingImageFile>();
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
        /// 1.将待选人员列表按参与度升序排列，总数为L
        /// 2.从最中间的位置k开始向两侧随机查询
        /// ，进入左侧的概率为(Sum[k+1,L-1]^3)/(Sum[0,k]^3+Sum[k+1,L]^3)
        /// ，进入右侧的概率为(Sum[0,k]^3)/(Sum[0,k]^3+Sum[k+1,L]^3)
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Person> RandomSelectItems()
        {
            //Random rnd = new Random();
            //int[] prefixSum = new int[Persons.Count + 1];
            //List<Person> ascendingPersonList = Persons.OrderBy(p => p.Participation).ToList();
            //var pickedPersons = new ObservableCollection<Person>();

            //if (ShuffleDemand > Persons.Count)
            //    ShuffleDemand = Persons.Count;

            //for (int pickedCount = 0; pickedCount < ShuffleDemand; pickedCount++)
            //{
            //    /// 前缀和处理
            //    /// prefixSum[i]表示ascendingPersonList[0]到ascendingPersonList[i-1]的累加和
            //    prefixSum[0] = 0;
            //    for (int i = 1; i <= ascendingPersonList.Count; i++)
            //    {
            //        prefixSum[i] = prefixSum[i - 1] + ascendingPersonList[i - 1].Participation;
            //    }

            //    int l = 1, r = ascendingPersonList.Count;
            //    while (l != r)
            //    {
            //        int m = (l + r) >> 1;
            //        var poweredLeft = Math.Pow(prefixSum[m] - prefixSum[l], 3);
            //        var poweredRight = Math.Pow(prefixSum[r] - prefixSum[m + 1], 3);
            //        var probLeft = poweredRight / (poweredRight + poweredLeft);

            //        if (rnd.NextDouble() <= probLeft)
            //        {
            //            r = m;
            //        }
            //        else
            //        {
            //            l = m+1;
            //        }
            //    }

            //    pickedPersons.Add(ascendingPersonList[l - 1]);
            //}

            //return pickedPersons;
            return new ObservableCollection<Person>(Rand.RandomItems(Persons.ToList(), ShuffleDemand));
        }

        /// <summary>
        /// 将<paramref name="uploadingImageFile"/>从<see cref="UploadingImageFiles"/>中去除
        /// </summary>
        /// <param name="uploadingImageFile"></param>
        public void RemoveFile(UploadingImageFile uploadingImageFile)
        {
            if(uploadingImageFile == null)
            {
                return;
            }
            UploadingImageFiles.Remove(uploadingImageFile);
        }

        /// <summary>
        /// 将选择的文件添加到<see cref="UploadingImageFiles"/>
        /// </summary>
        /// <param name="readOnlyList"></param>
        public void AddFiles(IReadOnlyList<StorageFile> readOnlyList)
        {
            if (readOnlyList == null)
            {
                return;
            }
            foreach (var file in readOnlyList)
            {
                UploadingImageFiles.Add(new UploadingImageFile(file));
            }
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

        private ObservableCollection<UploadingImageFile> _uploading_image_files;
        /// <summary>
        /// 待上传的相关图片文件
        /// </summary>
        public ObservableCollection<UploadingImageFile> UploadingImageFiles {
            get {
                return _uploading_image_files;
            }
            set {
                this._uploading_image_files = value;
                OnPropertyChanged();
            }
        }
    }
}
