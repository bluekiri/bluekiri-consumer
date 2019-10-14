using Bluekiri.Consumer;
using Bluekiri.Consumer.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Sample.Application
{
    public class Program
    {
      public static async Task Main(string[] args)
        {
            IHost host = new HostBuilder()
                 .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseEnvironment(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
                       .ConfigureAppConfiguration((hostContext, config) =>
                       {
                           config.AddEnvironmentVariables();
                           config.AddJsonFile("appsettings.json", optional: true);
                           config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                           config.AddCommandLine(args);
                       })
                       .ConfigureServices((hostContext, services) =>
                       {
                           services.AddOptions();
                           services.AddLogging();
                           services.AddSingleton(typeof(SampleHandler));
                           services.AddConsumerConfiguration<KafkaConsumer, KafkaConsumerOptions>(o =>
                           {
                               o.Topics.Add("test-topic");
                               o.SetProperty("bootstrap.servers", "lgmadanydkfk02v.corp.logitravelgroup.com:9092");
                               o.SetProperty("group.id", "test_1");
                               o.SetProperty("enable.auto.commit", "false");
                               
                           });
                       }).ConfigureLogging(c =>
                       {
                           c.AddConsole();
                           c.AddDebug();
                       })
                       .UseConsoleLifetime()
                       .Build();
            using (host)
            {
                await host.StartAsync();
                
                await host.WaitForShutdownAsync();
            }
        }
    }
}
