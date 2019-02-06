using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Blue.Common;
using Blue.Common.Gatt;

namespace Blue.Win32
{
    public class BluetoothManager : IBluetoothManager
    {
        public Task<List<IBluetoothDevice>> ListDevicesAsync()
        {
            var list = new List<IBluetoothDevice>();

            var searchParams = new BluetoothApis.BLUETOOTH_DEVICE_SEARCH_PARAMS
            {
                dwSize = Marshal.SizeOf<BluetoothApis.BLUETOOTH_DEVICE_SEARCH_PARAMS>(),
                cTimeoutMultiplier = 4,
                fIssueInquiry = true,
                fReturnAuthenticated = true
            };

            var info = new BluetoothApis.BLUETOOTH_DEVICE_INFO
            {
                dwSize = Marshal.SizeOf<BluetoothApis.BLUETOOTH_DEVICE_INFO>()
            };
            var searchHandle = BluetoothApis.BluetoothFindFirstDevice(ref searchParams, ref info);

            if (searchHandle != IntPtr.Zero)
            {
                do
                {
                    list.Add(new BluetoothDevice(info));
                }
                while (BluetoothApis.BluetoothFindNextDevice(searchHandle, ref info));


                BluetoothApis.BluetoothFindDeviceClose(searchHandle);
            }

            return Task.FromResult(list);
        }

        public Task<List<IBluetoothAdapter>> ListAdaptersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<IBluetoothGattService>> ListServicesAsync()
        {
            throw new NotImplementedException();
        }

        public static BluetoothManager GetDefault()
        {
            throw new NotImplementedException();
        }
    }
}