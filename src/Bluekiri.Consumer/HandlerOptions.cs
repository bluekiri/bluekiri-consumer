﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Bluekiri.Consumer.Tests")]
namespace Bluekiri.Consumer
{
    /// <summary>
    /// Handler options
    /// </summary>
    public class HandlerOptions
    {
        private readonly IDictionary<string, Type> _models;
        /// <summary>
        /// Constructor
        /// </summary>
        public HandlerOptions()
        {
            _models = new Dictionary<string, Type>();
        }

        internal void AddModel(string key, Type modelType)
        {
            //_handlers.Add(key, handlerType);
            _models.Add(key, modelType);
        }

        internal Type GetModel(string key)
        {
            return _models[key];
        }
    }
}
