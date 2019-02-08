using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Blue.BlueZ.DBus;
using Blue.Common;
using Tmds.DBus;

namespace Blue.BlueZ
{
    public class BluetoothDevice : IBluetoothDevice, IBlueZObject, IDisposable
    {
        private readonly IDevice1 _device1;

        public ObjectPath ObjectPath => _device1.ObjectPath;
        public string Address { get; }
        public string Name { get; }
        public string Alias { get; set; }
        public bool Paired { get; }
        public bool Trusted { get; set; }
        public bool Blocked { get; set; }
        public bool LegacyPairing { get; }
        public short RSSI { get; }
        public bool Connected { get; }
        public string[] UUIDs { get; }
        public IDictionary<ushort, object> ManufacturerData { get; }
        public IDictionary<string, object> ServiceData { get; }
        public short TxPower { get; }
        public bool ServicesResolved { get; }

        internal BluetoothDevice(IDevice1 device, Device1Properties properties)
        {
            _device1 = device;
            Address = properties.Address;
            Name = properties.Name;
            Alias = properties.Alias;
            Paired = properties.Paired;
            Trusted = properties.Trusted;
            Blocked = properties.Blocked;
            LegacyPairing = properties.LegacyPairing;
            RSSI = properties.RSSI;
            Connected = properties.Connected;
            UUIDs = properties.UUIDs;
            ManufacturerData = properties.ManufacturerData;
            ServiceData = properties.ServiceData;
            TxPower = properties.TxPower;
            ServicesResolved = properties.ServicesResolved;
        }


        public void Dispose()
        {
        }

        public Task Connect()
        {
            return _device1.ConnectAsync();
        }

        public Task Disconnect()
        {
            return _device1.DisconnectAsync();
        }

        public Task ConnectProfile(string uuid)
        {
            return _device1.ConnectProfileAsync(uuid);
        }

        public Task DisconnectProfile(string uuid)
        {
            return _device1.DisconnectProfileAsync(uuid);
        }

        public async Task Pair(CancellationToken cancellationToken = default)
        {
            try
            {
                await Observable.Create<PropertyChanges>(async observer =>
                    {
                        var action = new Action<PropertyChanges>(observer.OnNext);
                        var propertySignal = await _device1.WatchPropertiesAsync(action);

                        try
                        {
                            await _device1.PairAsync().ConfigureAwait(false);
                        }
                        catch
                        {
                            propertySignal.Dispose();
                            throw;
                        }

                        return propertySignal;
                    })
                    .Where(x => x.GetStruct<bool>(nameof(Device1Properties.Paired)).GetValueOrDefault(false))
                    .Take(1)
                    .ToTask(cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                await _device1.CancelPairingAsync().ConfigureAwait(false);
                throw;
            }
        }

        internal static BluetoothDevice Create(ManagedObject managedObject)
        {
            return new BluetoothDevice(managedObject.CreateProxy<IDevice1>(Services.Base),
                managedObject.GetPropertiesForInterface<Device1Properties>(Interfaces.Device1));
        }

    }
}