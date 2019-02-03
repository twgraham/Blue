using System.Collections.Generic;

namespace Blue.Infrastructure
{
    public static class DictionaryExtensions
    {
        public static T ToObject<T>(this IDictionary<string, object> source) where T : class, new()
        {
            var destination = new T();
            var destinationType = destination.GetType();

            foreach (var item in source)
            {
                destinationType
                    .GetProperty(item.Key)
                    ?.SetValue(destination, item.Value, null);
            }

            return destination;
        }
    }
}