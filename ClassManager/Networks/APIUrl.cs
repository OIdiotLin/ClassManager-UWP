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
        /// Prefix - protocol and common path
        public static string ServerHost = "http://server.oidiotlin.com/clsmngr/api/";

        public static class Person
        {
            /// <summary>
            /// GET - 获取学生列表
            /// </summary>
            public static string GetPersonList = ServerHost + "get_person_list/";

            /// <summary>
            /// POST - 添加学生
            /// </summary>
            public static string AddPerson = ServerHost + "add_person/";

            /// <summary>
            /// POST - 删除学生
            /// </summary>
            public static string DeletePerson = ServerHost + "delete_person/";

            /// <summary>
            /// POST - 修改学生信息
            /// </summary>
            public static string UpdatePerson = ServerHost + "update_person/";

            /// <summary>
            /// GET - 按活动编号查找学生
            /// </summary>
            public static string GetPersonByActivity = ServerHost + "get_person_by_activity/";
        }

        public static class Activity
        {
            /// <summary>
            /// GET - 获取活动列表
            /// </summary>
            public static string GetActivityList = ServerHost + "get_activity_list/";

            /// <summary>
            /// POST - 添加活动
            /// </summary>
            public static string AddActivity = ServerHost + "add_activity/";

            /// <summary>
            /// POST - 删除活动
            /// </summary>
            public static string DeleteActivity = ServerHost + "delete_activity/";

            /// <summary>
            /// POST - 修改活动信息
            /// </summary>
            public static string UpdateActivity = ServerHost + "update_activity/";
        }

        public static class Qiniu
        {
            /// <summary>
            /// GET - 获取七牛云上传凭证
            /// </summary>
            public static string GetUploadToken = ServerHost + "get_upload_token/";

            public static string StorageHost = "http://oolxxgdth.bkt.clouddn.com/";
        }

        public static class Permission
        {
            /// GET - 登录认证
            public static string Login = ServerHost + "login/";
        }

    }
}
