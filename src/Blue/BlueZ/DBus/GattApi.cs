using System.Collections.Generic;
using System.Threading.Tasks;
using Tmds.DBus;

/**
* BlueZ D-Bus GATT API description
* ********************************
*
* GATT local and remote services share the same high-level D-Bus API. Local
* refers to GATT based service exported by a BlueZ plugin or an external
* application. Remote refers to GATT services exported by the peer.
*
* BlueZ acts as a proxy, translating ATT operations to D-Bus method calls and
* Properties (or the opposite). Support for D-Bus Object Manager is mandatory for
* external services to allow seamless GATT declarations (Service, Characteristic
* and Descriptors) discovery. Each GATT service tree is required to export a D-Bus
* Object Manager at its root that is solely responsible for the objects that
* belong to that service.
*
* Releasing a registered GATT service is not defined yet. Any API extension
* should avoid breaking the defined API, and if possible keep an unified GATT
* remote and local services representation.
*/
namespace Blue.BlueZ.DBus
{
    /// <summary>
    /// Service hierarchy
    /// =================
    ///
    /// GATT remote and local service representation. Object path for local services
    /// is freely definable.
    ///
    /// External applications implementing local services must register the services
    /// using GattManager1 registration method and must implement the methods and
    /// properties defined in GattService1 interface.
    ///
    /// Service		org.bluez
    /// Interface	org.bluez.GattService1
    /// Object path	[variable prefix]/{hci0,hci1,...}/dev_XX_XX_XX_XX_XX_XX/serviceXX
    /// </summary>
    [DBusInterface("org.bluez.GattService1")]
    internal interface IGattService1
    {
    }

    [Dictionary]
    internal class GattService1Properties
    {
	    /// <summary>
	    /// 128-bit service UUID.
	    /// </summary>
	    public string UUID { get; set; } = default;

	    /// <summary>
	    /// Indicates whether or not this GATT service is a
	    /// primary service. If false, the service is secondary.
	    /// </summary>
	    public bool Primary { get; set; } = default;

	    /// <summary>
	    /// Object path of the Bluetooth device the service
	    /// belongs to. Only present on services from remote
		/// devices.
	    /// </summary>
	    public ObjectPath Device { get; set; } = default;

	    /// <summary>
	    /// Array of object paths representing the included
	    /// services of this service.
	    /// </summary>
	    public ObjectPath[] Includes { get; set; } = default;

	    /// <summary>
	    /// Service handle. When available in the server it
	    /// would attempt to use to allocate into the database
		/// which may fail, to auto allocate the value 0x0000
	    /// shall be used which will cause the allocated handle to
	    /// be set once registered.
	    /// </summary>
	    public ushort Handle { get; set; } = default;
    }

    /// <summary>
    /// Characteristic hierarchy
    /// ========================
    ///
    /// For local GATT defined services, the object paths need to follow the service
    /// path hierarchy and are freely definable.
    ///
    /// Service		org.bluez
    /// Interface	org.bluez.GattCharacteristic1
    /// Object path	[variable prefix]/{hci0,hci1,...}/dev_XX_XX_XX_XX_XX_XX/serviceXX/charYYYY
    /// </summary>
    [DBusInterface("org.bluez.GattCharacteristic1")]
    internal interface IGattCharacteristic1
    {
	    /// <summary>
	    /// Issues a request to read the value of the
	    /// characteristic and returns the value if the
	    /// operation was successful.
	    ///
	    /// Possible options:
	    /// "offset": uint16 offset
	    /// "mtu": Exchanged MTU (Server only)
	    /// "device": Object Device (Server only)
	    ///
	    /// Possible Errors:
	    /// 	org.bluez.Error.Failed
	    /// 	org.bluez.Error.InProgress
	    ///     org.bluez.Error.NotPermitted
	    /// 	org.bluez.Error.NotAuthorized
	    ///     org.bluez.Error.InvalidOffset
	    /// 	org.bluez.Error.NotSupported
	    /// </summary>
	    /// <param name="options"></param>
	    Task<byte[]> ReadValueAsync(IDictionary<string, object> options);

	    /// <summary>
	    /// Issues a request to write the value of the
	    /// characteristic.
	    ///
	    /// Possible options:
	    /// "offset": Start offset
	    /// "mtu": Exchanged MTU (Server only)
	    /// "device": Device path (Server only)
	    /// "link": Link type (Server only)
	    /// "prepare-authorize": True if prepare authorization request
	    ///
	    /// Possible Errors:
	    /// 	org.bluez.Error.Failed
	    ///     org.bluez.Error.InProgress
	    /// 	org.bluez.Error.NotPermitted
	    ///     org.bluez.Error.InvalidValueLength
	    /// 	org.bluez.Error.NotAuthorized
	    ///     org.bluez.Error.NotSupported
	    /// </summary>
	    /// <param name="value"></param>
	    /// <param name="options"></param>
	    Task WriteValueAsync(byte[] value, IDictionary<string, object> options);

	    /// <summary>
	    /// Starts a notification session from this characteristic
	    /// if it supports value notifications or indications.
	    ///
	    /// Possible Errors:
	    /// 	org.bluez.Error.Failed
	    ///     org.bluez.Error.NotPermitted
	    /// 	org.bluez.Error.InProgress
	    ///     org.bluez.Error.NotSupported
	    /// </summary>
	    Task StartNotifyAsync();

	    /// <summary>
	    /// This method will cancel any previous StartNotify
	    /// transaction. Note that notifications from a
	    /// characteristic are shared between sessions thus
	    /// calling StopNotify will release a single session.
	    ///
	    /// Possible Errors:
	    /// 	org.bluez.Error.Failed
	    /// </summary>
	    Task StopNotifyAsync();

	    /// <summary>
	    /// This method doesn't expect a reply so it is just a
	    /// confirmation that value was received.
	    ///
	    /// Possible Errors:
	    /// 	org.bluez.Error.Failed
	    /// </summary>
	    /// <returns></returns>
	    Task ConfirmAsync();

	    // TODO: Implement methods with file descriptors
	    //    fd, uint16 AcquireWrite(dict options) [optional]
	    //
	    // 	Acquire file descriptor and MTU for writing. Only
	    // 	sockets are supported. Usage of WriteValue will be
	    // 	locked causing it to return NotPermitted error.
	    //
	    // 	For server the MTU returned shall be equal or smaller
	    // 	than the negotiated MTU.
	    //
	    // 	For client it only works with characteristic that has
	    // 	WriteAcquired property which relies on
	    // 	write-without-response Flag.
	    //
	    // 	To release the lock the client shall close the file
	    // 	descriptor, a HUP is generated in case the device
	    // 	is disconnected.
	    //
	    // 	Note: the MTU can only be negotiated once and is
	    // 	symmetric therefore this method may be delayed in
	    // 	order to have the exchange MTU completed, because of
	    // 	that the file descriptor is closed during
	    // 	reconnections as the MTU has to be renegotiated.
	    //
	    // 	Possible options: "device": Object Device (Server only)
	    // 			  "mtu": Exchanged MTU (Server only)
	    // 			  "link": Link type (Server only)
	    //
	    // 	Possible Errors: org.bluez.Error.Failed
	    // 			 org.bluez.Error.NotSupported
	    //
	    // fd, uint16 AcquireNotify(dict options) [optional]
	    //
	    // 	Acquire file descriptor and MTU for notify. Only
	    // 	sockets are support. Usage of StartNotify will be locked
	    // 	causing it to return NotPermitted error.
	    //
	    // 	For server the MTU returned shall be equal or smaller
	    // 	than the negotiated MTU.
	    //
	    // 	Only works with characteristic that has NotifyAcquired
	    // 	which relies on notify Flag and no other client have
	    // 	called StartNotify.
	    //
	    // 	Notification are enabled during this procedure so
	    // 	StartNotify shall not be called, any notification
	    // 	will be dispatched via file descriptor therefore the
	    // 	Value property is not affected during the time where
	    // 	notify has been acquired.
	    //
	    // 	To release the lock the client shall close the file
	    // 	descriptor, a HUP is generated in case the device
	    // 	is disconnected.
	    //
	    // 	Note: the MTU can only be negotiated once and is
	    // 	symmetric therefore this method may be delayed in
	    // 	order to have the exchange MTU completed, because of
	    // 	that the file descriptor is closed during
	    // 	reconnections as the MTU has to be renegotiated.
	    //
	    // 	Possible options: "device": Object Device (Server only)
	    // 			  "mtu": Exchanged MTU (Server only)
	    // 			  "link": Link type (Server only)
	    //
	    // 	Possible Errors: org.bluez.Error.Failed
	    // 			 org.bluez.Error.NotSupported
    }

    [Dictionary]
    internal class GattCharacteristic1Properties
    {
	    /// <summary>
	    /// 128-bit characteristic UUID.
	    /// </summary>
	    public string UUID { get; set; } = default;

	    /// <summary>
	    /// Object path of the GATT service the characteristic belongs to.
	    /// </summary>
	    public ObjectPath Service { get; set; } = default;

	    /// <summary>
	    /// The cached value of the characteristic. This property
	    /// gets updated only after a successful read request and
		/// when a notification or indication is received, upon
		/// which a PropertiesChanged signal will be emitted.
	    /// </summary>
	    public byte[] Value { get; set; } = default;

	    /// <summary>
	    /// True, if this characteristic has been acquired by any
	    /// client using AcquireWrite.
		///
		/// For client properties is ommited in case
	    /// 'write-without-response' flag is not set.
		///
		/// For server the presence of this property indicates
	    /// that AcquireWrite is supported.
	    /// </summary>
	    public bool WriteAcquired { get; set; } = default;

	    /// <summary>
	    /// True, if this characteristic has been acquired by any
	    /// client using AcquireNotify.
		///
		/// For client this properties is ommited in case 'notify'
	    /// flag is not set.
		///
		/// For server the presence of this property indicates
	    /// that AcquireNotify is supported.
	    /// </summary>
	    public bool NotifyAcquired { get; set; } = default;

	    /// <summary>
	    /// True, if notifications or indications on this
	    /// characteristic are currently enabled.
	    /// </summary>
	    public bool Notifying { get; set; } = default;

	    /// <summary>
	    /// Defines how the characteristic value can be used. See
	    /// Core spec "Table 3.5: Characteristic Properties bit
	    /// field", and "Table 3.8: Characteristic Extended
	    /// Properties bit field". Allowed values:
	    ///
	    /// "broadcast"
	    /// "read"
	    /// "write-without-response"
	    /// "write"
	    /// "notify"
	    /// "indicate"
	    /// "authenticated-signed-writes"
	    /// "reliable-write"
	    /// "writable-auxiliaries"
	    /// "encrypt-read"
	    /// "encrypt-write"
	    /// "encrypt-authenticated-read"
	    /// "encrypt-authenticated-write"
	    /// "secure-read" (Server only)
	    /// "secure-write" (Server only)
	    /// "authorize"
	    /// </summary>
	    public string[] Flags { get; set; } = default;

	    /// <summary>
	    /// Characteristic handle. When available in the server it
		/// would attempt to use to allocate into the database
		/// which may fail, to auto allocate the value 0x0000
		/// shall be used which will cause the allocated handle to
		/// be set once registered.
	    /// </summary>
	    public ushort Handle { get; set; } = default;
    }

    /// <summary>
    /// Characteristic Descriptors hierarchy
    /// ====================================
    ///
    /// Local or remote GATT characteristic descriptors hierarchy.
    ///
    /// Service		org.bluez
    /// Interface	org.bluez.GattDescriptor1
    /// Object path	[variable prefix]/{hci0,hci1,...}/dev_XX_XX_XX_XX_XX_XX/serviceXX/charYYYY/descriptorZZZ
    /// </summary>
    [DBusInterface("org.bluez.GattDescriptor1")]
    internal interface IGattDescriptor1
    {
	    /// <summary>
	    /// Issues a request to read the value of the
	    ///	characteristic and returns the value if the
	    ///	operation was successful.
	    ///
	    ///	Possible options:
	    /// 	"offset": Start offset
	    ///		"device": Device path (Server only)
	    ///		"link": Link type (Server only)
	    ///
	    ///	Possible Errors:
	    /// 	org.bluez.Error.Failed
	    ///		org.bluez.Error.InProgress
	    ///		org.bluez.Error.NotPermitted
	    ///		org.bluez.Error.NotAuthorized
	    ///		org.bluez.Error.NotSupported
	    /// </summary>
	    /// <param name="flags"></param>
	    Task<byte[]> ReadValueAsync(IDictionary<string, object> flags);

	    /// <summary>
	    /// Issues a request to write the value of the
		///	characteristic.
		///
		///	Possible options:
		/// 	"offset": Start offset
		///		"device": Device path (Server only)
		///		"link": Link type (Server only)
		///		"prepare-authorize": boolean Is prepare authorization request
		///
		///	Possible Errors:
		/// 	org.bluez.Error.Failed
		///		org.bluez.Error.InProgress
		///		org.bluez.Error.NotPermitted
		///		org.bluez.Error.InvalidValueLength
		///		org.bluez.Error.NotAuthorized
		///		org.bluez.Error.NotSupported
	    /// </summary>
	    /// <param name="value"></param>
	    /// <param name="flags"></param>
	    Task WriteValueAsync(byte[] value, IDictionary<string, object> flags);
    }

    [Dictionary]
    internal class GattDescriptor1Properties
    {
	    /// <summary>
	    /// 128-bit descriptor UUID.
	    /// </summary>
	    public string UUID { get; set; } = default;

	    /// <summary>
	    /// Object path of the GATT characteristic the descriptor
	    /// belongs to.
	    /// </summary>
	    public ObjectPath Characteristic { get; set; } = default;

	    /// <summary>
	    /// The cached value of the descriptor. This property
	    /// gets updated only after a successful read request, upon
	    /// which a PropertiesChanged signal will be emitted.
	    /// </summary>
	    public byte[] Value { get; set; } = default;

	    /// <summary>
	    /// Defines how the descriptor value can be used.
		///
	    /// Possible values:
		///
	    /// "read"
	    /// "write"
	    /// "encrypt-read"
	    /// "encrypt-write"
	    /// "encrypt-authenticated-read"
	    /// "encrypt-authenticated-write"
	    /// "secure-read" (Server Only)
	    /// "secure-write" (Server Only)
	    /// "authorize"
	    /// </summary>
	    public string[] Flags { get; set; } = default;

	    /// <summary>
	    /// Characteristic handle. When available in the server it
	    /// would attempt to use to allocate into the database
		/// which may fail, to auto allocate the value 0x0000
	    /// shall be used which will cause the allocated handle to
	    /// be set once registered.
	    /// </summary>
	    public ushort Handle { get; set; } = default;
    }

    /// <summary>
    /// GATT Profile hierarchy
    /// =====================
    ///
    /// Local profile (GATT client) instance. By registering this type of object
    /// an application effectively indicates support for a specific GATT profile
    /// and requests automatic connections to be established to devices
    /// supporting it.
    ///
    /// Service		&lt;application dependent&gt;
    /// Interface	org.bluez.GattProfile1
    /// Object path	&lt;application dependent&gt;
    /// </summary>
    [DBusInterface("org.bluez.GattProfile1")]
    internal interface IGattProfile1
    {
	    /// <summary>
	    /// This method gets called when the service daemon
		///	unregisters the profile. The profile can use it to
		///	do cleanup tasks. There is no need to unregister the
		///	profile, because when this method gets called it has
		///	already been unregistered.
	    /// </summary>
	    Task ReleaseAsync();
    }

    [Dictionary]
    internal class IGattProfile1Properties
    {
	    /// <summary>
	    /// 128-bit GATT service UUIDs to auto connect.
	    /// </summary>
	    public string[] UUIDs { get; set; } = default;
    }

	/// <summary>
	/// GATT Manager hierarchy
	/// ======================
	///
	/// GATT Manager allows external applications to register GATT services and
	/// profiles.
	///
	/// Registering a profile allows applications to subscribe to *remote* services.
	/// These must implement the GattProfile1 interface defined above.
	///
	/// Registering a service allows applications to publish a *local* GATT service,
	/// which then becomes available to remote devices. A GATT service is represented by
	/// a D-Bus object hierarchy where the root node corresponds to a service and the
	/// child nodes represent characteristics and descriptors that belong to that
	/// service. Each node must implement one of GattService1, GattCharacteristic1,
	/// or GattDescriptor1 interfaces described above, based on the attribute it
	/// represents. Each node must also implement the standard D-Bus Properties
	/// interface to expose their properties. These objects collectively represent a
	/// GATT service definition.
	///
	/// To make service registration simple, BlueZ requires that all objects that belong
	/// to a GATT service be grouped under a D-Bus Object Manager that solely manages
	/// the objects of that service. Hence, the standard DBus.ObjectManager interface
	/// must be available on the root service path. An example application hierarchy
	/// containing two separate GATT services may look like this:
	///
	/// -> /com/example
	///   |   - org.freedesktop.DBus.ObjectManager
	///   |
	///   -> /com/example/service0
	///   | |   - org.freedesktop.DBus.Properties
	///   | |   - org.bluez.GattService1
	///   | |
	///   | -> /com/example/service0/char0
	///   | |     - org.freedesktop.DBus.Properties
	///   | |     - org.bluez.GattCharacteristic1
	///   | |
	///   | -> /com/example/service0/char1
	///   |   |   - org.freedesktop.DBus.Properties
	///   |   |   - org.bluez.GattCharacteristic1
	///   |   |
	///   |   -> /com/example/service0/char1/desc0
	///   |       - org.freedesktop.DBus.Properties
	///   |       - org.bluez.GattDescriptor1
	///   |
	///   -> /com/example/service1
	///     |   - org.freedesktop.DBus.Properties
	///     |   - org.bluez.GattService1
	///     |
	///     -> /com/example/service1/char0
	///         - org.freedesktop.DBus.Properties
	///         - org.bluez.GattCharacteristic1
	///
	/// When a service is registered, BlueZ will automatically obtain information about
	/// all objects using the service's Object Manager. Once a service has been
	/// registered, the objects of a service should not be removed. If BlueZ receives an
	/// InterfacesRemoved signal from a service's Object Manager, it will immediately
	/// unregister the service. Similarly, if the application disconnects from the bus,
	/// all of its registered services will be automatically unregistered.
	/// InterfacesAdded signals will be ignored.
	///
	/// Examples:
	/// 	- Client
	/// 		test/example-gatt-client
	/// 		client/bluetoothctl
	/// 	- Server
	/// 		test/example-gatt-server
	/// 		tools/gatt-service
	///
	///
	/// Service		org.bluez
	/// Interface	org.bluez.GattManager1
	/// Object path	[variable prefix]/{hci0,hci1,...}
	/// </summary>
	[DBusInterface("org.bluez.GattManager1")]
	interface IGattManager1 : IDBusObject
	{
		/// <summary>
		/// Registers a local GATT services hierarchy as described
		///	above (GATT Server) and/or GATT profiles (GATT Client).
		///
		///	The application object path together with the D-Bus
		///	system bus connection ID define the identification of
		///	the application registering a GATT based
		///	service or profile.
		///
		///	Possible errors:
		/// 	org.bluez.Error.InvalidArguments
		///		org.bluez.Error.AlreadyExists
		/// </summary>
		/// <param name="application"></param>
		/// <param name="options"></param>
		Task RegisterApplicationAsync(ObjectPath application, IDictionary<string, object> options);

		/// <summary>
		/// This unregisters the services that has been
		///	previously registered. The object path parameter
		///	must match the same value that has been used
		///	on registration.
		///
		///	Possible errors:
		/// 	org.bluez.Error.InvalidArguments
		///		org.bluez.Error.DoesNotExist
		/// </summary>
		/// <param name="application"></param>
		Task UnregisterApplicationAsync(ObjectPath application);
	}
}