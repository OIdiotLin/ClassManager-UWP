using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace ClassManager.Networks
{
    public class APIBaseService
    {
        /// <summary>
        /// 发送 GET 请求，返回 JsonObject
        /// </summary>
        /// <param name="url">目标 url</param>
        /// <returns>解析后的 JsonObject</returns>
        protected async Task<JsonObject> GetJsonByGet(string url)
        {
            try
            {
                string json = await BaseService.SendGetRequest(url);
                return json != null ? JsonObject.Parse(json) : null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 发送 POST 请求，返回 JsonObject
        /// </summary>
        /// <param name="url">目标 url</param>
        /// <param name="body">请求表单</param>
        /// <returns>解析后的 JsonObject</returns>
        protected async Task<JsonObject> GetJsonByPost(string url, string body)
        {
            try
            {
                string json = await BaseService.SendPostRequest(url, body);
                return json != null ? JsonObject.Parse(json) : null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 发送 GET 请求，返回 WriteableBitmap
        /// </summary>
        /// <param name="url">目标 url</param>
        /// <returns>位图</returns>
        protected async Task<WriteableBitmap> GetImageByGet(string url)
        {
            try
            {
                IBuffer buffer = await BaseService.SendGetRequestAsBytes(url);
                if (buffer != null)
                {
                    BitmapImage img = new BitmapImage();
                    WriteableBitmap wb = null;
                    Stream os = null;

                    using(InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
                    {
                        os = stream.AsStreamForWrite();

                        await os.WriteAsync(buffer.ToArray(), 0, (int)buffer.Length);
                        await os.FlushAsync();
                        stream.Seek(0);

                        await img.SetSourceAsync(stream);
                        wb = new WriteableBitmap(img.PixelWidth, img.PixelHeight);
                        stream.Seek(0);

                        await wb.SetSourceAsync(stream);

                        return wb;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
        
    }
}
