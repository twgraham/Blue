using System.Collections.Generic;
using System.Threading.Tasks;
using Blue.Common;
using Blue.Common.Gatt;

namespace Blue
{
    public interface IBluetoothManager
    {
        Task<List<IBluetoothDevice>> ListDevicesAsync();
        Task<List<IBluetoothAdapter>> ListAdaptersAsync();
        Task<List<IBluetoothGattService>> ListServicesAsync();
    }
}