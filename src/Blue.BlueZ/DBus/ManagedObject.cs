using System.Collections.Generic;
using Tmds.DBus;

namespace Blue.BlueZ.DBus
{
    public class ManagedObject
    {
        private readonly Connection _connection;
        public ObjectPath ObjectPath { get; }
        public IDictionary<string, IDictionary<string, object>> InterfaceProperties { get; }

        public ManagedObject(Connection connection, ObjectPath objectPath, IDictionary<string, IDictionary<string, object>> interfaceProperties)
        {
            _connection = connection;
            ObjectPath = objectPath;
            InterfaceProperties = interfaceProperties;
        }

        public bool HasInterface(string @interface)
        {
            return InterfaceProperties.ContainsKey(@interface);
        }

        public T GetPropertiesForInterface<T>(string @interface) where T : class, new()
        {
            var rawProps = InterfaceProperties[@interface];
            return RawPropsAsType<T>(rawProps);
        }

        public T CreateProxy<T>(string service)
            where T : IDBusObject
        {
            return _connection.CreateProxy<T>(service, ObjectPath);
        }

        private static T RawPropsAsType<T>(IDictionary<string, object> source) where T : class, new()
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