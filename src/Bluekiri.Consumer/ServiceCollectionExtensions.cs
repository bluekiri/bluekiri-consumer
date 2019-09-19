using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Reflection;

namespace Bluekiri.Consumer
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConsumerConfiguration<TConsumer, TConsumerOptions>(this IServiceCollection services,
                                                                                               Action<TConsumerOptions> setup)
            where TConsumer : class, IBrokerConsumer
            where TConsumerOptions : ConsumerOptions, new()
        {
            services.AddHandlers();
            services.Configure(setup);

            var consumeroptions = new TConsumerOptions();
            setup(consumeroptions);

            var options = Options.Create(consumeroptions);
            services.AddSingleton(options);

            foreach (var formatter in consumeroptions.MessageFormatters)
            {
                services.AddSingleton(typeof(IMessageFormatter), formatter);
            }

            services.AddSingleton<IBrokerConsumer, TConsumer>();

            services.AddHostedService<ConsumerService<TConsumerOptions>>();

            return services;
        }

        private static void AddHandlers(this IServiceCollection services)
        {
            services.Configure<HandlerOptions>(o =>
            {
                var types = Assembly.GetEntryAssembly().GetTypes()
                    .Where(t => t.GetCustomAttribute<MessageTypeAttribute>() != null)
                    .ToDictionary(t => t.GetCustomAttribute<MessageTypeAttribute>().Name,
                    t =>
                        new
                        {
                            HandlerType = t,
                            t.GetCustomAttribute<MessageTypeAttribute>().ModelType
                        }
                    );

                foreach (var type in types)
                {
                    o.AddHandler(type.Key, type.Value.HandlerType, type.Value.ModelType);
                }
            });

            services.AddSingleton<IHandlerManager, HandlerManager>();
        }
    }
}
