using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Blue
{
    public interface IBluetoothManager
    {
        Task<List<IBluetoothDevice>> ListDevicesAsync();
    }

    public class BluetoothManager
    {
        private static IBluetoothManager _bluetoothManager;

        public static IBluetoothManager GetBluetoothManager()
        {
            if (_bluetoothManager != null)
                return _bluetoothManager;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                _bluetoothManager = new BlueZ.BluetoothManager();
            }
            else
            {
                throw new InvalidOperationException("Currently, only linux platforms with BlueZ are supported.");
            }

            return _bluetoothManager;
        }
    }
}