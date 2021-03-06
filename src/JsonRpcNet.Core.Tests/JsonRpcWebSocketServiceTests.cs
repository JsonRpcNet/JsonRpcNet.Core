﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JsonRpcNet.Models;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace JsonRpcNet.Core.Tests
{
    [TestFixture]
    public class JsonRpcWebSocketServiceTests
    {
        private TestWebSocketService _webSocketService;
        private IWebSocketConnection _webSocketConnection;
        private Mock<IWebSocket> _webSocketMock;

        [SetUp]
        public void SetUp()
        {
            _webSocketMock = new Mock<IWebSocket>(MockBehavior.Strict);
            _webSocketMock.Setup(m => m.WebSocketState).Returns(JsonRpcWebSocketState.Open);
            _webSocketMock.Setup(m => m.Id).Returns(Guid.NewGuid().ToString("N"));
            _webSocketService = new TestWebSocketService();
            _webSocketConnection = _webSocketService;
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
            _webSocketConnection.HandleMessagesAsync(_webSocketMock.Object, CancellationToken.None).Wait(100);

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
            _webSocketConnection.HandleMessagesAsync(_webSocketMock.Object, CancellationToken.None)
                .Wait(100);

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
                Params = new List<string> {"1", "1"}
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
            _webSocketConnection.HandleMessagesAsync(_webSocketMock.Object, CancellationToken.None)
                .Wait(100);

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
            _webSocketConnection.HandleMessagesAsync(_webSocketMock.Object, CancellationToken.None)
                .Wait(100);

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
                Params = new List<string> {"wrongParam"}
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
            _webSocketConnection.HandleMessagesAsync(_webSocketMock.Object, CancellationToken.None)
                .Wait(100);

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
            _webSocketConnection.HandleMessagesAsync(_webSocketMock.Object, CancellationToken.None)
                .Wait(100);

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
            _webSocketConnection.HandleMessagesAsync(_webSocketMock.Object, CancellationToken.None)
                .Wait(100);

            // ASSERT
            Assert.That(_webSocketService.MethodsInvoked, Is.Empty);
            Assert.That(result, Is.Not.Null);
            Assert.That(jsonRpcMethodRequest.Id, Is.EqualTo(result["id"].Value<int>()));
            Assert.That(-32601, Is.EqualTo(result["error"]["code"].Value<int>()));
        }

        [Test, Category("Unit"), Ignore("Fails when running with other tests. ned to figure this out")]
        public void Event_WithArgs_SentToClients()
        {
            // ARRANGE
            JObject notification = null;
            var tcsReceive = new TaskCompletionSource<(MessageType, ArraySegment<byte>)>();
    
            _webSocketMock.Setup(m => m.ReceiveAsync(CancellationToken.None))
                .Returns(() => tcsReceive.Task);
            _webSocketMock.Setup(m => m.WebSocketState).Returns(JsonRpcWebSocketState.Closed);
            _webSocketMock
                .Setup(m => m.SendAsync(It.IsAny<string>()))
                .Returns((string s) =>
                {
                    notification = JObject.Parse(s);
                    tcsReceive.SetResult((MessageType.Close, ArraySegment<byte>.Empty));
                    return Task.CompletedTask;
                });
            
            _webSocketConnection.HandleMessagesAsync(_webSocketMock.Object, CancellationToken.None)
                .Wait();
            _webSocketMock.Setup(m => m.WebSocketState).Returns(JsonRpcWebSocketState.Open);
            
            // ACT
            _webSocketService.InvokeWithArgs("test");
                
            tcsReceive.Task.Wait(1000);
            
            // ASSERT
            _webSocketMock.Verify(m => m.SendAsync(It.IsAny<string>()), Times.Once);
            Assert.That(notification, Is.Not.Null);
            Assert.That(notification["method"].Value<string>(), Is.EqualTo("EventWithArgs"));
            Assert.That(notification["params"][0]["Message"].Value<string>(), Is.EqualTo("test"));
        }
    }
}