using System;
using System.Net;
using System.Threading;
using Moq;
using NUnit.Framework;

namespace JsonRpcNet.Core.Tests
{
    [TestFixture]
    public class JsonRpcConnectionManagerTests
    {
        private Mock<WebSocketConnection> _connectionMock;
        private Mock<IWebSocket> _websocketMock;
        
        private string _webSocketId = "testId";
        private JsonRpcConnectionManager _connectionManager;
        
        [SetUp]
        public void SetUp()
        {
            _connectionManager = new JsonRpcConnectionManager();
            _connectionMock = new Mock<WebSocketConnection>(MockBehavior.Strict);
            _websocketMock = new Mock<IWebSocket>(MockBehavior.Strict);
            _websocketMock.Setup(m => m.Id).Returns(_webSocketId);
        }
        [Test, Category("Unit")]
        public void GetById_Exists_Returned()
        {
            // ARRANGE
            var service = new TestWebSocketService(_connectionManager);
            ((WebSocketConnection) service).WebSocket = _websocketMock.Object;
            _connectionManager.AddSession(service);
            
            // ACT
            var result = _connectionManager.GetById<TestWebSocketService>(service.Id);

            // ASSERT
            Assert.That(result, Is.Not.Null);
        }
        
        [Test, Category("Unit")]
        public void GetAll_Exists_Returned()
        {
            // ARRANGE
            var service = new TestWebSocketService(_connectionManager);
            ((WebSocketConnection) service).WebSocket = _websocketMock.Object;
            _connectionManager.AddSession(service);
            
            // ACT
            var result = _connectionManager.GetAll<TestWebSocketService>();

            // ASSERT
            Assert.That(result, Has.Count.EqualTo(1));
        }
        
        [Test, Category("Unit")]
        public void RemoveSession_Exists_Removed()
        {
            // ARRANGE
            var service = new TestWebSocketService(_connectionManager);
            ((WebSocketConnection) service).WebSocket = _websocketMock.Object;
            _connectionManager.AddSession(service);
            
            // ACT
            _connectionManager.RemoveSession(service);

            // ASSERT
            Assert.That(_connectionManager.GetAll<TestWebSocketService>(), Has.Count.EqualTo(0));
        }
    }
}