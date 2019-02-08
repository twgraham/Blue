using System.Collections.Generic;
using Tmds.DBus;

namespace Blue.BlueZ.DBus
{
    internal static class ManagedObjectExtensions
    {
        public static bool HasInterface(this KeyValuePair<ObjectPath, IDictionary<string, IDictionary<string, object>>> rawManagedObject,
            string @interface)
        {
            return rawManagedObject.Value.ContainsKey(@interface);
        }
    }
}