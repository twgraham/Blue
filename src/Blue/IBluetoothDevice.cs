using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blue
{
    public interface IBluetoothDevice
    {
        string Address { get; }
        string Name { get; }
        string Alias { get; set; }
        bool Paired { get; }
        bool Trusted { get; set; }
        bool Blocked { get; set; }
        bool LegacyPairing { get; }
        short RSSI { get; }
        bool Connected { get; }
        string[] UUIDs { get; }
        IDictionary<ushort, object> ManufacturerData { get; }
        IDictionary<string, object> ServiceData { get; }
        short TxPower { get; }
        bool ServicesResolved { get; }

        Task Connect();
        Task Disconnect();
        Task ConnectProfile(string uuid);
        Task DisconnectProfile(string uuid);
        Task Pair(CancellationToken cancellationToken = default);
    }
}