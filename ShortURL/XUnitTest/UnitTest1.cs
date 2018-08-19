using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShortURL;
using ShortURL.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace XUnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //var list = new List<string>();
            //var list2 = new List<long>();
            //var a = 1;
            //var b = a + 100000;
            //for (int i = a; i < b; i++)
            //{
            //    list.Add(GenerateShortURL.ConfusionConvert(i));
            //}

            //foreach (var item in list)
            //{
            //    list2.Add(GenerateShortURL.ConfusionConvert(item));

            //}
            //var obj1 = string.Join(",", list);
            //var obj2 = string.Join(",", list2);
            //Assert.True(list2.Count() == list2.Distinct().Count());
        }

        [Fact]
        public async System.Threading.Tasks.Task test2Async()
        {
            IServiceCollection services = new ServiceCollection();
            //services.AddLogging();
            //services.AddSingleton<IMemcachedClient, NullMemcachedClient>();
            //services.AddScoped<UCenterService>(); 
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfiguration>();

            var home = new HomeController(configuration);

            try
            {
               // var o = await home.Generate("12312");
            }
            catch (Exception es)
            {
                throw;
            }
        } 
    }
}
