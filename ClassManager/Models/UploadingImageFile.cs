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
        public enum UPLOADSTATE
        {
            ReadyForUpload,
            Uploading,
            UploadSuccess,
            UploadFails
        }

        /// <summary>
        /// 上传状态
        /// </summary>
        public UPLOADSTATE UploadState { get; set; }
    }
}
