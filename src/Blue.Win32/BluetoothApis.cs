using System;
using System.Runtime.InteropServices;

namespace Blue.Win32
{
    internal static class BluetoothApis
    {
        private const string BthDll = "bthprops.cpl";

        [DllImport(BthDll, SetLastError = true)]
        internal static extern IntPtr BluetoothFindFirstRadio(ref BLUETOOTH_FIND_RADIO_PARAMS pbtfrp, out IntPtr phRadio);

        [StructLayout(LayoutKind.Sequential)]
        internal struct BLUETOOTH_FIND_RADIO_PARAMS
        {
            public int dwSize;
        }

        [DllImport(BthDll, SetLastError = true)]
        internal static extern IntPtr BluetoothFindFirstDevice(ref BLUETOOTH_DEVICE_SEARCH_PARAMS pbtsp, ref BLUETOOTH_DEVICE_INFO pbtdi);

        [StructLayout(LayoutKind.Sequential)]
        internal struct BLUETOOTH_DEVICE_SEARCH_PARAMS
        {
            public int dwSize;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fReturnAuthenticated;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fReturnRemembered;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fReturnUnknown;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fReturnConnected;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fIssueInquiry;
            public byte cTimeoutMultiplier;
            IntPtr hRadio;
        }

        [DllImport(BthDll, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothFindNextDevice(IntPtr hFind, ref BLUETOOTH_DEVICE_INFO pbtdi);

        [DllImport(BthDll, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothFindDeviceClose(IntPtr hFind);

        [DllImport(BthDll, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothSelectDevices(ref BLUETOOTH_SELECT_DEVICE_PARAMS pbtsdp);

        [DllImport(BthDll, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BluetoothSelectDevicesFree(ref BLUETOOTH_SELECT_DEVICE_PARAMS pbtsdp);

        [StructLayout(LayoutKind.Sequential, Size = 60)]
        internal struct BLUETOOTH_SELECT_DEVICE_PARAMS
        {
            internal int dwSize;
            internal int numOfClasses;
            internal IntPtr prgClassOfDevices;
            [MarshalAs(UnmanagedType.LPWStr)]
            internal string info;
            internal IntPtr hwndParent;
            [MarshalAs(UnmanagedType.Bool)]
            bool fForceAuthentication;
            [MarshalAs(UnmanagedType.Bool)]
            internal bool fShowAuthenticated;
            [MarshalAs(UnmanagedType.Bool)]
            bool fShowRemembered;
            [MarshalAs(UnmanagedType.Bool)]
            internal bool fShowUnknown;
            [MarshalAs(UnmanagedType.Bool)]
            bool fAddNewDeviceWizard;
            [MarshalAs(UnmanagedType.Bool)]
            bool fSkipServicesPage;
            [MarshalAs(UnmanagedType.FunctionPtr)]
            internal PFN_DEVICE_CALLBACK pfnDeviceCallback;
            internal IntPtr pvParam;
            internal uint numDevices;
            internal IntPtr /*PBLUETOOTH_DEVICE_INFO*/ pDevices;
        }

        internal delegate bool PFN_DEVICE_CALLBACK(IntPtr param, ref BLUETOOTH_DEVICE_INFO device);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct BLUETOOTH_DEVICE_INFO
        {
            public int dwSize;
            public ulong Address;
            public uint ulClassofDevice;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fConnected;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fRemembered;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fAuthenticated;
            public SYSTEMTIME stLastSeen;
            public SYSTEMTIME stLastUsed;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 248)]
            public string szName;
        }
    }
}