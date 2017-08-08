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
    public class ActivityDetailsViewModel: BaseViewModel
    {
        APIService api = new APIService();

        /// <summary>
        /// 该页面作为展示的<see cref="Activity"/>，在Admin模式下可进行删除操作
        /// </summary>
        private Activity _activity_on_display;
        public Activity ActivityOnDisplay {
            get {
                return this._activity_on_display;
            }
            set {
                this._activity_on_display = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// <see cref="ActivityOnDisplay"/>的参与者集合
        /// </summary>
        private ObservableCollection<Person> _participators;
        public ObservableCollection<Person> Participators {
            get {
                return _participators;
            }
            set {
                this._participators = value;
                OnPropertyChanged();
            }            
        }

        /// <summary>
        /// <see cref="Participators"/>的字符串表达
        /// </summary>
        /// <returns></returns>
        private string _participators_to_string = null;
        public string ParticipatorsToString {
            get {
                return _participators_to_string;
            }
            set {
                this._participators_to_string = value;
                OnPropertyChanged();
            }
        }
        
        public async void Initialize(Activity sourceActivity)
        {
            ActivityOnDisplay = sourceActivity;
            Participators = new ObservableCollection<Person>(await api.GetPersonByActivity(sourceActivity));
            ParticipatorsToString = String.Join("、", new ObservableCollection<string>(from p in Participators select p.Name));
        }

        /// <summary>
        /// 提交POST表单，删除<see cref="ActivityOnDisplay"/>
        /// </summary>
        /// <returns>删除结果</returns>
        public async Task<bool> DeleteActivityAsync()
        {
            return await api.DeleteActivity(ActivityOnDisplay);
        }
    }
}
