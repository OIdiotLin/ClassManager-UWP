using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ClassManager.Controls
{
    public sealed partial class ImageFileItemTemplate : UserControl
    {
        public Models.UploadingImageFile UploadingImageFile {
            get {
                return this.DataContext as Models.UploadingImageFile;
            }
            set {
                this.DataContext = value;
            }
        }

        
        /// <summary>
        /// 根据上传状态更新Icons
        /// </summary>
        private void IconRefresh()
        {
            if (UploadingImageFile == null)
            {
                return;
            }

            switch (UploadingImageFile.State)
            {
                case Models.UploadingImageFile.UploadState.ReadyForUpload:  // 准备上传
                    UploadFailIcon.Visibility = Visibility.Collapsed;
                    UploadSuccessIcon.Visibility = Visibility.Collapsed;
                    UploadingProgressRing.IsActive = false;
                    break;
                case Models.UploadingImageFile.UploadState.Uploading:       // 上传中
                    UploadFailIcon.Visibility = Visibility.Collapsed;
                    UploadSuccessIcon.Visibility = Visibility.Collapsed;
                    UploadingProgressRing.IsActive = true;
                    break;
                case Models.UploadingImageFile.UploadState.UploadSuccess:   // 上传成功
                    UploadFailIcon.Visibility = Visibility.Collapsed;
                    UploadSuccessIcon.Visibility = Visibility.Visible;
                    UploadingProgressRing.IsActive = false;
                    break;
                case Models.UploadingImageFile.UploadState.UploadFail:     // 上传失败
                    UploadFailIcon.Visibility = Visibility.Visible;
                    UploadSuccessIcon.Visibility = Visibility.Collapsed;
                    UploadingProgressRing.IsActive = false;
                    break;
                default:
                    break;
            }
        }

        public ImageFileItemTemplate()
        {
            this.InitializeComponent();

            this.DataContextChanged += (s, e) => Bindings.Update();

            this.DataContextChanged += (s, e) => IconRefresh();
        }
    }
}
