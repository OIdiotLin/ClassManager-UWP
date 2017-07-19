using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManager.Networks
{
    /// <summary>
    /// Url of apis used
    /// </summary>
    public static class APIUrl
    {
        // Prefix - protocol and common path
        public static string prefix = "http://server.oidiotlin.com/clsmngr/api/";
        
        // GET - 获取学生列表
        public static string GetPersonList = prefix + "get_person_list/";

        // POST - 添加学生
        public static string AddPerson = prefix + "add_person/";

        // POST - 删除学生
        public static string DeletePerson = prefix + "delete_person/";

        // POST - 修改学生信息
        public static string UpdatePerson = prefix + "update_person/";

        // GET - 按活动编号查找学生
        public static string GetPersonByActivity = prefix + "get_person_by_activity/";

        // GET - 获取活动列表
        public static string GetActivityList = prefix + "get_activity_list/";

        // POST - 添加活动
        public static string AddActivity = prefix + "add_activity/";

        // POST - 删除活动
        public static string DeleteActivity = prefix + "delete_activity/";

        // POST - 修改活动信息
        public static string UpdateActivity = prefix + "update_activity/";

        // GET - 获取七牛云上传凭证
        public static string GetUploadToken = prefix + "get_upload_token/";

    }
}
