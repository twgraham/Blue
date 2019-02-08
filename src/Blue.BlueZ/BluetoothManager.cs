using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blue.BlueZ.DBus;
using Blue.Common;
using Blue.Common.Gatt;
using Tmds.DBus;

namespace Blue.BlueZ
{
    public class BluetoothManager : IBluetoothManager
    {
        private readonly Connection _systemConnection;
        private readonly IObjectManager _objectManager;

        public BluetoothManager() : this(Connection.System)
        {
        }

        internal BluetoothManager(Connection connection)
        {
            _systemConnection = connection;
            _objectManager = _systemConnection.CreateProxy<IObjectManager>(Services.Base, ObjectPath.Root);
        }

        public async Task<List<IBluetoothDevice>> ListDevicesAsync()
        {
            var objects = await GetObjectsWithInterface(Interfaces.Device1).ConfigureAwait(false);
            return objects.Select(BluetoothDevice.Create).ToList<IBluetoothDevice>();
        }

        public async Task<List<IBluetoothAdapter>> ListAdaptersAsync()
        {
            var objects = await GetObjectsWithInterface(Interfaces.Adapter1).ConfigureAwait(false);
            return objects.Select(BluetoothAdapter.Create).ToList<IBluetoothAdapter>();
        }

        public async Task<List<IBluetoothGattService>> ListServicesAsync()
        {
            var objects = await GetObjectsWithInterfaces(Interfaces.GattService1, Interfaces.GattCharacteristic1, Interfaces.GattDescriptor1).ConfigureAwait(false);

            var serviceObjects = new List<ManagedObject>();
            var serviceCharacteristicObjects = new Dictionary<ObjectPath, List<ManagedObject>>();
            var characteristicDescriptorObjects = new Dictionary<ObjectPath, List<ManagedObject>>();

            foreach (var dbusObject in objects)
            {
                if (dbusObject.HasInterface(Interfaces.GattService1))
                {
                    serviceObjects.Add(dbusObject);
                }
                else if (dbusObject.HasInterface(Interfaces.GattCharacteristic1))
                {
                    var characteristicProperties =
                        dbusObject.GetPropertiesForInterface<GattCharacteristic1Properties>(Interfaces
                            .GattCharacteristic1);
                    if (serviceCharacteristicObjects.TryGetValue(characteristicProperties.Service, out var characteristics))
                        characteristics.Add(dbusObject);
                    else
                        serviceCharacteristicObjects[characteristicProperties.Service] = new List<ManagedObject> { dbusObject };
                }
                else if (dbusObject.HasInterface(Interfaces.GattDescriptor1))
                {
                    var descriptorProperties =
                        dbusObject.GetPropertiesForInterface<GattDescriptor1Properties>(Interfaces
                            .GattDescriptor1);
                    if (serviceCharacteristicObjects.TryGetValue(descriptorProperties.Characteristic, out var descriptors))
                        descriptors.Add(dbusObject);
                    else
                        serviceCharacteristicObjects[descriptorProperties.Characteristic] = new List<ManagedObject> { dbusObject };
                }
            }

            var services = new List<IBluetoothGattService>();

            foreach (var serviceObject in serviceObjects)
            {
                var service = BluetoothGattService.Create(serviceObject);
                if (!serviceCharacteristicObjects.TryGetValue(service.ObjectPath, out var characteristicObjects))
                    continue;

                var characteristics = characteristicObjects.Select(o => new BluetoothGattCharacteristic(o) { Service = service })
                    .ToList();

                foreach (var characteristic in characteristics)
                {
                    if (!characteristicDescriptorObjects.TryGetValue(service.ObjectPath, out var descriptorObjects))
                        continue;

                    var descriptors = descriptorObjects.Select(o => new BluetoothGattDescriptor(o) { Characteristic = characteristic })
                        .ToList<IBluetoothGattDescriptor>();

                    characteristic.Descriptors.AddRange(descriptors);
                }

                service.Characteristics.AddRange(characteristics);
                services.Add(service);
            }

            return services;
        }

        private async Task<ManagedObject[]> GetObjectsWithInterface(string @interface)
        {
            var objects = await _objectManager.GetManagedObjectsAsync().ConfigureAwait(false);
            return objects
                .Where(x => x.HasInterface(@interface))
                .Select(x => new ManagedObject(_systemConnection, x.Key, x.Value))
                .ToArray();
        }

        private async Task<ManagedObject[]> GetObjectsWithInterfaces(
            params string[] interfaces)
        {
            var objects = await _objectManager.GetManagedObjectsAsync().ConfigureAwait(false);
            return objects
                .Where(x => interfaces.Any(i => x.HasInterface(i)))
                .Select(x => new ManagedObject(_systemConnection, x.Key, x.Value))
                .ToArray();
        }

        // private async Task<(ObjectPath, Dictionary<string, IDictionary<string, object>>)[]> GetObjectsWithAllInterfaces(params string[] interfaces)
        // {
        //     var objects = await _objectManager.GetManagedObjectsAsync().ConfigureAwait(false);
        //     return objects
        //         .Where(x => interfaces.All(i => x.Value.ContainsKey(i)))
        //         .Select(x => (x.Key, interfaces.ToDictionary(i => i, i => x.Value[i])))
        //         .ToArray();
        // }
    }
}