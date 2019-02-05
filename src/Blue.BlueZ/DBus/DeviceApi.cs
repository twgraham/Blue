using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tmds.DBus;

namespace Blue.BlueZ.DBus
{
    [Dictionary]
    internal class Device1Properties
    {
        /// <summary>
        /// The Bluetooth device address of the remote device.
        /// </summary>
        public string Address { get; set; } = default;

        /// <summary>
        /// The Bluetooth remote name. This value can not be
        /// changed. Use the Alias property instead.
        ///
        /// This value is only present for completeness. It is
        /// better to always use the Alias property when
        /// displaying the devices name.
        ///
        /// If the Alias property is unset, it will reflect
        /// this value which makes it more convenient.
        /// </summary>
        public string Name { get; set; } = default;

        /// <summary>
        /// The name alias for the remote device. The alias can
        /// be used to have a different friendly name for the
        /// remote device.
        ///
        /// In case no alias is set, it will return the remote
        /// device name. Setting an empty string as alias will
        /// convert it back to the remote device name.
        ///
        /// When resetting the alias with an empty string, the
        /// property will default back to the remote name.
        /// </summary>
        public string Alias { get; set; } = default;

        /// <summary>
        /// The Bluetooth class of device of the remote device.
        /// </summary>
        public uint Class { get; set; } = default;

        /// <summary>
        /// External appearance of device, as found on GAP service.
        /// </summary>
        public ushort Appearance { get; set; } = default;

        /// <summary>
        /// Proposed icon name according to the freedesktop.org
        /// icon naming specification.
        /// </summary>
        public string Icon { get; set; } = default;

        /// <summary>
        /// Indicates if the remote device is paired.
        /// </summary>
        public bool Paired { get; set; } = default;

        /// <summary>
        /// Indicates if the remote is seen as trusted. This
        /// setting can be changed by the application.
        /// </summary>
        public bool Trusted { get; set; } = default;

        /// <summary>
        /// If set to true any incoming connections from the
        /// device will be immediately rejected. Any device
        /// drivers will also be removed and no new ones will
        /// be probed as long as the device is blocked.
        /// </summary>
        public bool Blocked { get; set; } = default;

        /// <summary>
        /// Set to true if the device only supports the pre-2.1
        /// pairing mechanism. This property is useful during
        /// device discovery to anticipate whether legacy or
        /// simple pairing will occur if pairing is initiated.
        ///
        /// Note that this property can exhibit false-positives
        /// in the case of Bluetooth 2.1 (or newer) devices that
        /// have disabled Extended Inquiry Response support.
        /// </summary>
        public bool LegacyPairing { get; set; } = default;

        /// <summary>
        /// Received Signal Strength Indicator of the remote
        /// device (inquiry or advertising).
        /// </summary>
        public short RSSI { get; set; } = default;

        /// <summary>
        /// Indicates if the remote device is currently connected.
        /// A PropertiesChanged signal indicate changes to this
        /// status.
        /// </summary>
        public bool Connected { get; set; } = default;

        /// <summary>
        /// List of 128-bit UUIDs that represents the available
        /// remote services.
        /// </summary>
        public string[] UUIDs { get; set; } = default;

        /// <summary>
        /// Remote Device ID information in modalias format
        /// used by the kernel and udev.
        /// </summary>
        public string Modalias { get; set; } = default;

        /// <summary>
        /// The object path of the adapter the device belongs to.
        /// </summary>
        public IAdapter1 Adapter { get; set; } = default;

        /// <summary>
        /// Manufacturer specific advertisement data. Keys are
        /// 16 bits Manufacturer ID followed by its byte array
        /// value.
        /// </summary>
        public IDictionary<ushort, object> ManufacturerData { get; set; } = default;

        /// <summary>
        /// Service advertisement data. Keys are the UUIDs in
        /// string format followed by its byte array value.
        /// </summary>
        public IDictionary<string, object> ServiceData { get; set; } = default;

        /// <summary>
        /// Advertised transmitted power level (inquiry or advertising).
        /// </summary>
        public short TxPower { get; set; } = default;

        /// <summary>
        /// Indicate whether or not service discovery has been
        /// resolved
        /// </summary>
        public bool ServicesResolved { get; set; } = default;
    }

    /// <summary>
    /// Device hierarchy
    /// ================
    ///
    /// Service		org.bluez
    /// Interface	org.bluez.Device1
    /// Object path	[variable prefix]/{hci0,hci1,...}/dev_XX_XX_XX_XX_XX_XX
    /// </summary>
    [DBusInterface(Interfaces.Device1)]
    internal interface IDevice1 : IDBusObject
    {
        /// <summary>
        /// This method gracefully disconnects all connected
        /// profiles and then terminates low-level ACL connection.
        ///
        /// ACL connection will be terminated even if some profiles
        /// were not disconnected properly e.g. due to misbehaving
        /// device.
        ///
        /// This method can be also used to cancel a preceding
        /// Connect call before a reply to it has been received.
        ///
        /// For non-trusted devices connected over LE bearer calling
        /// this method will disable incoming connections until
        /// Connect method is called again.
        ///
        /// Possible errors:
        ///     org.bluez.Error.NotConnected
        /// </summary>
        Task DisconnectAsync();

        /// <summary>
        /// This is a generic method to connect any profiles
        /// the remote device supports that can be connected
        /// to and have been flagged as auto-connectable on
        /// our side. If only subset of profiles is already
        /// connected it will try to connect currently disconnected
        /// ones.
        ///
        /// If at least one profile was connected successfully this
        /// method will indicate success.
        ///
        /// For dual-mode devices only one bearer is connected at
        /// time, the conditions are in the following order:
        ///
        ///     1. Connect the disconnected bearer if already
        ///     connected.
        ///
        ///     2. Connect first the bonded bearer. If no
        ///     bearers are bonded or both are skip and check
        ///     latest seen bearer.
        ///
        ///     3. Connect last seen bearer, in case the
        ///     timestamps are the same BR/EDR takes
        ///     precedence.
        ///
        /// Possible errors:
        ///     org.bluez.Error.NotReady
        ///     org.bluez.Error.Failed
        ///     org.bluez.Error.InProgress
        ///     org.bluez.Error.AlreadyConnected
        /// </summary>
        Task ConnectAsync();

        /// <summary>
        /// This method connects a specific profile of this
        /// device. The UUID provided is the remote service
        /// UUID for the profile.
        ///
        /// Possible errors:
        ///     org.bluez.Error.Failed
        ///     org.bluez.Error.InProgress
        ///     org.bluez.Error.InvalidArguments
        ///     org.bluez.Error.NotAvailable
        ///     org.bluez.Error.NotReady
        /// </summary>
        /// <param name="uuid"></param>
        Task ConnectProfileAsync(string uuid);

        /// <summary>
        /// This method disconnects a specific profile of
        /// this device. The profile needs to be registered
        /// client profile.
        ///
        /// There is no connection tracking for a profile, so
        /// as long as the profile is registered this will always
        /// succeed.
        ///
        /// Possible errors:
        ///     org.bluez.Error.Failed
        ///     org.bluez.Error.InProgress
        ///     org.bluez.Error.InvalidArguments
        ///     org.bluez.Error.NotSupported
        /// </summary>
        /// <param name="uuid"></param>
        Task DisconnectProfileAsync(string uuid);

        /// <summary>
        /// This method will connect to the remote device,
        /// initiate pairing and then retrieve all SDP records
        /// (or GATT primary services).
        ///
        /// If the application has registered its own agent,
        /// then that specific agent will be used. Otherwise
        /// it will use the default agent.
        ///
        /// Only for applications like a pairing wizard it
        /// would make sense to have its own agent. In almost
        /// all other cases the default agent will handle
        /// this just fine.
        ///
        /// In case there is no application agent and also
        /// no default agent present, this method will fail.
        ///
        /// Possible errors:
        ///     org.bluez.Error.InvalidArguments
        ///     org.bluez.Error.Failed
        ///     org.bluez.Error.AlreadyExists
        ///     org.bluez.Error.AuthenticationCanceled
        ///     org.bluez.Error.AuthenticationFailed
        ///     org.bluez.Error.AuthenticationRejected
        ///     org.bluez.Error.AuthenticationTimeout
        ///     org.bluez.Error.ConnectionAttemptFailed
        /// </summary>
        Task PairAsync();

        /// <summary>
        /// This method can be used to cancel a pairing
        /// operation initiated by the Pair method.
        ///
        /// Possible errors:
        ///     org.bluez.Error.DoesNotExist
        ///     org.bluez.Error.Failed
        /// </summary>
        Task CancelPairingAsync();

        Task<T> GetAsync<T>(string prop);
        Task<Device1Properties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }

    internal static class Device1Extensions
    {
        public static Task<string> GetAddressAsync(this IDevice1 o) => o.GetAsync<string>(nameof(Device1Properties.Address));
        public static Task<string> GetNameAsync(this IDevice1 o) => o.GetAsync<string>(nameof(Device1Properties.Name));
        public static Task<string> GetAliasAsync(this IDevice1 o) => o.GetAsync<string>(nameof(Device1Properties.Alias));
        public static Task<uint> GetClassAsync(this IDevice1 o) => o.GetAsync<uint>("Class");
        public static Task<ushort> GetAppearanceAsync(this IDevice1 o) => o.GetAsync<ushort>(nameof(Device1Properties.Appearance));
        public static Task<string> GetIconAsync(this IDevice1 o) => o.GetAsync<string>(nameof(Device1Properties.Icon));
        public static Task<bool> GetPairedAsync(this IDevice1 o) => o.GetAsync<bool>(nameof(Device1Properties.Paired));
        public static Task<bool> GetTrustedAsync(this IDevice1 o) => o.GetAsync<bool>(nameof(Device1Properties.Trusted));
        public static Task<bool> GetBlockedAsync(this IDevice1 o) => o.GetAsync<bool>(nameof(Device1Properties.Blocked));
        public static Task<bool> GetLegacyPairingAsync(this IDevice1 o) => o.GetAsync<bool>(nameof(Device1Properties.LegacyPairing));
        public static Task<short> GetRSSIAsync(this IDevice1 o) => o.GetAsync<short>(nameof(Device1Properties.RSSI));
        public static Task<bool> GetConnectedAsync(this IDevice1 o) => o.GetAsync<bool>(nameof(Device1Properties.Connected));
        public static Task<string[]> GetUUIDsAsync(this IDevice1 o) => o.GetAsync<string[]>(nameof(Device1Properties.UUIDs));
        public static Task<string> GetModaliasAsync(this IDevice1 o) => o.GetAsync<string>(nameof(Device1Properties.Modalias));
        public static Task<IAdapter1> GetAdapterAsync(this IDevice1 o) => o.GetAsync<IAdapter1>(nameof(Device1Properties.Adapter));

        public static Task<IDictionary<ushort, object>> GetManufacturerDataAsync(this IDevice1 o) =>
            o.GetAsync<IDictionary<ushort, object>>(nameof(Device1Properties.ManufacturerData));

        public static Task<IDictionary<string, object>> GetServiceDataAsync(this IDevice1 o) =>
            o.GetAsync<IDictionary<string, object>>(nameof(Device1Properties.ServiceData));

        public static Task<short> GetTxPowerAsync(this IDevice1 o) => o.GetAsync<short>(nameof(Device1Properties.TxPower));
        public static Task<bool> GetServicesResolvedAsync(this IDevice1 o) => o.GetAsync<bool>(nameof(Device1Properties.ServicesResolved));
        public static Task SetAliasAsync(this IDevice1 o, string val) => o.SetAsync(nameof(Device1Properties.Alias), val);
        public static Task SetTrustedAsync(this IDevice1 o, bool val) => o.SetAsync(nameof(Device1Properties.Trusted), val);
        public static Task SetBlockedAsync(this IDevice1 o, bool val) => o.SetAsync(nameof(Device1Properties.Blocked), val);
    }
}