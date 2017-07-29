using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManager.Utils
{
    public static class Crypto
    {
        /// <summary>
        /// 32位小写字母md5加密
        /// </summary>
        /// <param name="buffer">原串</param>
        /// <param name="extstr">附加串</param>
        /// <returns>md5</returns>
        public static string MD5(string buffer, string extstr = "")
        {
            if (extstr != "")
            {
                buffer += extstr;
            }

            string md5 = "";

            byte[] buffer_bytes = Encoding.UTF8.GetBytes(buffer);
            byte[] md5buffer = System.Security.Cryptography.MD5.Create().ComputeHash(buffer_bytes);

            for (int i = 0; i < md5buffer.Length; i++)
                md5 += md5buffer[i].ToString("x2");

            return md5;
        }
    }
}
