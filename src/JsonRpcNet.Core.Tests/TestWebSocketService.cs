using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JsonRpcNet.Attributes;

namespace JsonRpcNet.Core.Tests
{
    [JsonRpcService("/test", Description = "test", Name = "test")]
    public class TestWebSocketService : JsonRpcWebSocketService
    {
        [JsonRpcNotification(Description = "test", Name = "EventWithArgs")]
        private event EventHandler<TestEventArgs> TestEventWithArgs;
        
        public IList<string> MethodsInvoked { get; } = new List<string>();
        
        public void InvokeWithArgs(string message)
        {
            TestEventWithArgs?.Invoke(this, new TestEventArgs {Message = message});
        }
        
        [JsonRpcMethod(Description = "test")]
        [JsonRpcAuthorize(Roles = "admins", Users = "gottscj")]
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
}