using System;
using System.Collections.Generic;

namespace Bluekiri.Consumer
{
    /// <summary>
    /// Delegate handler
    /// </summary>
    /// <param name="handlerType"><see cref="Type"/></param>
    /// <returns></returns>
    public delegate object HandlerFactory(Type handlerType);
    /// <summary>
    /// Handler Factory Extension
    /// </summary>
    /// 
    public static class HandlerFactoryExtensions
    {
        /// <summary>
        /// Return instances.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetInstances<T>(this HandlerFactory factory) => (IEnumerable<T>)factory(typeof(IEnumerable<T>));
    }
}