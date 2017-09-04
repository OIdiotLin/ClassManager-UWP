using ClassManager.Models;
using ClassManager.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        /// 获取所有班费收支情况
        /// </summary>
        /// <returns></returns>
        public async Task<List<Finance>> GetFinanceList()
        {
            try
            {
                string url = APIUrl.Finance.GetFinanceList;

                JsonObject json = await GetJsonByGet(url);

                if (json != null)
                {
                    List<Finance> list = new List<Finance>();
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
                                    list.Add(new Finance(obj));
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
        /// 添加班费收支
        /// </summary>
        /// <param name="finance"></param>
        /// <returns></returns>
        public async Task<bool> AddFinance(Finance finance)
        {
            try
            {
                string url = APIUrl.Finance.AddFinance;

                JObject obj = new JObject();
                obj.Add(APIKey.Finance.Root, finance.ToJObject());

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
        /// 删除班费收支
        /// </summary>
        /// <param name="finance"></param>
        /// <returns></returns>
        public async Task<bool> DeleteFinance(Finance finance)
        {
            try
            {
                string url = APIUrl.Finance.DeleteFinance;
                JObject obj = new JObject();
                obj.Add(APIKey.Finance.Root, finance.ToJObject());

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
        /// 修改班费收支
        /// </summary>
        /// <param name="id">欲修改的目标id</param>
        /// <param name="finance">修改后的信息</param>
        /// <returns></returns>
        public async Task<bool> UpdateFinance(int id, Finance finance)
        {
            try
            {
                string url = APIUrl.Finance.UpdateFinance;

                JObject req = new JObject
                {
                    { APIKey.Finance.Target, id },
                    { APIKey.Finance.Root, finance.ToJObject() }
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
        /// 获取班费余额
        /// </summary>
        /// <returns></returns>
        public async Task<float> GetBalance()
        {
            try
            {
                string url = APIUrl.Finance.GetBalance;

                JsonObject json = await GetJsonByGet(url);

                if (json != null)
                {
                    if (json.ContainsKey(APIKey.Finance.Balance))
                    {
                        return (float)json[APIKey.Finance.Balance].GetNumber();
                    }
                    else
                    {
                        return 0f;
                    } // json doesn't contain key - 'result'
                }
                else
                {
                    return 0f;
                } // json == null
            }
            catch
            {
                return 0f;
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

        /// <summary>
        /// 提交反馈信息
        /// </summary>
        /// <param name="feedback"></param>
        /// <returns>提交结果</returns>
        public async Task<bool> AddFeedback(Feedback feedback)
        {
            try
            {
                string url = APIUrl.Feedback.AddFeedback;

                JObject obj = new JObject();
                obj.Add(APIKey.Feedback.Root, feedback.ToJObject());

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
        /// 获取最新应用版本
        /// </summary>
        /// <returns>版本信息</returns>
        public async Task<UpdateInfo> GetLatestGetLatestReleaseInfo()
        {
            try
            {
                var github = new Octokit.GitHubClient(new Octokit.ProductHeaderValue("ClassManager-UWP"));
                var latest = await github.Repository.Release.GetLatest("OIdiotLin", "ClassManager-UWP");

                Regex reg = new Regex(@"\*\w+");
                //var res = reg.Match(latest.Body).Groups[0].ToString();
                //var summary = res != "" ? res.Substring(0, res.Length - 5) : "";

                var summary = "";
                var res = reg.Matches(latest.Body);

                if (res.Count != 0)
                {
                    foreach(var item in res)
                    {
                        summary += item.ToString() + "\r\n";
                    }
                }

                var info = new UpdateInfo()
                {
                    Version = new AppVersion(latest.Name),
                    DetailsUrl = latest.HtmlUrl,
                    IsPrerelease = latest.Prerelease,
                    Summary = summary

                };
                return info;
            }
            catch
            {
                return null;
            }
        }
    }
}
