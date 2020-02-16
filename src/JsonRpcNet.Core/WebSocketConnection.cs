using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JsonRpcNet
{
    public abstract class WebSocketConnection : IWebSocketConnection
    {
        private readonly JsonRpcConnectionManager _connectionManager;
        public IWebSocket WebSocket { get; internal set; }

        protected WebSocketConnection(JsonRpcConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }
        async Task IWebSocketConnection.HandleMessagesAsync(IWebSocket socket, CancellationToken cancellation)
        {
            WebSocket = socket;
            _connectionManager.AddSession(this);
            await OnConnected();
            bool onDisconnectedInvoked = false;
            while (WebSocket.WebSocketState == JsonRpcWebSocketState.Open)
            {
                var (type, buffer) = await WebSocket.ReceiveAsync(cancellation);
                string message = null;
                if (buffer.Array == null)
                {
                    _connectionManager.RemoveSession(this);
                    const int invalidPayloadData = 1007;
                    await WebSocket.CloseAsync(invalidPayloadData, "Received empty data buffer");
                    throw new InvalidOperationException("Received empty data buffer from underlying socket");
                }
                if (type != MessageType.Binary)
                {
                    message = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
                }

                if (type == MessageType.Text)
                {
                    await OnMessage(message);
                }
                else if (type == MessageType.Binary)
                {
                    await OnBinaryMessage(buffer);
                }
                else if (type == MessageType.Close)
                {
                    await OnDisconnected(CloseStatusCode.Normal, message);
                    onDisconnectedInvoked = true;
                    break;
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            
            _connectionManager.RemoveSession(this);
            if (!onDisconnectedInvoked)
            {
                await OnDisconnected(CloseStatusCode.Normal, "Socket closed");
            }
        }

        protected virtual Task OnConnected()
        {
            return Task.CompletedTask;
        }
                                           
        protected virtual Task OnDisconnected(CloseStatusCode statusCode, string reason)
        {
            return Task.CompletedTask;
        }

        protected Task CloseAsync(CloseStatusCode statusCode, string reason)
        {
            return WebSocket.CloseAsync((int)statusCode, reason);
        }
        
        protected async Task SendAsync(string message)
        {
            if (WebSocket.WebSocketState != JsonRpcWebSocketState.Open)
            {
                return;
            }
            
            await WebSocket.SendAsync(message).ConfigureAwait(false);
        }

        protected IPAddress GetUserEndpointIpAddress()
        {
            // UserEndPoint can be disposed if e.g. the user closes the connection prematurely
            try
            {
                return WebSocket.UserEndPoint.Address;
            }
            catch
            {
                return null;
            }
        }
        protected abstract Task OnMessage(string message);
        protected abstract Task OnBinaryMessage(ArraySegment<byte> buffer);
    }
}