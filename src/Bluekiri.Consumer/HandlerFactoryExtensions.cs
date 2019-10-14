using System;
using System.Collections.Generic;

namespace Bluekiri.Consumer
{
    public delegate object HandlerFactory(Type handlerType);
    public static class HandlerFactoryExtensions
    {
        public static IEnumerable<T> GetInstances<T>(this HandlerFactory factory) => (IEnumerable<T>)factory(typeof(IEnumerable<T>));
    }
}