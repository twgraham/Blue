using System;
using System.Threading.Tasks;
using Tmds.DBus;

namespace Blue.BlueZ.DBus
{
    [Dictionary]
    internal class Network1Properties
    {
        /// <summary>
        /// Indicates if the device is connected.
        /// </summary>
        public bool Connected { get; set; } = default(bool);

        /// <summary>
        /// Indicates the network interface name when available.
        /// </summary>
        public string Interface { get; set; } = default(string);

        /// <summary>
        /// Indicates the connection role when available.
        /// </summary>
        public string UUID { get; set; } = default(string);
    }

    /// <summary>
    /// Network hierarchy
    /// =================
    ///
    /// Service		org.bluez
    /// Interface	org.bluez.Network1
    /// Object path	[variable prefix]/{hci0,hci1,...}/dev_XX_XX_XX_XX_XX_XX
    /// </summary>
    [DBusInterface(Interfaces.Network1)]
    internal interface INetwork1 : IDBusObject
    {
        /// <summary>
        /// Connect to the network device and return the network
        /// interface name. Examples of the interface name are
        /// bnep0, bnep1 etc.
        ///
        /// uuid can be either one of "gn", "panu" or "nap" (case
        /// insensitive) or a traditional string representation of
        /// UUID or a hexadecimal number.
        ///
        /// The connection will be closed and network device
        /// released either upon calling Disconnect() or when
        /// the client disappears from the message bus.
        ///
        /// Possible errors:
        ///     org.bluez.Error.AlreadyConnected
        ///     org.bluez.Error.ConnectionAttemptFailed
        /// </summary>
        /// <param name="uuid"></param>
        Task ConnectAsync(string uuid);

        /// <summary>
        /// Disconnect from the network device.
        ///
        /// To abort a connection attempt in case of errors or
        /// timeouts in the client it is fine to call this method.
        ///
        /// Possible errors:
        ///     org.bluez.Error.Failed
        /// </summary>
        Task DisconnectAsync();

        Task<T> GetAsync<T>(string prop);
        Task<Network1Properties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }

    /// <summary>
    /// Network server hierarchy
    /// ========================
    ///
    /// Service		org.bluez
    /// Interface	org.bluez.NetworkServer1
    /// Object path	/org/bluez/{hci0,hci1,...}
    /// </summary>
    [DBusInterface(Interfaces.NetworkServer1)]
    interface INetworkServer1 : IDBusObject
    {
        /// <summary>
        /// Register server for the provided UUID. Every new
        /// connection to this server will be added the bridge
        /// interface.
        ///
        /// Valid UUIDs are "gn", "panu" or "nap".
        ///
        /// Initially no network server SDP is provided. Only
        /// after this method a SDP record will be available
        /// and the BNEP server will be ready for incoming
        /// connections.
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="bridge"></param>
        Task RegisterAsync(string uuid, string bridge);

        /// <summary>
        /// Unregister the server for provided UUID.
        ///
        /// All servers will be automatically unregistered when
        /// the calling application terminates.
        /// </summary>
        /// <param name="uuid"></param>
        Task UnregisterAsync(string uuid);
    }
}