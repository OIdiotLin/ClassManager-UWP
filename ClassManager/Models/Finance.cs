using ClassManager.Networks;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace ClassManager.Models
{
    /// <summary>
    /// 单笔班费收支
    /// </summary>
    public class Finance
    {
        /// <summary>
        /// 数据库主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 收支活动发生的日期
        /// </summary>
        public string Date { get; set; }
        
        /// <summary>
        /// 相关事件/活动
        /// </summary>
        public string Event { get; set; }

        /// <summary>
        /// 收入金额
        /// </summary>
        public float Income { get; set; }

        /// <summary>
        /// 支出金额
        /// </summary>
        public float Expense { get; set; }

        /// <summary>
        /// 详细信息
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// 生成<see cref="JObject"/>对象
        /// </summary>
        /// <returns></returns>
        public JObject ToJObject()
        {
            try
            {
                JObject json = new JObject
                {
                    { APIKey.Finance.Id, this.Id },
                    { APIKey.Finance.Date, this.Date },
                    { APIKey.Finance.Event, this.Event },
                    { APIKey.Finance.Income, this.Income },
                    { APIKey.Finance.Expense, this.Expense },
                    { APIKey.Finance.Details, this.Details }
                };
                return json;
            }
            catch
            {
                return null;
            }
        }

        public Finance(JsonObject obj)
        {
            Id = (int)obj[APIKey.Finance.Id].GetNumber();
            Date = obj[APIKey.Finance.Date].GetString();
            Event = obj[APIKey.Finance.Event].GetString();
            Income = (float)obj[APIKey.Finance.Income].GetNumber();
            Expense = (float)obj[APIKey.Finance.Expense].GetNumber();
            Details = obj[APIKey.Finance.Details].GetString();
        }

        public Finance(int _id = -1,
                       string _date = "2000-01-01",
                       string _event = "",
                       float _income = 0.0f,
                       float _expense = 0.0f,
                       string _details = "")
        {
            Id = _id;
            Date = _date;
            Event = _event;
            Income = _income;
            Expense = _expense;
            Details = _details;
        }
    }
}
