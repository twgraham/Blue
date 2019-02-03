using System.Collections.Generic;
using System.Threading.Tasks;
using Tmds.DBus;

namespace Blue.BlueZ.DBus
{
    [DBusInterface("org.bluez.ProfileManager1")]
    internal interface IProfileManager1 : IDBusObject
    {
        Task RegisterProfileAsync(ObjectPath profile, string uuid, IDictionary<string, object> options);
        Task UnregisterProfileAsync(ObjectPath profile);
    }
}