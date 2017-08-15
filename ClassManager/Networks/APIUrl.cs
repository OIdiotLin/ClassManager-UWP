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
        public static string ServerHostApiPrefix = "http://server.oidiotlin.com/clsmngr/api/";

        public static class Person
        {
            /// <summary>
            /// GET - 获取学生列表
            /// </summary>
            public static string GetPersonList = ServerHostApiPrefix + "get_person_list/";

            /// <summary>
            /// POST - 添加学生
            /// </summary>
            public static string AddPerson = ServerHostApiPrefix + "add_person/";

            /// <summary>
            /// POST - 删除学生
            /// </summary>
            public static string DeletePerson = ServerHostApiPrefix + "delete_person/";

            /// <summary>
            /// POST - 修改学生信息
            /// </summary>
            public static string UpdatePerson = ServerHostApiPrefix + "update_person/";

            /// <summary>
            /// GET - 按活动编号查找学生
            /// </summary>
            public static string GetPersonByActivity = ServerHostApiPrefix + "get_person_by_activity/";
        }

        public static class Activity
        {
            /// <summary>
            /// GET - 获取活动列表
            /// </summary>
            public static string GetActivityList = ServerHostApiPrefix + "get_activity_list/";

            /// <summary>
            /// POST - 添加活动
            /// </summary>
            public static string AddActivity = ServerHostApiPrefix + "add_activity/";

            /// <summary>
            /// POST - 删除活动
            /// </summary>
            public static string DeleteActivity = ServerHostApiPrefix + "delete_activity/";

            /// <summary>
            /// POST - 修改活动信息
            /// </summary>
            public static string UpdateActivity = ServerHostApiPrefix + "update_activity/";
        }

        public static class Finance
        {
            /// <summary>
            /// GET - 获取班费余额
            /// </summary>
            public static string GetBalance = ServerHostApiPrefix + "get_balance/";

            /// <summary>
            /// POST - 提交财务活动（收入/支出）
            /// </summary>
            public static string AddFinance = ServerHostApiPrefix + "add_finance/";
        }

        public static class Qiniu
        {
            /// <summary>
            /// GET - 获取七牛云上传凭证
            /// </summary>
            public static string GetUploadToken = ServerHostApiPrefix + "get_upload_token/";

            public static string StorageHost = "http://oolxxgdth.bkt.clouddn.com/";
        }

        public static class Permission
        {
            /// GET - 登录认证
            public static string Login = ServerHostApiPrefix + "login/";
        }

        public static class Feedback
        {
            /// <summary>
            /// POST - 提交反馈信息（建议/Bug）
            /// </summary>
            public static string AddFeedback = ServerHostApiPrefix + "add_feedback/";
        }
        
    }
}
