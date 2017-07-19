using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManager.Models
{
    /// <summary>
    /// 活动类
    /// </summary>
    public class Activity
    {
        public string Model { get; set; }       // Django 对应 Model
        public int PrimaryKey { get; set; }     // 数据库主键
        public Fields Info { get; set; }        // 详细信息

        public class Fields
        {
            public string Name { get; set; }            // 活动名
            public string Date { get; set; }            // 开始日期
            public string Time { get; set; }            // 开始时间
            public string Place { get; set; }           // 活动地点
            public string Content { get; set; }         // 活动内容
            public int Participation { get; set; }      // 参与得分
            public string Participator { get; set; }    // 参与人员（学号以半角逗号分隔）
            public string ImagesUrl { get; set; }       // 相关图片url（以半角逗号分隔）
        }
    }
}
