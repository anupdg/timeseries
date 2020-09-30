using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;
using Teoco.Common;
using Teoco.Interface;
using Teoco.Parser;

namespace Teoco.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var setup = new Setup("appsettings.json");
            //Configure all the dependencies and service registration
            var provider = setup.Configure();

            var fileReader = provider.GetService<IProcessData>();

            var logger = provider.GetService<ILogger>();
            try
            {
                //Process file
                fileReader.Process(DateTime.Now).Wait();
            }
            catch (Exception ex)
            {
                CommonHelper.LogError(logger, ex, "There is some error. Please verify log for details");
            }

            Console.ReadKey();
        }
    }
}
