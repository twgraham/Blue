using System.Threading.Tasks;

namespace Blue.Common.Gatt
{
    public interface IBluetoothGattDescriptor
    {
        string UUID { get; }
        IBluetoothGattCharacteristic Characteristic { get; }

        Task<byte[]> ReadValue();
        Task WriteValue(byte[] value);
    }
}