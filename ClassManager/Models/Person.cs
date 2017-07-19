using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManager.Models
{
    /// <summary>
    /// 学生类
    /// </summary>
    public class Person
    {
        public string Id { get; set; }                  // 数据库主键
        public string StudentNumber { get; set; }       // 学号
        public string Name { get; set; }                // 姓名
        public string Pinyin { get; set; }              // 拼音
        public string Gender { get; set; }              // 性别（F/M）
        public string NativeProvince { get; set; }      // 籍贯
        public string Dormitory { get; set; }           // 寝室号
        public string Birthday { get; set; }            // 生日（公历）
        public string PhoneNumber { get; set; }         // 手机号
        public string Position { get; set; }            // 职务
        public int Participation { get; set; }          // 活动参与分
    }
}
