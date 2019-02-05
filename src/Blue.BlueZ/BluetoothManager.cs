using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blue.BlueZ.DBus;
using Blue.Common;
using Blue.Common.Gatt;
using Tmds.DBus;

namespace Blue.BlueZ
{
    public class BluetoothManager : IBluetoothManager
    {
        private readonly Connection _systemConnection;
        private readonly IObjectManager _objectManager;

        public BluetoothManager() : this(Connection.System)
        {
        }

        internal BluetoothManager(Connection connection)
        {
            _systemConnection = connection;
            _objectManager = _systemConnection.CreateProxy<IObjectManager>(Services.Base, ObjectPath.Root);
        }

        public async Task<List<IBluetoothDevice>> ListDevicesAsync()
        {
            var objects = await GetObjectsWithInterface(Interfaces.Device1).ConfigureAwait(false);
            return objects.Select(x => BluetoothDevice.Create(_systemConnection, x.Item1, x.Item2)).ToList<IBluetoothDevice>();
        }

        public async Task<List<IBluetoothAdapter>> ListAdaptersAsync()
        {
            var objects = await GetObjectsWithInterface(Interfaces.Adapter1).ConfigureAwait(false);
            return objects.Select(x => BluetoothAdapter.Create(_systemConnection, x.Item1, x.Item2)).ToList<IBluetoothAdapter>();
        }

        public Task<List<IBluetoothGattService>> ListServicesAsync()
        {
            throw new NotImplementedException();
        }

        private async Task<(ObjectPath, IDictionary<string, object>)[]> GetObjectsWithInterface(string @interface)
        {
            var objects = await _objectManager.GetManagedObjectsAsync().ConfigureAwait(false);
            return objects
                .Where(x => x.Value.ContainsKey(@interface))
                .Select(x => (x.Key, x.Value[@interface]))
                .ToArray();
        }

        private async Task<(ObjectPath, Dictionary<string, IDictionary<string, object>>)[]> GetObjectsWithInterfaces(
            params string[] interfaces)
        {
            var objects = await _objectManager.GetManagedObjectsAsync().ConfigureAwait(false);
            return objects
                .Where(x => interfaces.Any(i => x.Value.ContainsKey(i)))
                .Select(x => (x.Key, interfaces.ToDictionary(i => i, i => x.Value[i])))
                .ToArray();
        }

        private async Task<(ObjectPath, Dictionary<string, IDictionary<string, object>>)[]> GetObjectsWithAllInterfaces(params string[] interfaces)
        {
            var objects = await _objectManager.GetManagedObjectsAsync().ConfigureAwait(false);
            return objects
                .Where(x => interfaces.All(i => x.Value.ContainsKey(i)))
                .Select(x => (x.Key, interfaces.ToDictionary(i => i, i => x.Value[i])))
                .ToArray();
        }
    }
}