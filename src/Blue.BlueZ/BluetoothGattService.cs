using System.Collections.Generic;
using Blue.BlueZ.DBus;
using Blue.Common;
using Blue.Common.Gatt;
using JetBrains.Annotations;
using Tmds.DBus;

namespace Blue.BlueZ
{
    internal class BluetoothGattService : IBluetoothGattService, IBlueZObject
    {
        private readonly IGattService1 _gattService1Proxy;

        public ObjectPath ObjectPath => _gattService1Proxy.ObjectPath;
        public string UUID { get; }
        public bool Primary { get; }

        public List<IBluetoothGattCharacteristic> Characteristics { get; }

        internal BluetoothGattService(IGattService1 gattService1, GattService1Properties gattService1Properties)
        {
            _gattService1Proxy = gattService1;
            UUID = gattService1Properties.UUID;
            Primary = gattService1Properties.Primary;
            Characteristics = new List<IBluetoothGattCharacteristic>();
        }

        internal static BluetoothGattService Create(ManagedObject managedObject)
        {
            return new BluetoothGattService(
                managedObject.CreateProxy<IGattService1>(Services.Base),
                managedObject.GetPropertiesForInterface<GattService1Properties>(Interfaces.GattService1));
        }
    }
}