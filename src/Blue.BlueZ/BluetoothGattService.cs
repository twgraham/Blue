using System.Collections.Generic;
using Blue.BlueZ.DBus;
using Blue.Common;
using Blue.Common.Gatt;
using Blue.Infrastructure;
using Tmds.DBus;

namespace Blue.BlueZ
{
    public class BluetoothGattService : IBluetoothGattService, IBlueZObject
    {
        private readonly IGattService1 _gattService1Proxy;

        public ObjectPath ObjectPath => _gattService1Proxy.ObjectPath;
        public string UUID { get; }
        public bool Primary { get; }
        public IBluetoothDevice Device { get; }
        public List<IBluetoothGattCharacteristic> Characteristics { get; }

        internal BluetoothGattService(IGattService1 gattService1, GattService1Properties gattService1Properties)
        {
            _gattService1Proxy = gattService1;
            UUID = gattService1Properties.UUID;
            Primary = gattService1Properties.Primary;
        }

        internal static BluetoothGattService Create(Connection connection, ObjectPath objectPath,
            IDictionary<string, object> properties)
        {
            return new BluetoothGattService(connection.CreateProxy<IGattService1>(Services.Base, objectPath),
                properties.ToObject<GattService1Properties>());
        }

    }
}