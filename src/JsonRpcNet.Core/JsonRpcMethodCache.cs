using System;
using System.Collections.Generic;

namespace JsonRpcNet
{
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