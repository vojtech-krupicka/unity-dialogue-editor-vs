using System;

namespace Hassa.Essentials
{
    public partial class EnsureThat
    {

        public void IsTypeOf<T>(object param) => IsTypeOf(param, typeof(T));
        public void IsTypeOf<T>(Type type) => IsTypeOf(type, typeof(T));


        public void IsTypeOf<T>(T param, Type expectedType)
        {
            IsTypeOf(typeof(T), expectedType);
        }

        public void IsTypeOf(Type actualType, Type expectedType)
        {
            if (!expectedType.IsAssignableFrom(actualType)) {
                throw new ArgumentException("Expected a '{expectedType}' but got '{actualType}'.", paramName);
            }
        }

    }
}