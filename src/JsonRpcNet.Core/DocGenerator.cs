using System;
using System.Linq;
using System.Reflection;
using JsonRpcNet.Attributes;

namespace JsonRpcNet
{
    public static class DocGenerator
    {
        public static JsonRpcDoc GenerateJsonRpcDoc(JsonRpcInfo info)
        {
            var jsonRpcServices = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(t => !t.IsAbstract && typeof(JsonRpcWebSocketService).IsAssignableFrom(t)))
                .ToList();

            var jsonRpcDoc = new JsonRpcDoc(info);
            
            foreach (var jsonRpcService in jsonRpcServices)
            {
                jsonRpcDoc.Services.Add(GenerateJsonRpcServiceDoc(jsonRpcService, jsonRpcDoc));
            }

            return jsonRpcDoc;
        }
        
        public static JsonRpcService GenerateJsonRpcServiceDoc(Type type, JsonRpcDoc jsonRpcDoc)
        {
            var serviceAttribute =
                (JsonRpcServiceAttribute) type.GetCustomAttribute(typeof(JsonRpcServiceAttribute));

            var serviceDoc = new JsonRpcService
            {
                Name = serviceAttribute?.Name ?? type.Name,
                Path = serviceAttribute?.Path ?? type.Name.ToLower(),
                Description = serviceAttribute?.Description ?? string.Empty
            };

            var methodMetaData = MethodHelper.GetRpcMethods(type);
            
            foreach (var (attribute, methodInfo) in methodMetaData)
            {
                var parameters = methodInfo.GetParameters();
                var method = new JsonRpcMethod(methodInfo, parameters)
                {
                    Name = attribute.Name,
                    Description = attribute.Description
                };
                serviceDoc.Methods.Add(method);
            }

            var notificationMetaData = MethodHelper.GetRpcNotifications(type);
            
            foreach (var (attribute, eventInfo) in notificationMetaData)
            {
                var notification = new JsonRpcNotification(eventInfo)
                {
                    Name = attribute.Name,
                    Description = attribute.Description
                };
                serviceDoc.Notifications.Add(notification);
            }
            
            return serviceDoc;
        }
    }
}