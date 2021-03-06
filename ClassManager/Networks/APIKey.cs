﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManager.Networks
{
    /// <summary>
    /// 与服务器交互的Json数据的节点键名
    /// </summary>
    public static class APIKey
    {
        public static string Count = "count";
        public static string Results = "result";

        public static string Status = "status";
        
        public static class Person
        {
            public static string Root = "person";
            public static string Target = "target_student_number";
            public static string Id = "id";
            public static string Name = "name";
            public static string StudentNumber = "student_number";
            public static string Pinyin = "pinyin";
            public static string Gender = "gender";
            public static string NativeProvince = "native_province";
            public static string Dormitory = "dormitory";
            public static string Birthday = "birthday";
            public static string PhoneNumber = "phone_number";
            public static string Position = "position";
            public static string Participation = "participation";
        }

        public static class Activity
        {
            public static string Root = "activity";
            public static string Target = "target_id";
            public static string Id = "id";
            public static string Name = "name";
            public static string Content = "content";
            public static string Date = "date";
            public static string Time = "time";
            public static string Place = "place";
            public static string Participator = "participator";
            public static string Participation = "participation";
            public static string Images = "images";
        }

        public static class Finance
        {
            public static string Root = "finance";
            public static string Target = "target_id";
            public static string Id = "id";
            public static string Date = "date";
            public static string Income = "income";
            public static string Expense = "expense";
            public static string Event = "event";
            public static string Details = "details";

            public static string Balance = "balance";
        }

        public static class Feedback
        {
            public static string Root = "feedback";
            public static string Id = "id";
            public static string Summary = "summary";
            public static string Details = "details";
            public static string Category = "category";
            public static string Contact = "contact";
        }

        public static class Permission
        {
            public static string RandCode = "rand_code";
            public static string Password = "password";
            public static string Token = "check_code";
        }

        public static class Qiniu
        {
            public static string ThumbnailSuffix = "imageView2/1/w/450/h/300/q/95|imageslim";
            public static string Filename = "filename";
            public static string UploadToken = "upload_token";
        }
    }

    public static class APIValue
    {
        public static string Success = "success";
        public static string Fail = "fail";
    }
}
