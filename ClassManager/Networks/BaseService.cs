﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;

using System.Security.Cryptography;

namespace ClassManager.Networks
{
    /// <summary>
    /// 访问 HTTP 服务器的基础服务
    /// </summary>
    public static class BaseService
    {
        /// <summary>
        /// 发送 GET 请求，并以 String 形式获取返回数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns>响应数据</returns>
        public async static Task<string> SendGetRequest(string url)
        {
            try
            {
                HttpClient client = new HttpClient();
                Uri uri = new Uri(url);

                HttpResponseMessage response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 发送 GET 请求，并以 Bytes 形式获取返回数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns>响应数据</returns>
        public async static Task<IBuffer> SendGetRequestAsBytes(string url)
        {
            try
            {
                HttpClient client = new HttpClient();
                Uri uri = new Uri(url);

                HttpResponseMessage response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsBufferAsync();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 发送 POST 请求，并以 String 形式获取返回数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data">请求表单</param>
        /// <returns>响应数据</returns>
        public async static Task<String> SendPostRequest(string url, string body)
        {
            try
            {
                Uri uri = new Uri(url);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
                request.Content = new HttpStringContent(
                    body, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json; charset=utf-8"
                );

                string token = "";
                string buffer = body + Sensitive.token_key;
                byte[] buffer_bytes = Encoding.UTF8.GetBytes(buffer);
                byte[] md5buffer = MD5.Create().ComputeHash(buffer_bytes);
                for (int i = 0; i < md5buffer.Length; i++)
                    token += md5buffer[i].ToString("x2");

                request.Headers.Add("token", token);

                HttpClient client = new HttpClient();

                HttpResponseMessage response = await client.SendRequestAsync(request);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch
            {
                return null;
            }
        }
    }
}
