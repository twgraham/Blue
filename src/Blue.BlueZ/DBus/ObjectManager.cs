using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Tmds.DBus;

[assembly: InternalsVisibleTo(Connection.DynamicAssemblyName)]
namespace Blue.BlueZ.DBus
{
    [DBusInterface("org.freedesktop.DBus.ObjectManager")]
    internal interface IObjectManager : IDBusObject
    {
        Task<IDictionary<ObjectPath, IDictionary<string, IDictionary<string, object>>>> GetManagedObjectsAsync();

        Task<IDisposable> WatchInterfacesAddedAsync(
            Action<(ObjectPath @object, IDictionary<string, IDictionary<string, object>> interfaces)> handler,
            Action<Exception> onError = null);

        Task<IDisposable> WatchInterfacesRemovedAsync(Action<(ObjectPath @object, string[] interfaces)> handler,
            Action<Exception> onError = null);
    }
}