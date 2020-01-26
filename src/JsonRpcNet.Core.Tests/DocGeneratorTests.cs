using System.Linq;
using Castle.Core.Internal;
using JsonRpcNet.Attributes;
using NUnit.Framework;

namespace JsonRpcNet.Core.Tests
{
    [TestFixture]
    public class DocGeneratorTests
    {
        [Test, Category("Unit")]
        public void GetRpcMethods_MethodsExist_ReturnsMethods()
        {
            // ARRANGE
            
            // ACT
            var methods = 
                MethodHelper.GetRpcMethods(typeof(TestWebSocketService));
            
            // ASSERT
            Assert.That(methods.Count, Is.EqualTo(4));
            Assert.That(methods[0].Attribute.Name, Is.EqualTo(""));
            Assert.That(methods[0].Attribute.Description, Is.EqualTo("test"));
            Assert.That(methods[0].MethodInfo.Name, Is.EqualTo("TestMethod"));
            Assert.That(methods[0].MethodInfo.GetParameters(), Has.Length.EqualTo(0));
            
            Assert.That(methods[1].Attribute.Name, Is.EqualTo(""));
            Assert.That(methods[1].Attribute.Description, Is.EqualTo(""));
            Assert.That(methods[1].MethodInfo.Name, Is.EqualTo("TestMethodWithParams"));
            Assert.That(methods[1].MethodInfo.GetParameters(), Has.Length.EqualTo(2));
            
            Assert.That(methods[2].Attribute.Name, Is.EqualTo(""));
            Assert.That(methods[2].Attribute.Description, Is.EqualTo(""));
            Assert.That(methods[2].MethodInfo.Name, Is.EqualTo("Throws"));
            Assert.That(methods[2].MethodInfo.GetParameters(), Has.Length.EqualTo(0));
            
            Assert.That(methods[3].Attribute.Name, Is.EqualTo("TestMethod23"));
            Assert.That(methods[3].Attribute.Description, Is.EqualTo(""));
            Assert.That(methods[3].MethodInfo.Name, Is.EqualTo("TestMethod2"));
            Assert.That(methods[3].MethodInfo.GetParameters(), Has.Length.EqualTo(0));
        }

        [Test, Category("Unit")]
        public void GetNotifications_NotificationsExist_Returned()
        {
            // ARRANGE

            // ACT
            var notifications = 
                MethodHelper.GetRpcNotifications(typeof(TestWebSocketService));
            
            // ASSERT
            Assert.That(notifications, Has.Count.EqualTo(1));
            Assert.That(notifications[0].Attribute.Description, Is.EqualTo("test"));
            Assert.That(notifications[0].Attribute.Name, Is.EqualTo("EventWithArgs"));
            Assert.That(notifications[0].EventInfo.Name, Is.EqualTo("TestEventWithArgs"));

        }

        [Test, Category("Unit")]
        public void GenerateJsonRpcDoc_Valid_CreatesDoc()
        {
            // ARRANGE
            var info = new JsonRpcInfo
            {
                Contact = new JsonRpcContact
                {
                    Email = "test@test.dk",
                    Name = "test",
                    Url = "www.test.dk"
                },
                Description = "test",
                Title = "test",
                Version = "test",
                JsonRpcApiEndpoint = "/test"
            };

            var attribute = typeof(TestWebSocketService)
                .GetAttribute<JsonRpcServiceAttribute>();
            // ACT
            var doc = DocGenerator.GenerateJsonRpcDoc(info);
            
            // ASSERT
            Assert.That(doc.Info.Description, Is.EqualTo(info.Description));
            Assert.That(doc.Info.Title, Is.EqualTo(info.Title));
            Assert.That(doc.Info.Version, Is.EqualTo(info.Version));
            Assert.That(doc.Info.JsonRpcApiEndpoint, Is.EqualTo(info.JsonRpcApiEndpoint));
            Assert.That(doc.Info.Contact.Email, Is.EqualTo(info.Contact.Email));
            Assert.That(doc.Info.Contact.Name, Is.EqualTo(info.Contact.Name));
            Assert.That(doc.Info.Contact.Url, Is.EqualTo(info.Contact.Url));
            Assert.That(doc.Services, Has.Count.EqualTo(1));
            var service = doc.Services.First();
            Assert.That(service.Description, Is.EqualTo(attribute.Description));
            Assert.That(service.Name, Is.EqualTo(attribute.Name));
            Assert.That(service.Path, Is.EqualTo(attribute.Path));
            Assert.That(service.Notifications, Has.Count.EqualTo(1));
            var notification = service.Notifications.First();
            
            Assert.That(notification.Name, Is.EqualTo("EventWithArgs"));
            Assert.That(notification.Description, Is.EqualTo("test"));
            Assert.That(notification.Parameters, Has.Count.EqualTo(1));
            var parameters = notification.Parameters.First();
            
            Assert.That(parameters.Name, Is.EqualTo(typeof(TestEventArgs).Name));
            Assert.That(parameters.Required, Is.True);
            Assert.That(parameters.Type, Is.EqualTo(typeof(TestEventArgs)));
            var schema = parameters.Schema;
            
            Assert.That(schema, Contains.Key("type"));
            Assert.That(schema["type"], Is.EqualTo("object"));
            Assert.That(schema, Contains.Key("$ref"));
            Assert.That(schema["$ref"], Is.EqualTo("#/definitions/TestEventArgs"));


        }
    }
}