using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace JsonRpcNet.Core.Tests
{
    class TestWebSocketConnection : WebSocketConnection
    {
        public IList<string> Messages { get; } = new Collection<string>();
        public IList<ArraySegment<byte>> BinaryMessages { get; } = new Collection<ArraySegment<byte>>();

        public void Close()
        {
            CloseAsync(CloseStatusCode.Normal, "test").GetAwaiter().GetResult();
        }

        public void Send(string message)
        {
            SendAsync(message).GetAwaiter().GetResult();
        }
        public bool ConnectedCalled { get; private set; } = false;
        public bool DisconnectedCalled { get; private set; } = false;
        protected override Task OnMessage(string message)
        {
           Messages.Add(message);
           return Task.CompletedTask;
        }

        protected override Task OnBinaryMessage(ArraySegment<byte> buffer)
        {
            BinaryMessages.Add(buffer);
            return Task.CompletedTask;
        }

        protected override Task OnConnected()
        {
            ConnectedCalled = true;
            return base.OnConnected();
        }

        protected override Task OnDisconnected(CloseStatusCode statusCode, string reason)
        {
            DisconnectedCalled = true;
            return base.OnDisconnected(statusCode, reason);
        }
    }

    [TestFixture]
    public class WebSocketConnectionTests
    {
        private Mock<IWebSocket> _webSocketMock;
        private TestWebSocketConnection _webSocketConnection;
        private IWebSocketConnection Connection => (IWebSocketConnection) _webSocketConnection;
        [SetUp]
        public void SetUp()
        {
            _webSocketMock = new Mock<IWebSocket>(MockBehavior.Strict);
            _webSocketConnection = new TestWebSocketConnection();
            _webSocketMock.Setup(m => m.WebSocketState)
                .Returns(JsonRpcWebSocketState.Open)
                .Callback(() => _webSocketMock.Setup(m => m.WebSocketState).Returns(JsonRpcWebSocketState.Closed));
            _webSocketMock.Setup(m => m.Id).Returns("test");
        }

        [Test, Category("Unit")]
        public void HandleMessagesAsync_Connected_ConnectionMethodsInvoked()
        {
            // ARRANGE
            SetupReceive(MessageType.Close, "test");
            
            // ACT
            Connection.HandleMessagesAsync(_webSocketMock.Object, CancellationToken.None).GetAwaiter().GetResult();
            
            // ASSERT
            Assert.That(_webSocketConnection.ConnectedCalled, Is.True);
            Assert.That(_webSocketConnection.DisconnectedCalled, Is.True);
        }
        
        [Test, Category("Unit")]
        public void HandleMessagesAsync_TextMessage_OnMessageInvoked()
        {
            // ARRANGE
            SetupReceive(MessageType.Text, "test");
            
            // ACT
            Connection.HandleMessagesAsync(_webSocketMock.Object, CancellationToken.None).GetAwaiter().GetResult();
            
            // ASSERT
            Assert.That(_webSocketConnection.ConnectedCalled, Is.True);
            Assert.That(_webSocketConnection.Messages, Has.Count.EqualTo(1));
            Assert.That(_webSocketConnection.Messages.First(), Is.EqualTo("test"));
            Assert.That(_webSocketConnection.DisconnectedCalled, Is.True);
        }
        
        [Test, Category("Unit")]
        public void HandleMessagesAsync_BinaryMessage_OnMessageInvoked()
        {
            // ARRANGE
            var buffer = Encoding.UTF8.GetBytes("test");
            SetupReceive(buffer);
            
            // ACT
            Connection.HandleMessagesAsync(_webSocketMock.Object, CancellationToken.None).GetAwaiter().GetResult();
            
            // ASSERT
            Assert.That(_webSocketConnection.ConnectedCalled, Is.True);
            Assert.That(_webSocketConnection.BinaryMessages, Has.Count.EqualTo(1));
            CollectionAssert.AreEqual(buffer, _webSocketConnection.BinaryMessages.First().Array);
            Assert.That(_webSocketConnection.DisconnectedCalled, Is.True);
        }

        [Test, Category("Unit")]
        public void Close_Connection_WebSocketClosed()
        {
            // ARRANGE
            var signal = new AutoResetEvent(false);
            var tcs = new TaskCompletionSource<(MessageType type, ArraySegment<byte>)>();
            _webSocketMock.Setup(m => m.CloseAsync((int) CloseStatusCode.Normal, "test"))
                .Returns(Task.CompletedTask)
                .Callback(() => tcs.SetResult((MessageType.Close, Encoding.UTF8.GetBytes("close"))));
            
            _webSocketMock.Setup(m => m.ReceiveAsync(CancellationToken.None)).Returns(() => tcs.Task);

            var handlerTask = Task.Factory.StartNew(
                () =>
                {
                    var t = Connection.HandleMessagesAsync(_webSocketMock.Object, CancellationToken.None);
                    signal.Set();
                    return t;
                },
                TaskCreationOptions.LongRunning);
            signal.WaitOne();
            // ACT
            _webSocketConnection.Close();
            // ASSERT
            handlerTask.Wait();
            _webSocketMock.Verify(m => m.CloseAsync((int) CloseStatusCode.Normal, "test"), Times.Once);
        }
        
        [Test, Category("Unit")]
        public void Send_TextMessage_MessageSentToWebSocket()
        {
            // ARRANGE
            var signal = new AutoResetEvent(false);
            var tcs = new TaskCompletionSource<(MessageType type, ArraySegment<byte>)>();
            // Close when "SendAsync" is invoked
            _webSocketMock.Setup(m => m.SendAsync("test"))
                .Returns(Task.CompletedTask)
                .Callback(() =>
                {
                    _webSocketMock.Setup(m => m.WebSocketState).Returns(JsonRpcWebSocketState.Closed);
                    tcs.SetResult((MessageType.Close, Encoding.UTF8.GetBytes("close")));
                });
            
            _webSocketMock.Setup(m => m.WebSocketState).Returns(JsonRpcWebSocketState.Open);
            _webSocketMock.Setup(m => m.ReceiveAsync(CancellationToken.None)).Returns(() => tcs.Task);

            var handlerTask = Task.Factory.StartNew(
                () =>
                {
                    var t = Connection.HandleMessagesAsync(_webSocketMock.Object, CancellationToken.None);
                    signal.Set();
                    return t;
                },
                TaskCreationOptions.LongRunning);
            signal.WaitOne();
            // ACT
            // invoking overload with two parameters
            _webSocketConnection.Send("test");
            
            // ASSERT
            handlerTask.Wait();
            _webSocketMock.Verify(m => m.SendAsync("test"), Times.Once);
        }

        private void SetupReceive(MessageType type, string message)
        {
            var mockSetup = _webSocketMock.Setup(m => m.ReceiveAsync(CancellationToken.None))
                .Returns(Task.FromResult<(MessageType type, ArraySegment<byte> buffer)>((type,
                    Encoding.UTF8.GetBytes(message))));
        }
        private void SetupReceive(ArraySegment<byte>buffer)
        {
            _webSocketMock.Setup(m => m.ReceiveAsync(CancellationToken.None))
                .Returns(Task.FromResult<(MessageType type, ArraySegment<byte> buffer)>((MessageType.Binary,
                    buffer)));
        }
    }
}