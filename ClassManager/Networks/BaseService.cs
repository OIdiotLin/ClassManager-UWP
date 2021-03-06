﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;

using System.Security.Cryptography;
using ClassManager.Utils;
using Windows.Web.Http.Filters;

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
        /// <param name="cache">是否使用缓存</param>
        /// <returns>响应数据</returns>
        public async static Task<string> SendGetRequest(string url, bool cache=false)
        {
            try
            {
                HttpClient client;

                // 是否需要缓存
                if (cache == false)
                {
                    var filter = new HttpBaseProtocolFilter();

                    filter.CacheControl.ReadBehavior = HttpCacheReadBehavior.NoCache;
                    filter.CacheControl.WriteBehavior = HttpCacheWriteBehavior.NoCache;

                    client = new HttpClient(filter);
                }
                else
                {
                    client = new HttpClient();
                }

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

                string token = Crypto.MD5(body, App.token);

                request.Headers.Add("token", token);

                var filter = new HttpBaseProtocolFilter();

                filter.CacheControl.ReadBehavior = HttpCacheReadBehavior.NoCache;
                filter.CacheControl.WriteBehavior = HttpCacheWriteBehavior.NoCache;

                var client = new HttpClient(filter);

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
