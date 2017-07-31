using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Qiniu.Util;

namespace ClassManager.Networks
{
    public sealed class QiniuManager
    {
        private static QiniuManager instance = null;
        private static object padLock = new Object();

        private QiniuManager() { }

        /// <summary>
        /// Use singleton method for Qiniu
        /// </summary>
        public static QiniuManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padLock)
                    {
                        if (instance == null)
                            instance = new QiniuManager();
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// Upload a local file to Qiniu Storage
        /// </summary>
        /// <param name="localFilename">filename of local source file</param>
        /// <param name="remoteFilename">filename of Qiniu target file</param>
        public void UploadFile(string localFilename, string remoteFilename)
        {
            string uploadToken = App.token;
        }
    }
}
