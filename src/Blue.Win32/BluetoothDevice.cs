using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blue.Common;

namespace Blue.Win32
{
    public class BluetoothDevice : IBluetoothDevice
    {
        private readonly BluetoothApis.BLUETOOTH_DEVICE_INFO _info;

        public string Address { get; }
        public string Name => _info.szName;
        public string Alias { get; set; }
        public bool Paired => _info.fAuthenticated;
        public bool Trusted { get; set; }
        public bool Blocked { get; set; }
        public bool LegacyPairing { get; }
        public short RSSI { get; }
        public bool Connected => _info.fConnected;
        public string[] UUIDs { get; }
        public IDictionary<ushort, object> ManufacturerData { get; }
        public IDictionary<string, object> ServiceData { get; }
        public short TxPower { get; }
        public bool ServicesResolved { get; }

        internal BluetoothDevice(BluetoothApis.BLUETOOTH_DEVICE_INFO info)
        {

        }

        public Task Connect()
        {
            throw new System.NotImplementedException();
        }

        public Task Disconnect()
        {
            throw new System.NotImplementedException();
        }

        public Task ConnectProfile(string uuid)
        {
            throw new System.NotImplementedException();
        }

        public Task DisconnectProfile(string uuid)
        {
            throw new System.NotImplementedException();
        }

        public Task Pair(CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}