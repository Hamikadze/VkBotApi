using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace VkBotApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LogCore.LogWriteStart();
            FileCore.CreateHidFolder();
            ThreadCore.AccountSetOnlineStart();

            ThreadCore.CheckingMessagesThread_Start();
            ThreadCore.CommandExecutionThread_Start();
            MailCore.Read();
            MailCore.InitializeInboxes();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}