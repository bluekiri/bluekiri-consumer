using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Bluekiri.Consumer.Tests")]
namespace Bluekiri.Consumer
{
    public class HandlerOptions
    {
        private readonly IDictionary<string, Type> _handlers;
        private readonly IDictionary<string, Type> _models;

        public HandlerOptions()
        {
            _handlers = new Dictionary<string, Type>();
            _models = new Dictionary<string, Type>();
        }
        internal void AddHandler(string key, Type handlerType, Type modelType)
        {
            _handlers.Add(key, handlerType);
            _models.Add(key, modelType);
        }

        internal Type GetHandler(string key)
        {
            var handler = _handlers[key];
            if (handler is null) throw new InvalidOperationException($"There is not a valid handler for {key}");
            return handler;
        }
        internal Type GetModel(string key)
        {
            return _models[key];
        }
    }
}
