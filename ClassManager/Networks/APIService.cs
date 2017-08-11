using ClassManager.Models;
using ClassManager.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace ClassManager.Networks
{
    /// <summary>
    /// API
    /// </summary>
    public class APIService : APIBaseService
    {
        private string localPath = Windows.Storage.ApplicationData.Current.LocalFolder.Path;

        /// <summary>
        /// 以管理员身份登录
        /// </summary>
        /// <param name="pswd">密码</param>
        /// <param name="rand">随机补位码</param>
        /// <returns>登录是否成功</returns>
        public async Task<bool> Login(string pswd, string rand)
        {
            try
            {
                string url = APIUrl.Permission.Login;
                JObject obj = new JObject
                {
                    { APIKey.Permission.Password, pswd },
                    { APIKey.Permission.RandCode, rand }
                };

                JsonObject json = await GetJsonByPost(url, obj.ToString());

                if (json != null)
                {
                    if (json.ContainsKey(APIKey.Permission.Token))
                    {
                        App.token = json[APIKey.Permission.Token].GetString();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取满足过滤器的 Person 列表
        /// </summary>
        /// <param name="filters">过滤器键值对 Dictionary</param>
        /// <returns>Person 列表</returns>
        public async Task<List<Person>> GetPersonList(Dictionary<string,string> filters = null)
        {
            try
            {
                string url = APIUrl.Person.GetPersonList;
                if (filters != null)
                {
                    url += "?";
                    foreach (var item in filters)
                    {
                        url += item.Key + "=" + item.Value + "&";
                    }
                }

                JsonObject json = await GetJsonByGet(url);

                if (json != null)
                {
                    List<Person> list = new List<Person>();
                    if (json.ContainsKey(APIKey.Results))
                    {
                        var results = json[APIKey.Results];
                        {
                            if (results != null)
                            {
                                JsonArray ja = results.GetArray();
                                foreach (var item in ja)
                                {
                                    var obj = item.GetObject();
                                    list.Add(new Person(obj));
                                }
                                return list;
                            }
                            else
                            {
                                return null;
                            } // result == null
                        }
                    }
                    else
                    {
                        return null;
                    } // json doesn't contain key - 'result'
                }
                else
                {
                    return null;
                } // json == null
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 根据<paramref name="activity"/>，获取相关的参与者列表
        /// </summary>
        /// <param name="activity"></param>
        /// <returns><see cref="List{Person}"/></returns>
        public async Task<List<Person>> GetPersonByActivity(Activity activity)
        {
            try
            {
                string url = String.Format("{0}?id={1}", APIUrl.Person.GetPersonByActivity, activity.Id);

                JsonObject json = await GetJsonByGet(url);

                if (json != null)
                {
                    List<Person> list = new List<Person>();
                    if (json.ContainsKey(APIKey.Results))
                    {
                        var results = json[APIKey.Results];
                        {
                            if (results != null)
                            {
                                JsonArray ja = results.GetArray();
                                foreach (var item in ja)
                                {
                                    var obj = item.GetObject();
                                    list.Add(new Person(obj));
                                }
                                return list;
                            }
                            else
                            {
                                return null;
                            } // result == null
                        }
                    }
                    else
                    {
                        return null;
                    } // json doesn't contain key - 'result'
                }
                else
                {
                    return null;
                } // json == null
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 添加新学生
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public async Task<bool> AddPerson(Person person)
        {
            try
            {
                string url = APIUrl.Person.AddPerson;
                JObject obj = new JObject();
                obj.Add(APIKey.Person.Root, person.ToJObject());

                JsonObject json = await GetJsonByPost(url, obj.ToString());

                if (json != null)
                {
                    if (json.ContainsKey(APIKey.Status))
                    {
                        return json[APIKey.Status].GetString() == APIValue.Success;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除学生信息
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public async Task<bool> DeletePerson(Person person)
        {
            try
            {
                string url = APIUrl.Person.DeletePerson;
                JObject obj = new JObject();
                obj.Add(APIKey.Person.Root, person.ToJObject());

                JsonObject json = await GetJsonByPost(url, obj.ToString());

                if (json != null)
                {
                    if (json.ContainsKey(APIKey.Status))
                    {
                        return json[APIKey.Status].GetString() == APIValue.Success;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 修改学生信息
        /// </summary>
        /// <param name="studentNumber">欲修改的学生学号</param>
        /// <param name="person">修改后的信息</param>
        /// <returns></returns>
        public async Task<bool> UpdatePerson(string studentNumber, Person person)
        {
            try
            {
                string url = APIUrl.Person.UpdatePerson;

                JObject req = new JObject
                {
                    { APIKey.Person.Target, studentNumber },
                    { APIKey.Person.Root, person.ToJObject() }
                };
                string body = req.ToString();

                JsonObject json = await GetJsonByPost(url, body);

                if (json != null)
                {
                    if (json.ContainsKey(APIKey.Status))
                    {
                        return json[APIKey.Status].GetString() == APIValue.Success;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取满足过滤器的 Activity 列表
        /// </summary>
        /// <param name="filters">过滤器键值对 Dictionary</param>
        /// <returns>Activity 列表</returns>
        public async Task<List<Activity>> GetActivityList(Dictionary<string,string> filters = null)
        {
            try
            {
                string url = APIUrl.Activity.GetActivityList;
                if (filters != null)
                {
                    url += "?";
                    foreach (var item in filters)
                    {
                        url += item.Key + "=" + item.Value + "&";
                    }
                }

                JsonObject json = await GetJsonByGet(url);

                if (json != null)
                {
                    List<Activity> list = new List<Activity>();
                    if (json.ContainsKey(APIKey.Results))
                    {
                        var results = json[APIKey.Results];
                        {
                            if (results != null)
                            {
                                JsonArray ja = results.GetArray();
                                foreach (var item in ja)
                                {
                                    var obj = item.GetObject();
                                    list.Add(new Activity(obj));
                                }
                                return list;
                            }
                            else
                            {
                                return null;
                            } // result == null
                        }
                    }
                    else
                    {
                        return null;
                    } // json doesn't contain key - 'result'
                }
                else
                {
                    return null;
                } // json == null
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 添加新活动
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public async Task<bool> AddActivity(Activity activity)
        {
            try
            {
                string url = APIUrl.Activity.AddActivity;

                JObject obj = new JObject();
                obj.Add(APIKey.Activity.Root, activity.ToJObject());

                JsonObject json = await GetJsonByPost(url, obj.ToString());

                if (json != null)
                {
                    if (json.ContainsKey(APIKey.Status))
                    {
                        return json[APIKey.Status].GetString() == APIValue.Success;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除活动信息
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public async Task<bool> DeleteActivity(Activity activity)
        {
            try
            {
                string url = APIUrl.Activity.DeleteActivity;
                JObject obj = new JObject();
                obj.Add(APIKey.Activity.Root, activity.ToJObject());

                JsonObject json = await GetJsonByPost(url, obj.ToString());

                if (json != null)
                {
                    if (json.ContainsKey(APIKey.Status))
                    {
                        return json[APIKey.Status].GetString() == APIValue.Success;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 修改活动信息
        /// </summary>
        /// <param name="id">欲修改的活动的 id</param>
        /// <param name="activity">修改后的活动信息</param>
        /// <returns></returns>
        public async Task<bool> UpdateActivity(int id, Activity activity)
        {
            try
            {
                string url = APIUrl.Activity.UpdateActivity;

                JObject req = new JObject
                {
                    { APIKey.Activity.Target, id },
                    { APIKey.Activity.Root, activity.ToJObject() }
                };
                string body = req.ToString();

                JsonObject json = await GetJsonByPost(url, body);

                if (json != null)
                {
                    if (json.ContainsKey(APIKey.Status))
                    {
                        return json[APIKey.Status].GetString() == APIValue.Success;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取Qiniu上传凭证
        /// </summary>
        /// <param name="qiniuFileName">上传到qiniu后的文件名</param>
        /// <returns>上传凭证token </returns>
        public async Task<string> GetUploadToken(string qiniuFileName)
        {
            try
            {
                string url = APIUrl.Qiniu.GetUploadToken;

                JObject req = new JObject
                {
                    { APIKey.Qiniu.Filename, qiniuFileName }
                };
                string body = req.ToString();

                JsonObject json = await GetJsonByPost(url, body);

                if (json != null)
                {
                    if (json.ContainsKey(APIKey.Status))
                    {
                        if(json[APIKey.Status].GetString() == APIValue.Success)
                        {
                            return json[APIKey.Qiniu.UploadToken].GetString();
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
