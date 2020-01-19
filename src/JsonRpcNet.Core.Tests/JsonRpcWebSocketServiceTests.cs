using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JsonRpcNet.Attributes;
using JsonRpcNet.Models;
using Moq;
using Newtonsoft.Json.Linq;
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
        [JsonRpcMethod]
        private string TestMethodWithParams(string param1, string param2)
        {
            MethodsInvoked.Add(nameof(TestMethodWithParams));
            return param1 + param2;
        }
        
        [JsonRpcMethod]
        private void Throws()
        {
            MethodsInvoked.Add(nameof(Throws));
            throw new InvalidOperationException("test");
        }

        [JsonRpcMethod(Name = "TestMethod23")]
        private Task<List<string>> TestMethod2()
        {
            MethodsInvoked.Add(nameof(TestMethod2));
            return Task.FromResult(new List<string> {"test", "test"});
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
        public void RcpMethod_ExistingMethod_InvokedAndTaskResultReturned()
        {
            // ARRANGE
            var jsonRpcMethodRequest = new JsonRpcRequest
            {
                Id = 1,
                Method = "TestMethod23",
                Params = new List<string>()
            };
            var jsonRpcMethod = jsonRpcMethodRequest.ToString();
            JObject result = null;
            _webSocketMock.Setup(m => m.SendAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask)
                .Callback((string s) =>
                {
                    _webSocketMock.Setup(m => m.WebSocketState).Returns(JsonRpcWebSocketState.Closed);
                    result = JObject.Parse(s);
                });

            _webSocketMock.Setup(
                    m => m.ReceiveAsync(CancellationToken.None))
                .Returns(() =>
                    Task.FromResult((MessageType.Text, new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonRpcMethod)))));


            // ACT
            _handleMessageTask =
                _webSocketConnection.HandleMessagesAsync(_webSocketMock.Object, CancellationToken.None);

            // ASSERT
            Assert.That(_webSocketService.MethodsInvoked, Contains.Item("TestMethod2"));
            Assert.That(result, Is.Not.Null);
            Assert.That(jsonRpcMethodRequest.Id, Is.EqualTo(result["id"].Value<int>()));
            Assert.That(result["result"].ToObject<List<string>>(), Is.EqualTo(new List<string> {"test", "test"}));
        }

        [Test, Category("Unit")]
        public void RcpMethod_ExistingVoidMethod_Invoked()
        {
            // ARRANGE
            var jsonRpcMethodRequest = new JsonRpcRequest
            {
                Id = 1,
                Method = "TestMethod",
                Params = new List<string>()
            };
            var jsonRpcMethod = jsonRpcMethodRequest.ToString();
            JObject result = null;
            _webSocketMock.Setup(m => m.SendAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask)
                .Callback((string s) =>
                {
                    _webSocketMock.Setup(m => m.WebSocketState).Returns(JsonRpcWebSocketState.Closed);
                    result = JObject.Parse(s);
                });

            _webSocketMock.Setup(
                    m => m.ReceiveAsync(CancellationToken.None))
                .Returns(() =>
                    Task.FromResult((MessageType.Text, new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonRpcMethod)))));


            // ACT
            _handleMessageTask =
                _webSocketConnection.HandleMessagesAsync(_webSocketMock.Object, CancellationToken.None);

            // ASSERT
            Assert.That(_webSocketService.MethodsInvoked, Contains.Item("TestMethod"));
            Assert.That(result, Is.Not.Null);
            Assert.That(jsonRpcMethodRequest.Id, Is.EqualTo(result["id"].Value<int>()));
        }
        
        [Test, Category("Unit")]
        public void RcpMethod_ExistingWithParams_Invoked()
        {
            // ARRANGE
            var jsonRpcMethodRequest = new JsonRpcRequest
            {
                Id = 1,
                Method = "TestMethodWithParams",
                Params = new List<string>{"1", "1"}
            };
            var jsonRpcMethod = jsonRpcMethodRequest.ToString();
            JObject result = null;
            _webSocketMock.Setup(m => m.SendAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask)
                .Callback((string s) =>
                {
                    _webSocketMock.Setup(m => m.WebSocketState).Returns(JsonRpcWebSocketState.Closed);
                    result = JObject.Parse(s);
                });

            _webSocketMock.Setup(
                    m => m.ReceiveAsync(CancellationToken.None))
                .Returns(() =>
                    Task.FromResult((MessageType.Text, new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonRpcMethod)))));


            // ACT
            _handleMessageTask =
                _webSocketConnection.HandleMessagesAsync(_webSocketMock.Object, CancellationToken.None);

            // ASSERT
            Assert.That(_webSocketService.MethodsInvoked, Contains.Item("TestMethodWithParams"));
            Assert.That(result, Is.Not.Null);
            Assert.That(jsonRpcMethodRequest.Id, Is.EqualTo(result["id"].Value<int>()));
            Assert.That(result["result"].Value<string>(), Is.EqualTo("11"));
        }
        
        
        [Test, Category("Unit")]
        public void RcpMethod_ExistingWithNamedParams_Invoked()
        {
            // ARRANGE
            var jsonRpcMethodRequest = new JsonRpcRequest
            {
                Id = 1,
                Method = "TestMethodWithParams",
                Params = new Dictionary<string, string>
                {
                    ["param1"] = "1",
                    ["param2"] = "1"
                }
            };
            
            var jsonRpcMethod = jsonRpcMethodRequest.ToString();
            JObject result = null;
            _webSocketMock.Setup(m => m.SendAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask)
                .Callback((string s) =>
                {
                    _webSocketMock.Setup(m => m.WebSocketState).Returns(JsonRpcWebSocketState.Closed);
                    result = JObject.Parse(s);
                });

            _webSocketMock.Setup(
                    m => m.ReceiveAsync(CancellationToken.None))
                .Returns(() =>
                    Task.FromResult((MessageType.Text, new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonRpcMethod)))));


            // ACT
            _handleMessageTask =
                _webSocketConnection.HandleMessagesAsync(_webSocketMock.Object, CancellationToken.None);

            // ASSERT
            Assert.That(_webSocketService.MethodsInvoked, Contains.Item("TestMethodWithParams"));
            Assert.That(result, Is.Not.Null);
            Assert.That(jsonRpcMethodRequest.Id, Is.EqualTo(result["id"].Value<int>()));
            Assert.That(result["result"].Value<string>(), Is.EqualTo("11"));
        }
        
        [Test, Category("Unit")]
        public void RcpMethod_ExistingVoidMethodWrongParams_ReturnsError()
        {
            // ARRANGE
            var jsonRpcMethodRequest = new JsonRpcRequest
            {
                Id = 1,
                Method = "TestMethod",
                Params = new List<string>{"wrongParam"}
            };
            var jsonRpcMethod = jsonRpcMethodRequest.ToString();
            JObject result = null;
            _webSocketMock.Setup(m => m.SendAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask)
                .Callback((string s) =>
                {
                    _webSocketMock.Setup(m => m.WebSocketState).Returns(JsonRpcWebSocketState.Closed);
                    result = JObject.Parse(s);
                });

            _webSocketMock.Setup(
                    m => m.ReceiveAsync(CancellationToken.None))
                .Returns(() =>
                    Task.FromResult((MessageType.Text, new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonRpcMethod)))));


            // ACT
            _handleMessageTask =
                _webSocketConnection.HandleMessagesAsync(_webSocketMock.Object, CancellationToken.None);

            // ASSERT
            Assert.That(result, Is.Not.Null);
            Assert.That(jsonRpcMethodRequest.Id, Is.EqualTo(result["id"].Value<int>()));
            Assert.That(-32602, Is.EqualTo(result["error"]["code"].Value<int>()));
        }
        
        [Test, Category("Unit")]
        public void RcpMethod_MethodThrows_ReturnsError()
        {
            // ARRANGE
            var jsonRpcMethodRequest = new JsonRpcRequest
            {
                Id = 1,
                Method = "Throws",
                Params = new List<string>()
            };
            var jsonRpcMethod = jsonRpcMethodRequest.ToString();
            JObject result = null;
            _webSocketMock.Setup(m => m.SendAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask)
                .Callback((string s) =>
                {
                    _webSocketMock.Setup(m => m.WebSocketState).Returns(JsonRpcWebSocketState.Closed);
                    result = JObject.Parse(s);
                });

            _webSocketMock.Setup(
                    m => m.ReceiveAsync(CancellationToken.None))
                .Returns(() =>
                    Task.FromResult((MessageType.Text, new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonRpcMethod)))));


            // ACT
            _handleMessageTask =
                _webSocketConnection.HandleMessagesAsync(_webSocketMock.Object, CancellationToken.None);

            // ASSERT
            Assert.That(_webSocketService.MethodsInvoked, Contains.Item("Throws"));
            Assert.That(result, Is.Not.Null);
            Assert.That(jsonRpcMethodRequest.Id, Is.EqualTo(result["id"].Value<int>()));
            Assert.That(-32603, Is.EqualTo(result["error"]["code"].Value<int>()));
        }
        
        [Test, Category("Unit")]
        public void RcpMethod_NonExistingMethod_ReturnsError()
        {
            // ARRANGE
            var jsonRpcMethodRequest = new JsonRpcRequest
            {
                Id = 1,
                Method = "DoesNotExisting",
                Params = new List<string>()
            };
            var jsonRpcMethod = jsonRpcMethodRequest.ToString();
            JObject result = null;
            _webSocketMock.Setup(m => m.SendAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask)
                .Callback((string s) =>
                {
                    _webSocketMock.Setup(m => m.WebSocketState).Returns(JsonRpcWebSocketState.Closed);
                    result = JObject.Parse(s);
                });

            _webSocketMock.Setup(
                    m => m.ReceiveAsync(CancellationToken.None))
                .Returns(() =>
                    Task.FromResult((MessageType.Text, new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonRpcMethod)))));


            // ACT
            _handleMessageTask =
                _webSocketConnection.HandleMessagesAsync(_webSocketMock.Object, CancellationToken.None);

            // ASSERT
            Assert.That(_webSocketService.MethodsInvoked, Is.Empty);
            Assert.That(result, Is.Not.Null);
            Assert.That(jsonRpcMethodRequest.Id, Is.EqualTo(result["id"].Value<int>()));
            Assert.That(-32601, Is.EqualTo(result["error"]["code"].Value<int>()));
        }
    }
}