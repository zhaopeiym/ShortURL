using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Dapper;
using System.Threading.Tasks;
using System;
using Dapper.Contrib.Extensions;

namespace ShortURL.Controllers
{
    /// <summary>
    /// 转短码：
    /// 1、根据自增主键id前面补0，如：00000123
    /// 2、倒转32100000
    /// 3、把倒转后的十进制转六十二进制（乱序后）
    /// 解析短码：
    /// 1、六十二进制转十进制，得到如：32100000
    /// 2、倒转00000123，得到123
    /// 3、根据123作为主键去数据库查询映射对象
    /// </summary>
    public class HomeController : Controller
    {
        private IConfiguration configuration;
        public HomeController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<IActionResult> Index(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return View();

            var seqKey = configuration.GetValue<string>("SeqKye");//获取62个符号和数字（乱序后的）
            GenerateShortURL generate = new GenerateShortURL(seqKey);
            var id = generate.ConfusionConvert(key);
            var dbConnection = new MySqlConnection(configuration.GetValue<string>("MySQLSPConnection"));
            //根据id查询对应的映射关系实体
            var entity = await dbConnection.QueryFirstOrDefaultAsync<UrlRecordPO>("SELECT * from ShortURLs where Id = @id;", new { id });
            entity.LastModificationTime = DateTime.Now;
            entity.AccessNumber++;
            await dbConnection.UpdateAsync(entity);//更新点击数和最后修改时间
            return Redirect(entity.Url ?? "http://www.haojima.net");
        }
    }
}