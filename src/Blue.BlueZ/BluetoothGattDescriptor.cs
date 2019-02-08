using System.Collections.Generic;
using System.Threading.Tasks;
using Blue.BlueZ.DBus;
using Blue.Common.Gatt;
using JetBrains.Annotations;
using Tmds.DBus;

namespace Blue.BlueZ
{
    internal class BluetoothGattDescriptor : IBluetoothGattDescriptor, IBlueZObject
    {
        private readonly IGattDescriptor1 _gattDescriptor1Proxy;

        public ObjectPath ObjectPath => _gattDescriptor1Proxy.ObjectPath;
        public string UUID { get; }

        [CanBeNull]
        public IBluetoothGattCharacteristic Characteristic { get; set; }

        internal BluetoothGattDescriptor(IGattDescriptor1 descriptor1, GattDescriptor1Properties properties)
        {
            _gattDescriptor1Proxy = descriptor1;
            UUID = properties.UUID;
        }

        internal BluetoothGattDescriptor(ManagedObject managedObject)
            : this(
                managedObject.CreateProxy<IGattDescriptor1>(Services.Base),
                managedObject.GetPropertiesForInterface<GattDescriptor1Properties>(Interfaces.GattDescriptor1)
            )
        {
        }

        public Task<byte[]> ReadValue()
        {
            return _gattDescriptor1Proxy.ReadValueAsync(new Dictionary<string, object>());
        }

        public Task WriteValue(byte[] value)
        {
            return _gattDescriptor1Proxy.WriteValueAsync(value, new Dictionary<string, object>());
        }
    }
}