using System.Reflection;
using System.Threading.Tasks;
using JsonRpcNet.Attributes;

namespace JsonRpcNet
{
	internal class MethodInfoWithPermissions
	{
		public FastMethodInfo MethodInfo { get; }
		public string Roles { get; }
		
		public string Users { get; }

		public string RpcName { get; }

		public MethodInfoWithPermissions((JsonRpcMethodAttribute attribute, MethodInfo methodInfo) methodInfoTuple)
			: this(methodInfoTuple.methodInfo, methodInfoTuple.attribute)
		{
			
		}
		public MethodInfoWithPermissions(MethodInfo methodInfo, JsonRpcMethodAttribute jsonRpcMethodAttribute) : this(
			methodInfo, jsonRpcMethodAttribute, null)
		{
		}

		public MethodInfoWithPermissions(MethodInfo methodInfo, JsonRpcMethodAttribute jsonRpcMethodAttribute,
			AuthorizeAttribute authorizeAttribute)
		{
			MethodInfo = new FastMethodInfo(methodInfo);
			Roles = authorizeAttribute?.Roles;
			Users = authorizeAttribute?.Users;
			RpcName = jsonRpcMethodAttribute?.Name ?? methodInfo.Name;
		}

		public Task<object> InvokeAsync(object instance, object[] parameters, string role)
		{
			if (string.IsNullOrEmpty(role))
			{
				return MethodInfo.InvokeAsync(instance, parameters);
			}
			
			if (Roles.Contains(role))
			{
				return MethodInfo.InvokeAsync(instance, parameters);
			}

			return null;
		}
	}
}