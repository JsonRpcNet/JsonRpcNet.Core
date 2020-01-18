using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JsonRpcNet.Attributes;

namespace JsonRpcNet
{
    internal static class MethodHelper
    {
        public static IReadOnlyList<(JsonRpcMethodAttribute Attribute, MethodInfo MethodInfo)> GetRpcMethods(Type type)
        {
            return type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.GetCustomAttribute(typeof(JsonRpcMethodAttribute)) != null)
                .Select(m => ((JsonRpcMethodAttribute) m.GetCustomAttribute(typeof(JsonRpcMethodAttribute)), m))
                .ToList();
        }

        public static IReadOnlyList<(JsonRpcNotificationAttribute Attribute, EventInfo EventInfo)> GetRpcNotifications(Type type)
        {
            return type.GetEvents(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.GetCustomAttribute(typeof(JsonRpcNotificationAttribute)) != null)
                .Select(m => ((JsonRpcNotificationAttribute) m.GetCustomAttribute(typeof(JsonRpcNotificationAttribute)),
                    m))
                .ToList();
        }

        public static Dictionary<string, MethodInfoWithPermissions> CreateMethodCache(Type type)
        {
            var methods = GetRpcMethods(type);
            var methodCache = new Dictionary<string, MethodInfoWithPermissions>();
            foreach (var (attribute, methodInfo) in methods)
            {
                var name = string.IsNullOrEmpty(attribute.Name) ? methodInfo.Name : attribute.Name;
                var authorizeAttribute = methodInfo.GetCustomAttribute<AuthorizeAttribute>();
                
                var methodInvoker = authorizeAttribute == null
                    ? new MethodInfoWithPermissions(methodInfo, attribute)
                    : new MethodInfoWithPermissions(methodInfo, attribute, authorizeAttribute);
                methodCache[name] = methodInvoker;
            }

            return methodCache;
        }
    }
}