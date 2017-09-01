using ClassManager.Models;
using ClassManager.Networks;
using ClassManager.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace ClassManager.ViewModels
{
    public class LoginViewModel: BaseViewModel
    {
        APIService api = new APIService();

        /// <summary>
        /// 是否登录成功
        /// </summary>
        private bool _is_login_success;
        public bool IsLoginSuccess {
            get {
                return _is_login_success;
            }
            set {
                _is_login_success = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 以管理员身份登录
        /// </summary>
        /// <param name="password">密码</param>
        public async Task Login(string password)
        {
            IsLoginSuccess = await api.Login(password, Rand.RandomString());
        }

        public async Task<UpdateInfo> CheckUpdate()
        {
            UpdateInfo latest = await api.GetLatestGetLatestReleaseInfo();

            if (latest == null)
            {
                return new UpdateInfo();
            }

            latest.NeedUpdate = true;
            if (latest.Version.Major < Package.Current.Id.Version.Major)
            {
                latest.NeedUpdate = false;
            }
            else if (latest.Version.Minor < Package.Current.Id.Version.Minor)
            {
                latest.NeedUpdate = false;
            }
            else if (latest.Version.Build < Package.Current.Id.Version.Build)
            {
                latest.NeedUpdate = false;
            }

            if (latest.IsPrerelease)
            {
                latest.NeedUpdate = false;
            }
            return latest;
        }
    }
}
