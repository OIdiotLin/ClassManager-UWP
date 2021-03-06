﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassManager.Utils
{
    public static class Rand
    {
        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="length">字符串长度</param>
        /// <param name="useNum">字符集是否包含数字</param>
        /// <param name="useLow">字符集是否包含小写字母</param>
        /// <param name="useUpp">字符集是否包含大写字母</param>
        /// <param name="useSpe">字符集是否包含特殊字符</param>
        /// <param name="custom">自定义字符集</param>
        /// <returns>随机字符串</returns>
        public static string RandomString(int length = 16, 
                                          bool useNum = true,
                                          bool useLow = true,
                                          bool useUpp = true,
                                          bool useSpe = true,
                                          string custom = "")
        {

            string result = string.Empty;
            string charSet = custom;

            Random rand = new Random();

            if (useNum == true) { charSet += "0123456789"; }
            if (useLow == true) { charSet += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { charSet += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { charSet += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }

            for (int i = 0; i < length; i++)
            {
                result += charSet.Substring(rand.Next(0, charSet.Length - 1), 1);
            }
            return result;
        }
        
        /// <summary>
        /// 从<paramref name="list"/>中随机选取<paramref name="count"/>个项，构成新的<see cref="List"/>
        /// </summary>
        /// <param name="list">源list</param>
        /// <param name="count">待选个数</param>
        /// <returns></returns>
        public static IEnumerable<T> RandomItems<T>(IEnumerable<T> list, int count)
        {
            if(count > list.Count())
            {
                count = list.Count();
            }

            var rnd = new Random();
            list = list.OrderBy(x => (rnd.Next())) as IEnumerable<T>;

            return list.Take(count);
        }
    }
}
