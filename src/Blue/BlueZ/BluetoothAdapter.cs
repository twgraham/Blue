using System;
using System.Threading.Tasks;
using Blue.BlueZ.DBus;
using Tmds.DBus;

namespace Blue.BlueZ
{
    public class BluetoothAdapter : IBluetoothAdapter
    {
        private readonly IAdapter1 _adapter1;
        private readonly Connection _connection;

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
            throw new NotImplementedException();
        }
    }
}