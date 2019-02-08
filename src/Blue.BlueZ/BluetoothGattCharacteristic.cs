using System.Collections.Generic;
using System.Threading.Tasks;
using Blue.BlueZ.DBus;
using Blue.Common.Gatt;
using Tmds.DBus;

namespace Blue.BlueZ
{
    public class BluetoothGattCharacteristic : IBluetoothGattCharacteristic, IBlueZObject
    {
        private readonly IGattCharacteristic1 _gattCharacteristic1Proxy;

        public ObjectPath ObjectPath => _gattCharacteristic1Proxy.ObjectPath;
        public string UUID { get; }
        public IBluetoothGattService Service { get; set; }
        public List<IBluetoothGattDescriptor> Descriptors { get; }
        public bool Notifying { get; }

        internal BluetoothGattCharacteristic(IGattCharacteristic1 gattCharacteristic1,
            GattCharacteristic1Properties properties)
        {
            _gattCharacteristic1Proxy = gattCharacteristic1;
            UUID = properties.UUID;
            Notifying = properties.Notifying;
            Descriptors = new List<IBluetoothGattDescriptor>();
        }

        internal BluetoothGattCharacteristic(ManagedObject managedObject)
            : this(
                managedObject.CreateProxy<IGattCharacteristic1>(Services.Base),
                managedObject.GetPropertiesForInterface<GattCharacteristic1Properties>(Interfaces.GattCharacteristic1)
            )
        {
        }

        public async Task<byte[]> ReadValueAsync(bool sendConfirmOnReceive = false)
        {
            var data = await _gattCharacteristic1Proxy.ReadValueAsync(new Dictionary<string, object>());
            if (sendConfirmOnReceive)
                await _gattCharacteristic1Proxy.ConfirmAsync();

            return data;
        }

        public Task WriteValueAsync(byte[] value)
        {
            return _gattCharacteristic1Proxy.WriteValueAsync(value, new Dictionary<string, object>());
        }

        public Task StartNotifyAsync()
        {
            return _gattCharacteristic1Proxy.StartNotifyAsync();
        }

        public Task StopNotifyAsync()
        {
            return _gattCharacteristic1Proxy.StopNotifyAsync();
        }
    }
}