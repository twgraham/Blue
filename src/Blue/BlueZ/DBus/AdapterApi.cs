using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tmds.DBus;

namespace Blue.BlueZ.DBus
{
    [DBusInterface("org.bluez.Adapter1")]
    internal interface IAdapter1 : IDBusObject
    {
        /// <summary>
        /// This method starts the device discovery session. This
        /// includes an inquiry procedure and remote device name
        /// resolving. Use StopDiscovery to release the sessions
        /// acquired.
        ///
        /// This process will start creating Device objects as
        /// new devices are discovered.
        ///
        /// During discovery RSSI delta-threshold is imposed.
        ///
        /// Possible errors:
        ///     org.bluez.Error.NotReady
        ///     org.bluez.Error.Failed
        /// </summary>
        Task StartDiscoveryAsync();

        /// <summary>
        /// This method sets the device discovery filter for the
        /// caller. When this method is called with no filter
        /// parameter, filter is removed.
        /// </summary>
        /// <returns></returns>
        Task SetDiscoveryFilterAsync(DiscoveryFilter filter);

        /// <summary>
        /// Return available filters that can be given to
        /// SetDiscoveryFilter.
        ///
        /// Possible errors: None
        /// </summary>
        /// <returns>Discovery filter</returns>
        Task<DiscoveryFilter> GetDiscoveryFiltersAsync();

        /// <summary>
        /// This method will cancel any previous StartDiscovery
        /// transaction.
        ///
        /// Note that a discovery procedure is shared between all
        /// discovery sessions thus calling StopDiscovery will only
        /// release a single session.
        ///
        /// Possible errors:
        ///     org.bluez.Error.NotReady
        ///     org.bluez.Error.Failed
        ///     org.bluez.Error.NotAuthorized
        /// </summary>
        Task StopDiscoveryAsync();

        /// <summary>
        /// This removes the remote device object at the given
        /// path. It will remove also the pairing information.
        ///
        /// Possible errors:
        ///     org.bluez.Error.InvalidArguments
        ///     org.bluez.Error.Failed
        /// </summary>
        /// <param name="device"></param>
        Task RemoveDeviceAsync(IDevice1 device);

        Task<T> GetAsync<T>(string prop);
        Task<Adapter1Properties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }

    [Dictionary]
    internal class DiscoveryFilter
    {
        /// <summary>
        /// Filter by service UUIDs, empty means match
        /// _any_ UUID.
        ///
        /// When a remote device is found that advertises
        /// any UUID from UUIDs, it will be reported if:
        /// - Pathloss and RSSI are both empty.
        /// - only Pathloss param is set, device advertise
        ///   TX pwer, and computed pathloss is less than
        ///   Pathloss param.
        /// - only RSSI param is set, and received RSSI is
        ///   higher than RSSI param.
        /// </summary>
        public string[] UUIDs { get; set; } = default;

        /// <summary>
        /// RSSI threshold value.
        ///
        /// PropertiesChanged signals will be emitted
        /// for already existing Device objects, with
        /// updated RSSI value. If one or more discovery
        /// filters have been set, the RSSI delta-threshold,
        /// that is imposed by StartDiscovery by default,
        /// will not be applied.
        /// </summary>
        public short RSSI { get; set; } = default;

        /// <summary>
        /// Pathloss threshold value.
        ///
        /// PropertiesChanged signals will be emitted
        /// for already existing Device objects, with
        /// updated Pathloss value.
        /// </summary>
        public ushort Pathloss { get; set; } = default;

        /// <summary>
        ///
        /// Transport parameter determines the type of
        /// scan.
        ///
        /// Possible values:
        ///     "auto"	- interleaved scan
        ///     "bredr"	- BR/EDR inquiry
        ///     "le"	- LE scan only
        ///
        /// If "le" or "bredr" Transport is requested,
        /// and the controller doesn't support it,
        /// org.bluez.Error.Failed error will be returned.
        /// If "auto" transport is requested, scan will use
        /// LE, BREDR, or both, depending on what's
        /// currently enabled on the controller.
        /// </summary>
        public string Transport { get; set; } = "auto";

        /// <summary>
        /// Disables duplicate detection of advertisement
        /// data.
        ///
        /// When enabled PropertiesChanged signals will be
        /// generated for either ManufacturerData and
        /// ServiceData everytime they are discovered.
        /// </summary>
        public bool DuplicateData { get; set; } = true;

        /// <summary>
        /// Make adapter discoverable while discovering,
        /// if the adapter is already discoverable setting
        /// this filter won't do anything.
        /// </summary>
        public bool Discoverable { get; set; } = false;
    }

    [Dictionary]
    internal class Adapter1Properties
    {
        /// <summary>
        /// The Bluetooth device address.
        /// </summary>
        public string Address { get; set; } = default;

        /// <summary>
        /// The Bluetooth system name (pretty hostname).
        /// This property is either a static system default
        /// or controlled by an external daemon providing
        /// access to the pretty hostname configuration.
        /// </summary>
        public string Name { get; set; } = default;

        /// <summary>
        /// The Bluetooth friendly name. This value can be
        /// changed.
        ///
        /// In case no alias is set, it will return the system
        /// provided name. Setting an empty string as alias will
        /// convert it back to the system provided name.
        ///
        /// When resetting the alias with an empty string, the
        /// property will default back to system name.
        ///
        /// On a well configured system, this property never
        /// needs to be changed since it defaults to the system
        /// name and provides the pretty hostname. Only if the
        /// local name needs to be different from the pretty
        /// hostname, this property should be used as last
        /// </summary>
        public string Alias { get; set; } = default;

        /// <summary>
        /// The Bluetooth class of device.
        ///
        /// This property represents the value that is either
        /// automatically configured by DMI/ACPI information
        /// or provided as static configuration.
        /// </summary>
        public uint Class { get; set; } = default;

        /// <summary>
        /// Switch an adapter on or off. This will also set the
        /// appropriate connectable state of the controller.
        ///
        /// The value of this property is not persistent. After
        /// restart or unplugging of the adapter it will reset
        /// back to false.
        /// </summary>
        public bool Powered { get; set; } = default;

        /// <summary>
        /// Switch an adapter to discoverable or non-discoverable
        /// to either make it visible or hide it. This is a global
        /// setting and should only be used by the settings
        /// application.
        ///
        /// If the DiscoverableTimeout is set to a non-zero
        /// value then the system will set this value back to
        /// false after the timer expired.
        ///
        /// In case the adapter is switched off, setting this
        /// value will fail.
        ///
        /// When changing the Powered property the new state of
        /// this property will be updated via a PropertiesChanged
        /// signal.
        ///
        /// For any new adapter this settings defaults to false.
        /// </summary>
        public bool Discoverable { get; set; } = default;

        /// <summary>
        /// The discoverable timeout in seconds. A value of zero
        /// means that the timeout is disabled and it will stay in
        /// discoverable/limited mode forever.
        ///
        /// The default value for the discoverable timeout should
        /// be 180 seconds (3 minutes).
        /// </summary>
        public uint DiscoverableTimeout { get; set; } = default;

        /// <summary>
        /// Switch an adapter to pairable or non-pairable. This is
        /// a global setting and should only be used by the
        /// settings application.
        ///
        /// Note that this property only affects incoming pairing
        /// requests.
        ///
        /// For any new adapter this settings defaults to true.
        /// </summary>
        public bool Pairable { get; set; } = default;

        /// <summary>
        /// The pairable timeout in seconds. A value of zero
        /// means that the timeout is disabled and it will stay in
        /// pairable mode forever.
        ///
        /// The default value for pairable timeout should be
        /// disabled (value 0).
        /// </summary>
        public uint PairableTimeout { get; set; } = default;

        /// <summary>
        /// Indicates that a device discovery procedure is active.
        /// </summary>
        public bool Discovering { get; set; } = default;

        /// <summary>
        /// List of 128-bit UUIDs that represents the available local services.
        /// </summary>
        public string[] UUIDs { get; set; } = default;

        /// <summary>
        /// Local Device ID information in modalias format
        /// used by the kernel and udev.
        /// </summary>
        public string Modalias { get; set; } = default;
    }

    internal static class Adapter1Extensions
    {
        public static Task<string> GetAddressAsync(this IAdapter1 o) => o.GetAsync<string>(nameof(Adapter1Properties.Address));
        public static Task<string> GetNameAsync(this IAdapter1 o) => o.GetAsync<string>(nameof(Adapter1Properties.Name));
        public static Task<string> GetAliasAsync(this IAdapter1 o) => o.GetAsync<string>(nameof(Adapter1Properties.Alias));
        public static Task<uint> GetClassAsync(this IAdapter1 o) => o.GetAsync<uint>(nameof(Adapter1Properties.Class));
        public static Task<bool> GetPoweredAsync(this IAdapter1 o) => o.GetAsync<bool>(nameof(Adapter1Properties.Powered));
        public static Task<bool> GetDiscoverableAsync(this IAdapter1 o) => o.GetAsync<bool>(nameof(Adapter1Properties.Discoverable));

        public static Task<uint> GetDiscoverableTimeoutAsync(this IAdapter1 o) =>
            o.GetAsync<uint>(nameof(Adapter1Properties.DiscoverableTimeout));

        public static Task<bool> GetPairableAsync(this IAdapter1 o) => o.GetAsync<bool>(nameof(Adapter1Properties.Pairable));
        public static Task<uint> GetPairableTimeoutAsync(this IAdapter1 o) => o.GetAsync<uint>(nameof(Adapter1Properties.PairableTimeout));
        public static Task<bool> GetDiscoveringAsync(this IAdapter1 o) => o.GetAsync<bool>(nameof(Adapter1Properties.Discovering));
        public static Task<string[]> GetUUIDsAsync(this IAdapter1 o) => o.GetAsync<string[]>(nameof(Adapter1Properties.UUIDs));
        public static Task<string> GetModaliasAsync(this IAdapter1 o) => o.GetAsync<string>(nameof(Adapter1Properties.Modalias));
        public static Task SetAliasAsync(this IAdapter1 o, string val) => o.SetAsync(nameof(Adapter1Properties.Alias), val);
        public static Task SetPoweredAsync(this IAdapter1 o, bool val) => o.SetAsync(nameof(Adapter1Properties.Powered), val);
        public static Task SetDiscoverableAsync(this IAdapter1 o, bool val) => o.SetAsync(nameof(Adapter1Properties.Discoverable), val);

        public static Task SetDiscoverableTimeoutAsync(this IAdapter1 o, uint val) =>
            o.SetAsync(nameof(Adapter1Properties.DiscoverableTimeout), val);

        public static Task SetPairableAsync(this IAdapter1 o, bool val) => o.SetAsync(nameof(Adapter1Properties.Pairable), val);
        public static Task SetPairableTimeoutAsync(this IAdapter1 o, uint val) => o.SetAsync(nameof(Adapter1Properties.PairableTimeout), val);
    }
}