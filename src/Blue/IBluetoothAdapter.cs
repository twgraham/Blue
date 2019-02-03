using System.Threading.Tasks;

namespace Blue
{
    public interface IBluetoothAdapter
    {
        string Address { get; }
        string Name { get; }
        string Alias { get; set; }
        bool Discoverable { get; set; }
        bool Discovering { get; }
        string[] UUIDs { get; }

        Task StartDiscovery();
        Task StopDiscovery();
        Task RemoveDevice(IBluetoothDevice device);
    }
}