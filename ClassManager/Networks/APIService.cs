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
    /// api service. cast application/json to models
    /// </summary>
    public class APIService : APIBaseService
    {
        private string localPath = Windows.Storage.ApplicationData.Current.LocalFolder.Path;

        /// <summary>
        /// 以管理员身份登录
        /// </summary>
        /// <param name="pswd"></param>
        /// <param name="rand"></param>
        /// <returns>校验码 </returns>
        public async Task<string> Login(string pswd, string rand)
        {
            try
            {
                string url = APIUrl.Permission.Login;
                JObject obj = new JObject
                {
                    { APIKey.Permission.Password, pswd },
                    { APIKey.Permission.RandCode, rand }
                };
                string body = Crypto.MD5(obj.ToString(), Sensitive.token_key);

                JsonObject json = await GetJsonByPost(url, body);

                if (json != null)
                {
                    if (json.ContainsKey(APIKey.Permission.CheckCode))
                    {
                        return json[APIKey.Permission.CheckCode].GetString();
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
        /// 添加新学生
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public async Task<bool> AddPerson(Person person)
        {
            try
            {
                string url = APIUrl.Person.AddPerson;
                string body = person.ToJObject().ToString();
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
        /// 删除学生信息
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public async Task<bool> DeletePerson(Person person)
        {
            try
            {
                string url = APIUrl.Person.DeletePerson;
                string body = person.ToJObject().ToString();
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
                string body = activity.ToJObject().ToString();
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
        /// 删除活动信息
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public async Task<bool> DeleteActivity(Activity activity)
        {
            try
            {
                string url = APIUrl.Activity.DeleteActivity;
                string body = activity.ToJObject().ToString();
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
    }
}
