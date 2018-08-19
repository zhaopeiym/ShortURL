using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using Dapper.Contrib.Extensions;
using MySql.Data.MySqlClient;
using System;
using System.DrawingCore.Imaging;
using QRCoder;
using System.DrawingCore;

namespace ShortURL.Controllers
{
    [Route("api/[controller]/[Action]")]
    public class ValuesController : Controller
    {
        private static List<KeyValuePair<string, string>> UrlCache = new List<KeyValuePair<string, string>>();
        private static HttpClient httpClient = new HttpClient();

        private IConfiguration configuration;
        public ValuesController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// 解析生成短网址，并保存到数据库
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> Generate(string url)
        {
            #region 验证url的重复和有效性
            var urlInfo = UrlCache.Where(t => t.Key == url).FirstOrDefault();
            //如果存在直接返回
            if (!string.IsNullOrWhiteSpace(urlInfo.Key))
            {
                return new { ShortURL = urlInfo.Value };
            }
            //最多缓存50个
            if (UrlCache.Count() > 50)
                UrlCache.RemoveRange(0, UrlCache.Count() - 50);

            //Nginx变量设置 proxy_set_header x-host $scheme://$host;
            var host = Request.Headers["x-host"].FirstOrDefault();//Nginx 问题获取的是内网地址
            try
            {
                if ((!string.IsNullOrWhiteSpace(host) && url.Contains(host)))
                    return new { ShortURL = url };

                var statusCode = await httpClient.GetAsync(url);
                if (!url.Contains("http") ||
                    (int)statusCode.StatusCode >= 400)
                    return new { ShortURL = "", Msg = "请输入有效网址" };
            }
            catch (Exception ex)
            {
                return new { ShortURL = "", Msg = "请输入有效网址" };
            }
            #endregion

            var dbConnection = new MySqlConnection(configuration.GetValue<string>("MySQLSPConnection"));
            var entityPo = new UrlRecordPO();
            var id = await dbConnection.InsertAsync(entityPo);
            var seqKey = configuration.GetValue<string>("SeqKye");//获取62个符号和数字（乱序后的）
            GenerateShortURL generate = new GenerateShortURL(seqKey);
            var shortURL = generate.ConfusionConvert(id);
            entityPo.Id = id;
            entityPo.ShortURL = shortURL;
            entityPo.Url = url;
            await dbConnection.UpdateAsync(entityPo);
            UrlCache.Add(new KeyValuePair<string, string>(url, shortURL));
            return new { shortURL };
        }

        /// <summary>
        /// 获取二维码图片
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task Img(string url)
        {
            Response.ContentType = "image/jpeg";
            using (var bitmap = CodeHelper.GetQRCode(url, 10))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitmap.Save(ms, ImageFormat.Jpeg);
                    await Response.Body.WriteAsync(ms.GetBuffer(), 0, Convert.ToInt32(ms.Length));
                    Response.Body.Close();
                }
            }
        }
    }
}
