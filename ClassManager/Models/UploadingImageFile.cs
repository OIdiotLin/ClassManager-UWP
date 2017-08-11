using ClassManager.Networks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ClassManager.Models
{
    /// <summary>
    /// 需要上传的Image类型
    /// </summary>
    public sealed class UploadingImageFile : Object
    {
        /// <summary>
        /// 文件本体
        /// </summary>
        public StorageFile File { get; set; }

        /// <summary>
        /// 上传状态枚举类型
        /// </summary>
        public enum UploadState
        {
            ReadyForUpload,
            Uploading,
            UploadSuccess,
            UploadFails
        }

        /// <summary>
        /// 上传状态
        /// </summary>
        public UploadState State { get; set; }

        public UploadingImageFile(StorageFile file = null, int activityId = 0)
        {
            File = file;
            State = UploadState.ReadyForUpload;
            if (file != null)
            {
                QiniuFileUrl = String.Format("{0}{1}-{2}", APIUrl.Qiniu.StorageHost, activityId, file.Name);
            }
        }

        /// <summary>
        /// 在Qiniu上的文件名，添加了活动的id作为前缀
        /// </summary>
        public string QiniuFileUrl { get; set; }
    }
}
