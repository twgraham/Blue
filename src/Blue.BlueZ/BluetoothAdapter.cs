using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blue.BlueZ.DBus;
using Blue.Common;
using Blue.Infrastructure;
using Tmds.DBus;

namespace Blue.BlueZ
{
    public class BluetoothAdapter : IBluetoothAdapter, IBlueZObject
    {
        private readonly IAdapter1 _adapter1;

        public ObjectPath ObjectPath => _adapter1.ObjectPath;

        public string Address { get; }
        public string Name { get; }
        public string Alias { get; set; }
        public bool Discoverable { get; set; }
        public bool Discovering { get; }
        public string[] UUIDs { get; }

        internal BluetoothAdapter(IAdapter1 adapter1, Adapter1Properties properties)
        {
            _adapter1 = adapter1;
            Address = properties.Address;
            Name = properties.Name;
            Alias = properties.Alias;
            Discoverable = properties.Discoverable;
            Discovering = properties.Discovering;
            UUIDs = properties.UUIDs;
        }

        public Task StartDiscovery()
        {
            return _adapter1.StartDiscoveryAsync();
        }

        public Task StopDiscovery()
        {
            return _adapter1.StopDiscoveryAsync();
        }

        public Task RemoveDevice(IBluetoothDevice device)
        {
            if (device is IBlueZObject blueZObject)
            {
                return _adapter1.RemoveDeviceAsync(blueZObject.ObjectPath);
            }

            return Task.FromException(new InvalidOperationException("The bluetooth device belongs to a different manager."));
        }

        public static BluetoothAdapter Create(Connection connection, ObjectPath objectPath,
            IDictionary<string, object> properties)
        {
            return new BluetoothAdapter(connection.CreateProxy<IAdapter1>(Services.Base, objectPath),
                properties.ToObject<Adapter1Properties>());
        }
    }
}