using System;

namespace Bluekiri.Consumer
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MessageTypeAttribute : Attribute
    {
        
        private readonly string _name;
        private readonly Type _modelType;

        public MessageTypeAttribute(string name, Type type)
        {
            
            this._name = name;
            this._modelType = type;

        }
        
        public virtual string Name
        {
            get { return _name; }
        }
        
        public virtual Type ModelType
        {
            get { return _modelType; }
        }
    }

    
}
