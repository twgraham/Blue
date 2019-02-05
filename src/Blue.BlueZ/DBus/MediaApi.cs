using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tmds.DBus;

namespace Blue.BlueZ.DBus
{
    [DBusInterface(Interfaces.Media1)]
    internal interface IMedia1 : IDBusObject
    {
        Task RegisterEndpointAsync(ObjectPath Endpoint, IDictionary<string, object> Properties);
        Task UnregisterEndpointAsync(ObjectPath Endpoint);
        Task RegisterPlayerAsync(ObjectPath Player, IDictionary<string, object> Properties);
        Task UnregisterPlayerAsync(ObjectPath Player);
    }

    [Dictionary]
    internal class MediaPlayer1Properties
    {
        public string Equalizer { get; set; } = default;
        public string Repeat { get; set; } = default;
        public string Shuffle { get; set; } = default;
        public string Scan { get; set; } = default;
        public string Status { get; private set; } = default;
        public uint Position { get; private set; } = default;
        public IDictionary<string, object> Track { get; private set; } = default;
        public IDevice1 Device { get; private set; } = default;
        public string Name { get; private set; } = default;
        public string Type { get; private set; } = default;
        public string Subtype { get; private set; } = default;
        public bool Browsable { get; private set; } = default;
        public bool Searchable { get; private set; } = default;
        public ObjectPath Playlist { get; private set; } = default;
    }

    [DBusInterface(Interfaces.MediaPlayer1)]
    internal interface IMediaPlayer1 : IDBusObject
    {
        Task PlayAsync();
        Task PauseAsync();
        Task StopAsync();
        Task NextAsync();
        Task PreviousAsync();
        Task FastForwardAsync();
        Task RewindAsync();
        Task<T> GetAsync<T>(string prop);
        Task<MediaPlayer1Properties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }

    [DBusInterface(Interfaces.MediaControl1)]
    interface IMediaControl1 : IDevice1
    {
        Task PlayAsync();
        Task PauseAsync();
        Task StopAsync();
        Task NextAsync();
        Task PreviousAsync();
        Task VolumeUpAsync();
        Task VolumeDownAsync();
        Task FastForwardAsync();
        Task RewindAsync();
        Task<T> GetAsync<T>(string prop);
        Task<MediaControl1Properties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }

    [Dictionary]
    class MediaControl1Properties : Device1Properties
    {
        public IMediaPlayer1 Player { get; set; }
    }

    static class MediaControl1Extensions
    {
        public static Task<ObjectPath> GetPlayerAsync(this IMediaControl1 o) => o.GetAsync<ObjectPath>(nameof(MediaControl1Properties.Player));
    }
}