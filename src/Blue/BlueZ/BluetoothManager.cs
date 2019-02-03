using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blue.BlueZ.DBus;
using Blue.Infrastructure;
using Tmds.DBus;

namespace Blue.BlueZ
{
    internal class BluetoothManager : IBluetoothManager
    {
        private readonly Connection _systemConnection;
        private readonly IObjectManager _objectManager;

        public BluetoothManager() : this(Connection.System)
        {
        }

        public BluetoothManager(Connection connection)
        {
            _systemConnection = connection;
            _objectManager = _systemConnection.CreateProxy<IObjectManager>(BlueZServices.Base, "/");
        }

        public async Task<List<IBluetoothDevice>> ListDevicesAsync()
        {
            var objects = await GetObjectsWithInterface(BlueZInterfaces.Device1);
            return objects.Select(x => CreateDeviceProxy(x.Item1, x.Item2)).ToList<IBluetoothDevice>();
        }

        public async Task<List<IBluetoothAdapter>> ListAdaptersAsync()
        {
            var objects = await GetObjectsWithInterface(BlueZInterfaces.Adapter1);
            return objects.Select(x => CreateAdapterProxy(x.Item1, x.Item2)).ToList<IBluetoothAdapter>();
        }

        private BluetoothDevice CreateDeviceProxy(ObjectPath path, IDictionary<string, object> props)
        {
            return new BluetoothDevice(_systemConnection.CreateProxy<IDevice1>(BlueZServices.Base, path),
                props.ToObject<Device1Properties>());
        }

        private BluetoothAdapter CreateAdapterProxy(ObjectPath path, IDictionary<string, object> props)
        {
            return new BluetoothAdapter(_systemConnection.CreateProxy<IAdapter1>(BlueZServices.Base, path),
                props.ToObject<Adapter1Properties>());
        }

        private async Task<(ObjectPath, IDictionary<string, object>)[]> GetObjectsWithInterface(string @interface)
        {
            var objects = await _objectManager.GetManagedObjectsAsync().ConfigureAwait(false);
            return objects
                .Where(x => x.Value.ContainsKey(@interface))
                .Select(x => (x.Key, x.Value[@interface]))
                .ToArray();
        }

        private async Task<(ObjectPath, Dictionary<string, IDictionary<string, object>>)[]> GetObjectsWithInterfaces(string[] interfaces)
        {
            var objects = await _objectManager.GetManagedObjectsAsync().ConfigureAwait(false);
            return objects
                .Where(x => interfaces.All(i => x.Value.ContainsKey(i)))
                .Select(x => (x.Key, interfaces.ToDictionary(i => i, i => x.Value[i])))
                .ToArray();
        }
    }
}