using System;

namespace Bluekiri.Consumer.Abstractions
{
    /// <summary>
    /// Attribute used for detect handlers
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MessageTypeAttribute : Attribute
    {

        private readonly string _name;
        private readonly Type _modelType;
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Identifier name</param>
        /// <param name="type">Model type</param>
        public MessageTypeAttribute(string name, Type type)
        {

            this._name = name;
            this._modelType = type;

        }
        /// <summary>
        /// Returns name.
        /// </summary>
        public virtual string Name
        {
            get { return _name; }
        }
        /// <summary>
        /// Returns model type.
        /// </summary>
        public virtual Type ModelType
        {
            get { return _modelType; }
        }
    }
}