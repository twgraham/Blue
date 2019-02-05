using System.Collections.Generic;
using System.Threading.Tasks;
using Blue.BlueZ.DBus;
using Blue.Common.Gatt;
using Blue.Infrastructure;
using Tmds.DBus;

namespace Blue.BlueZ
{
    public class BluetoothGattDescriptor : IBluetoothGattDescriptor, IBlueZObject
    {
        private readonly IGattDescriptor1 _gattDescriptor1Proxy;

        public ObjectPath ObjectPath => _gattDescriptor1Proxy.ObjectPath;
        public string UUID { get; }
        public IBluetoothGattCharacteristic Characteristic { get; }

        internal BluetoothGattDescriptor(IGattDescriptor1 descriptor1, GattDescriptor1Properties properties,
            IBluetoothGattCharacteristic parent)
        {
            _gattDescriptor1Proxy = descriptor1;
            Characteristic = parent;
            UUID = properties.UUID;
        }

        public Task<byte[]> ReadValue()
        {
            return _gattDescriptor1Proxy.ReadValueAsync(new Dictionary<string, object>());
        }

        public Task WriteValue(byte[] value)
        {
            return _gattDescriptor1Proxy.WriteValueAsync(value, new Dictionary<string, object>());
        }

        internal static BluetoothGattDescriptor Create(Connection connection, ObjectPath objectPath,
            IDictionary<string, object> properties, IBluetoothGattCharacteristic parent)
        {
            return new BluetoothGattDescriptor(
                connection.CreateProxy<IGattDescriptor1>(Services.Base, objectPath),
                properties.ToObject<GattDescriptor1Properties>(),
                parent);
        }
    }
}