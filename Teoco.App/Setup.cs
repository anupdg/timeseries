using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;
using Teoco.Common;
using Teoco.Interface;
using Teoco.Parser;

namespace Teoco.App
{
    internal class Setup
    {
        readonly string _configFile;
        public Setup(string configFile)
        {
            _configFile = configFile;
        }

        /// <summary>
        /// Configure dependencies
        /// </summary>
        /// <returns></returns>
        internal IServiceProvider Configure()
        {
            var services = ConfigureServices();

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }

        /// <summary>
        /// Configure services
        /// </summary>
        /// <returns></returns>
        private IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            //Load configuration from appsettings.json and add configuration as singleton service
            var config = LoadConfiguration();
            services.AddSingleton(config);

            //Create a serilog logging instance as singleton.  Logger type will change based on what is configured 
            //in appsettings.json => Serilog =>WriteTo
            //Currently it is configured to use console logger ("WriteTo": [ { "Name": "Console" } ])
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();
            services.AddSingleton<ILogger>(logger);

            services.AddSingleton<ITimeParser, TimeParser>();
            services.AddTransient<IProcessData, ProcessData>();
            services.AddTransient<ITimeFileReader, TimeFileReader>();
            services.AddTransient<ITimeFileWriter, TimeFileWriter>();
            services.AddTransient<ILineParser, LineParser>();

            return services;
        }

        /// <summary>
        /// Load configuration from appsettings
        /// </summary>
        /// <returns></returns>
        public IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(_configFile, optional: true, reloadOnChange: true);

            return builder.Build();
        }
    }
}
