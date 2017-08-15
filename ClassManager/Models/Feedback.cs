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
    /// 反馈信息类
    /// </summary>
    public class Feedback
    {
        /// <summary>
        /// 数据库主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 反馈总结
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 详细信息
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// 反馈分类
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 联系邮箱
        /// </summary>
        public string Contact { get; set; }

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
                    { APIKey.Feedback.Id, this.Id },
                    { APIKey.Feedback.Summary, this.Summary },
                    { APIKey.Feedback.Details, this.Details },
                    { APIKey.Feedback.Category, this.Category[0] },
                    { APIKey.Feedback.Contact, this.Contact }
                };

                return json;
            }
            catch
            {
                return null;
            }
        }

        public Feedback(JsonObject obj)
        {
            Id = (int)obj[APIKey.Feedback.Id].GetNumber();
            Summary = obj[APIKey.Feedback.Summary].GetString();
            Details = obj[APIKey.Feedback.Details].GetString();
            Category = obj[APIKey.Feedback.Category].GetString() == "A" ? "Advice" : "Bug";
            Contact = obj[APIKey.Feedback.Contact].GetString();
        }
        
        /// <summary>
        /// 缺省构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="summary"></param>
        /// <param name="details"></param>
        /// <param name="category"></param>
        /// <param name="contact"></param>
        public Feedback(int id = -1,
                        string summary = "",
                        string details = "",
                        string category = "Bug",
                        string contact = "")
        {
            Id = id;
            Summary = summary;
            Details = details;
            Category = category;
            Contact = contact;
        }
    }
}
