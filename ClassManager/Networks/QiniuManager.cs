using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Qiniu.Util;
using Qiniu.Http;
using Qiniu.IO;
using Windows.Storage;

namespace ClassManager.Networks
{
    public sealed class QiniuManager
    {
        private static QiniuManager instance = null;
        private static object padLock = new Object();

        private static APIService api;
        private static UploadManager um;

        private QiniuManager() { }

        public static void Initialize()
        {
            api = new APIService();
            Qiniu.Common.Config.SetZone(Qiniu.Common.ZoneID.CN_South, false);
            um = new UploadManager();
        }

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
        /// 上传文件到七牛云
        /// </summary>
        /// <param name="file">本地文件</param>
        /// <param name="qiniuFilename">上传后的key</param>
        /// <returns>上传结果</returns>
        public async Task<bool> UploadFileAsync(StorageFile file, string qiniuFilename)
        {
            var token = await api.GetUploadToken(qiniuFilename);
            HttpResult result = await um.UploadFileAsync(file, qiniuFilename, token);
            return result.Code == 200;
        }
        
    }
}
