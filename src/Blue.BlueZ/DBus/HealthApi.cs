using System.Collections.Generic;
using System.Threading.Tasks;
using Tmds.DBus;

namespace Blue.BlueZ.DBus
{
    [DBusInterface(Interfaces.HealthManager1)]
    internal interface IHealthManager1 : IDBusObject
    {
        Task<ObjectPath> CreateApplicationAsync(IDictionary<string, object> Config);
        Task DestroyApplicationAsync(ObjectPath Application);
    }
}