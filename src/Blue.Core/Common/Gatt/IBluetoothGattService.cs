using System.Collections.Generic;

namespace Blue.Common.Gatt
{
    public interface IBluetoothGattService
    {
        string UUID { get; }

        bool Primary { get; }

        List<IBluetoothGattCharacteristic> Characteristics { get; }
    }
}