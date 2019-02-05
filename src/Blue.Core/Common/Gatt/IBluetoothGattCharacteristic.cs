using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blue.Common.Gatt
{
    public interface IBluetoothGattCharacteristic
    {
        string UUID { get; }
        bool Notifying { get; }
        IBluetoothGattService Service { get; }
        List<IBluetoothGattDescriptor> Descriptors { get; }

	    Task<byte[]> ReadValueAsync(bool sendConfirmOnReceive = false);
	    Task WriteValueAsync(byte[] value);
	    Task StartNotifyAsync();
	    Task StopNotifyAsync();
    }
}