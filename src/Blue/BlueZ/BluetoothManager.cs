using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blue.BlueZ.DBus;
using Tmds.DBus;

namespace Blue.BlueZ
{
    internal class BluetoothManager : IBluetoothManager
    {
        private readonly Connection _systemConnection;
        private readonly IObjectManager _objectManager;

        public BluetoothManager()
        {
            _systemConnection = Connection.System;
            _objectManager = _systemConnection.CreateProxy<IObjectManager>("org.bluez", "/");
        }

        public async Task<List<IBluetoothDevice>> ListDevicesAsync()
        {
            var objects = await _objectManager.GetManagedObjectsAsync().ConfigureAwait(false);

            var devices = new List<IBluetoothDevice>();
            foreach (var key in objects.Where(x => x.Value.ContainsKey("org.bluez.Device1")).ToArray())
            {
                var device = _systemConnection.CreateProxy<IDevice1>("org.bluez", key.Key);
                var deviceProps = await device.GetAllAsync();
                devices.Add(new BluetoothDevice(device, deviceProps));
            }

            return devices;
        }
    }
}