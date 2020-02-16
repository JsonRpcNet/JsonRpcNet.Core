using System.Collections.Generic;
using System.Linq;

namespace JsonRpcNet
{
    public class JsonRpcConnectionManager
    {
        public static JsonRpcConnectionManager Default { get; set; } = new JsonRpcConnectionManager();
        private readonly Dictionary<string, WebSocketConnection> _connections =
            new Dictionary<string, WebSocketConnection>();
        private readonly object _syncRoot = new object();
        
        public virtual JsonRpcWebSocketService GetById<T>(string id) where T : JsonRpcWebSocketService
        {
            lock (_syncRoot)
            {
                if (_connections.TryGetValue(id, out var service))
                {
                    
                    return service as T;
                }
            }

            return null;
        }

        public virtual IReadOnlyList<T> GetAll<T>() where T : JsonRpcWebSocketService
        {
            lock (_syncRoot)
            {
                return _connections
                    .Select(kvp => kvp.Value)
                    .OfType<T>()
                    .ToList();
            }
            
        }

        public virtual void AddSession(WebSocketConnection connection)
        {
            lock (_syncRoot)
            {
                _connections[connection.WebSocket.Id] = connection;
            }
        }

        public virtual void RemoveSession(WebSocketConnection connection)
        {
            lock (_syncRoot)
            {
                _connections.Remove(connection.WebSocket.Id);
            }
        }
    }
}