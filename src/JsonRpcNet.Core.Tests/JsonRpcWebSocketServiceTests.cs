using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JsonRpcNet.Attributes;
using JsonRpcNet.Models;
using Moq;
using NUnit.Framework;

namespace JsonRpcNet.Core.Tests
{
    class TestWebSocketService : JsonRpcWebSocketService
    {
        public IList<string> MethodsInvoked { get; } = new List<string>();

        public TestWebSocketService()
        {
            
        }
        [JsonRpcMethod]
        private void TestMethod()
        {
            MethodsInvoked.Add(nameof(TestMethod));
        }
        
        [JsonRpcMethod(Name = "TestMethod23")]
        private Task TestMethod2()
        {
            MethodsInvoked.Add(nameof(TestMethod2));
            return Task.CompletedTask;
        }
    }
    [TestFixture]
    public class JsonRpcWebSocketServiceTests
    {
        private TestWebSocketService _webSocketService;
        private IWebSocketConnection _webSocketConnection;
        private Mock<IWebSocket> _webSocketMock;
        private CancellationTokenSource _cts;
        private Task _handleMessageTask;
         
        [SetUp]
        public void SetUp()
        {
            _webSocketMock = new Mock<IWebSocket>(MockBehavior.Strict);
            _webSocketMock.Setup(m => m.WebSocketState).Returns(JsonRpcWebSocketState.Open);
            _webSocketMock.Setup(m => m.Id).Returns(Guid.NewGuid().ToString("N"));
            _webSocketService = new TestWebSocketService();
            _webSocketConnection = _webSocketService;
            _cts = new CancellationTokenSource();
        }

        [TearDown]
        public void TearDown()
        {
            _cts.Cancel();
            _cts.Dispose();
            _handleMessageTask.Wait(100);
        }

        [Test, Category("Unit")]
        public void RcpMethod_ExistingMethod_Invoked()
        {
            // ARRANGE
            var jsonRpcMethod = new JsonRpcRequest
            {
                Id = 1,
                Method = "TestMethod",
                Params = new List<string>()
            }.ToString();

            _webSocketMock.Setup(
                    m => m.ReceiveAsync(CancellationToken.None))
                .Returns(() =>
                    Task.FromResult((MessageType.Text, new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonRpcMethod)))))
                .Callback(() => _webSocketMock.Setup(m => m.WebSocketState).Returns(JsonRpcWebSocketState.Closed));
            
            
            // ACT
            _handleMessageTask = _webSocketConnection.HandleMessagesAsync(_webSocketMock.Object, CancellationToken.None);
         
            // ASSERT
            Assert.That(_webSocketService.MethodsInvoked, Contains.Item("TestMethod"));
        }
    }
}