using Bluekiri.Consumer.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bluekiri.Consumer
{
    /// <summary>
    /// Extensions for IOC
    /// </summary>
    public static class ServiceCollectionExtensions
    {

        private static readonly Type _handlerInterfaceTemplate = typeof(IMessageHandler<>);
        /// <summary>
        /// Extension for confirgure the consumer on IoC.
        /// </summary>
        /// <typeparam name="TConsumer"><see cref="IBrokerConsumer"/></typeparam>
        /// <typeparam name="TConsumerOptions"><see cref="ConsumerOptions"/></typeparam>
        /// <param name="services"></param>
        /// <param name="setup"><see cref="ConsumerOptions"/></param>
        /// <returns><see cref="IServiceCollection"/></returns>
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

            // adds default formatter (Json).
            consumeroptions.AddMessageFormatter<JsonMessageFormatter>();

            foreach (var formatter in consumeroptions.MessageFormatters)
            {
                services.AddSingleton(typeof(IMessageFormatter), formatter);
            }

            services.AddSingleton<IBrokerConsumer, TConsumer>();
            services.AddHostedService<ConsumerService<TConsumerOptions>>();
            services.AddSingleton<IHandlerManager, HandlerManager>();
            services.AddSingleton<IHandlerMessageFactory, HandlerMessageFactory>();
            services.AddTransient<HandlerFactory>(p => p.GetService);

            return services;
        }

        private static void AddHandlers(this IServiceCollection services)
        {
            var listHandlersType = new List<Type>();
            var interfaces = new List<Type>();

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

            services.Configure<HandlerOptions>(o =>
            {
                foreach (var type in types)
                {
                    o.AddModel(type.Key, type.Value.ModelType);
                }
            });
            foreach (var type in types)
            {
                listHandlersType.Add(type.Value.HandlerType);

                foreach (var interfaceType in type.Value.HandlerType.GetGenericInterfacesForMessageHandler())
                {
                    interfaces.Add(interfaceType);
                }
            }

            foreach (var @interface in interfaces)
            {
                var exactMatches = listHandlersType.Where(x => x.CanBeCastTo(@interface)).ToList();
                foreach (var type in exactMatches)
                {
                    services.AddSingleton(@interface, type);
                }
            }
        }


        private static IEnumerable<Type> GetGenericInterfacesForMessageHandler(this Type type)
        {
            foreach (var interfaceType in type.GetInterfaces()
                .Where(t => t.GetTypeInfo().IsGenericType && (t.GetGenericTypeDefinition() == _handlerInterfaceTemplate)))
            {
                yield return interfaceType;
            }
        }

        private static bool CanBeCastTo(this Type expectedType, Type requestType)
        {
            if (expectedType is null) return false;

            if (expectedType == requestType) return true;

            return requestType.GetTypeInfo().IsAssignableFrom(expectedType.GetTypeInfo());
        }
    }
}
