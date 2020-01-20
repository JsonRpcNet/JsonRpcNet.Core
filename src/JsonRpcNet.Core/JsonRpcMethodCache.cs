using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JsonRpcNet.Attributes;

namespace JsonRpcNet
{
	internal class JsonRpcNotificationCache
	{
		private readonly
			Dictionary<Type, List<(MethodInfo AddMethod, Delegate Delegate)>>
			_notificationCache =
				new Dictionary<Type, List<(MethodInfo AddMethod, Delegate Delegate
					)>>();

		public List<(MethodInfo AddMethod, Delegate Delegate)>
			Get(JsonRpcWebSocketService service)
		{
			var type = service.GetType();
			if (_notificationCache.TryGetValue(type, out var eventTuples))
			{
				return eventTuples;
			}

			var events = type
				.GetEvents(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				.Where(e => e.GetCustomAttribute(typeof(JsonRpcNotificationAttribute)) != null);
			
			var list = new List<(MethodInfo, Delegate)>();
			foreach (var e in events)
			{
				var attribute = (JsonRpcNotificationAttribute)e.GetCustomAttribute(typeof(JsonRpcNotificationAttribute));
				var name = string.IsNullOrEmpty(attribute.Name) ? e.Name : attribute.Name;
				var addMethod = e.GetAddMethod(true);
				var notificationDelegate = EventProxy.Create(e, args => service.InvokeNotification(name, args));
				list.Add((addMethod, notificationDelegate));
			}
			_notificationCache[type] = list;
			
			return list;
		}
	}
	internal class JsonRpcMethodCache
	{
		private readonly Dictionary<Type, Dictionary<string, MethodInfoWithPermissions>> _typeMethodCache = new Dictionary<Type, Dictionary<string, MethodInfoWithPermissions>>();

		public Dictionary<string, MethodInfoWithPermissions> Get(JsonRpcWebSocketService service)
		{
			var type = service.GetType();
			if (_typeMethodCache.TryGetValue(type, out var methodCache))
			{
				return methodCache;
			}

			methodCache = MethodHelper.CreateMethodCache(type);
			_typeMethodCache.Add(type, methodCache);

			return methodCache;
		}
	}
}