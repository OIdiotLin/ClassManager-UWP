using ClassManager.Models;
using ClassManager.Networks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManager.ViewModels
{
    /// <summary>
    /// 反馈视图模型
    /// </summary>
    public class FeedbackViewModel: BaseViewModel
    {
        APIService api = new APIService();

        private Feedback _source_feedback;
        /// <summary>
        /// 反馈信息
        /// </summary>
        public Feedback SourceFeedback {
            get {
                return this._source_feedback;
            }
            set {
                this._source_feedback = value;
                OnPropertyChanged();
            }
        }
        

        /// <summary>
        /// 初始化各属性
        /// </summary>
        public void Initialize()
        {
            SourceFeedback = new Feedback();
        }
        

        /// <summary>
        /// 提交<see cref="SourceFeedback"/>
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SumbitFeedback()
        {
            return await api.AddFeedback(SourceFeedback);
        }
    }
}
