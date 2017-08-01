using ClassManager.Networks;
using ClassManager.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
