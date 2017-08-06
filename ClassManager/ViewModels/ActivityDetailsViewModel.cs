using ClassManager.Models;
using ClassManager.Networks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManager.ViewModels
{
    public class ActivityDetailsViewModel: BaseViewModel
    {
        APIService api = new APIService();

        /// <summary>
        /// 该页面作为展示的<see cref="Activity"/>
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
        /// 提交POST表单，删除<see cref="ActivityOnDisplay"/>
        /// </summary>
        /// <returns>删除结果</returns>
        public async Task<bool> DeleteActivityAsync()
        {
            return await api.DeleteActivity(ActivityOnDisplay);
        }

        /// <summary>
        /// 提交POST表单，添加<see cref="ActivityOnDisplay"/>
        /// </summary>
        /// <returns>添加结果</returns>
        public async Task<bool> AddActivityAsync()
        {
            return await api.AddActivity(ActivityOnDisplay);
        }

        /// <summary>
        /// 提交POST表单，修改<see cref="ActivityOnDisplay"/>
        /// </summary>
        /// <returns>修改结果</returns>
        public async Task<bool> UpdateActivityAsync()
        {
            return await api.UpdateActivity(ActivityOnDisplay.Id, ActivityOnDisplay);
        }
    }
}
