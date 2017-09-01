using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace ClassManager.Models
{
    /// <summary>
    /// 版本号
    /// </summary>
    public class AppVersion
    {
        public AppVersion(string v)
        {
            /// <example>v1.3.2-alpha => (1,3,2)</example>
            Regex reg = new Regex(@"\d+\.\d+\.\d+");
            var res = reg.Match(v).Groups[0].ToString().Split('.');
            Major = Int16.Parse(res[0]);
            Minor = Int16.Parse(res[1]);
            Build = Int16.Parse(res[2]);
        }

        public override string ToString()
        {
            return String.Format("v{0}.{1}.{2}", Major, Minor, Build);
        }

        public AppVersion(PackageVersion v)
        {
            Major = v.Major;
            Minor = v.Minor;
            Build = v.Build;
        }

        public int Major { get; set; }
        public int Minor { get; set; }
        public int Build { get; set; }
    }

    /// <summary>
    /// 版本更新信息
    /// </summary>
    public class UpdateInfo
    {
        /// <summary>
        /// 新版本号
        /// </summary>
        public AppVersion Version { get; set; }
        /// <summary>
        /// 版本详情URL
        /// </summary>
        public string DetailsUrl { get; set; }
        /// <summary>
        /// 预发布版本
        /// </summary>
        public bool IsPrerelease { get; set; }
        /// <summary>
        /// 需要更新
        /// </summary>
        public bool NeedUpdate { get; set; }
    }
}
