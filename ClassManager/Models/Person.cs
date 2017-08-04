using ClassManager.Networks;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace ClassManager.Models
{
    /// <summary>
    /// 学生类
    /// </summary>
    public class Person
    {
        public int Id { get; set; }                     // 数据库主键
        public string StudentNumber { get; set; }       // 学号
        public string Name { get; set; }                // 姓名
        public string Pinyin { get; set; }              // 拼音
        public string Gender { get; set; }              // 性别
        public string NativeProvince { get; set; }      // 籍贯
        public string Dormitory { get; set; }           // 寝室号
        public string Birthday { get; set; }            // 生日（公历）
        public string PhoneNumber { get; set; }         // 手机号
        public string Position { get; set; }            // 职务
        public int Participation { get; set; }          // 活动参与分

        /// <summary>
        /// 姓氏首字
        /// </summary>
        public string LastName {
            get {
                return Pinyin[0].ToString();
            }
        }

        public JObject ToJObject()
        {
            try
            {
                JObject json = new JObject
                {
                    { APIKey.Person.Id, this.Id },
                    { APIKey.Person.Name, this.Name },
                    { APIKey.Person.StudentNumber,this.StudentNumber },
                    { APIKey.Person.Pinyin, this.Pinyin },
                    { APIKey.Person.Gender, this.Gender == "男" ? "M" : "F"},
                    { APIKey.Person.NativeProvince, this.NativeProvince },
                    { APIKey.Person.Dormitory, this.Dormitory },
                    { APIKey.Person.PhoneNumber, this.PhoneNumber },
                    { APIKey.Person.Birthday, this.Birthday },
                    { APIKey.Person.Position, this.Position },
                    { APIKey.Person.Participation, this.Participation }
                };

                return json;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 解析JObject
        /// </summary>
        /// <param name="obj"></param>
        public Person(JsonObject obj)
        {
            Id = (int)obj[APIKey.Person.Id].GetNumber();
            StudentNumber = obj[APIKey.Person.StudentNumber].GetString();
            Name = obj[APIKey.Person.Name].GetString();
            Pinyin = obj[APIKey.Person.Pinyin].GetString();
            Gender = obj[APIKey.Person.Gender].GetString() == "M" ? "男" : "女";
            NativeProvince = obj[APIKey.Person.NativeProvince].GetString();
            Birthday = obj[APIKey.Person.Birthday].GetString();
            Dormitory = obj[APIKey.Person.Dormitory].GetString();
            Participation = (int)obj[APIKey.Person.Participation].GetNumber();
            PhoneNumber = obj[APIKey.Person.PhoneNumber].GetString();
            Position = obj[APIKey.Person.Position].GetString();
        }

        /// <summary>
        /// 缺省构造函数
        /// </summary>
        /// <param name="_Id"></param>
        /// <param name="_StudentNumber"></param>
        /// <param name="_Name"></param>
        /// <param name="_Pinyin"></param>
        /// <param name="_Gender"></param>
        /// <param name="_NativeProvince"></param>
        /// <param name="_Birthday"></param>
        /// <param name="_Dormitory"></param>
        /// <param name="_Participation"></param>
        /// <param name="_PhoneNumber"></param>
        /// <param name="_Position"></param>
        public Person(int _Id = -1,
                      string _StudentNumber = "",
                      string _Name = "",
                      string _Pinyin = "",
                      string _Gender = "男",
                      string _NativeProvince = "",
                      string _Birthday = "2000-01-01",
                      string _Dormitory = "",
                      int _Participation = 0,
                      string _PhoneNumber = "",
                      string _Position = ""
            )
        {
            Id = _Id;
            StudentNumber = _StudentNumber;
            Name = _Name;
            Pinyin = _Pinyin;
            Gender = _Gender;
            NativeProvince = _NativeProvince;
            Birthday = _Birthday;
            Dormitory = _Dormitory;
            Participation = _Participation;
            PhoneNumber = _PhoneNumber;
            Position = _Position;
        }
    }

    /// <summary>
    /// 学生分组
    /// </summary>
    public class PersonGroup
    {
        public PersonGroup(string name)
        {
            this.LastName = name;
            this.Items = new ObservableCollection<Person>();
        }

        public string LastName { get; private set; }
        public ObservableCollection<Person> Items { get; private set; }
        
    }
}
