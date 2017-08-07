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
    /// 活动类
    /// </summary>
    public class Activity : Object
    {
        public int Id { get; set; }                 // 数据库主键
        public string Name { get; set; }            // 活动名
        public string Date { get; set; }            // 开始日期
        public string Time { get; set; }            // 开始时间
        public string Place { get; set; }           // 活动地点
        public string Content { get; set; }         // 活动内容
        public int Participation { get; set; }      // 参与得分
        public string Participator { get; set; }    // 参与人员（学号以半角逗号分隔）
        public string ImagesUrl { get; set; }       // 相关图片url（以半角逗号分隔）

        /// <summary>
        /// 将<see cref="ImagesUrl"/>解析后的stringsList
        /// </summary>
        public ObservableCollection<string> ImgUrls {
            get {
                if (ImagesUrl == null)
                    return null;
                return new ObservableCollection<string>(ImagesUrl.Split(",".ToArray()));
                //return ImagesUrl.Split(",".ToArray()).ToList();
            }
        }

        /// <summary>
        /// 缩略图（若<see cref="ImagesUrl"/>非空则返回首张图片，否则返回默认图片）
        /// </summary>
        public string Thumbnail {
            get {
                return ImagesUrl == "" ? "/Assets/Pictures/SCUT.png" : ImgUrls[0] + "?" + APIKey.Qiniu.ThumbnailSuffix;
            }
        }

        /// <summary>
        /// 将<see cref="Participator"/>解析后的学号stringsList
        /// </summary>
        public List<string> Participators {
            get {
                return Participator.Split(",".ToArray()).ToList();
            }
        }


        public Activity(Activity source)
        {
            Id = source.Id;
            Name = source.Name;
            Date = source.Date;
            Time = source.Time;
            Place = source.Place;
            Content = source.Content;
            Participator = source.Participator;
            Participation = source.Participation;
            ImagesUrl = source.ImagesUrl;
        }

        public JObject ToJObject()
        {
            try
            {
                JObject json = new JObject
                {
                    { APIKey.Activity.Id, this.Id },
                    { APIKey.Activity.Name, this.Name },
                    { APIKey.Activity.Date,this.Date },
                    { APIKey.Activity.Time, this.Time },
                    { APIKey.Activity.Place, this.Place },
                    { APIKey.Activity.Content, this.Content },
                    { APIKey.Activity.Participation, this.Participation },
                    { APIKey.Activity.Participator, this.Participator },
                    { APIKey.Activity.Images, this.ImagesUrl }
                };

                return json;
            }
            catch
            {
                return null;
            }
        }

        public Activity(JsonObject obj)
        {
            Id = (int)obj[APIKey.Activity.Id].GetNumber();
            Name = obj[APIKey.Activity.Name].GetString();
            Date = obj[APIKey.Activity.Date].GetString();
            Time = obj[APIKey.Activity.Time].GetString();
            Place = obj[APIKey.Activity.Place].GetString();
            Content = obj[APIKey.Activity.Content].GetString();
            Participator = obj[APIKey.Activity.Participator].GetString();
            Participation = (int)obj[APIKey.Activity.Participation].GetNumber();
            ImagesUrl = obj[APIKey.Activity.Images].GetString();
        }

        public Activity()
        {
        }
    }

    
}
