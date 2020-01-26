using System;

namespace JsonRpcNet
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class JsonRpcAuthorizeAttribute : Attribute
    {
        public string Roles { get; set; } = string.Empty;

        public string Users { get; set; } = string.Empty;
    }
}