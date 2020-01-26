using System;

namespace JsonRpcNet.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class JsonRpcServiceAttribute : System.Attribute
    {
        public string Path { get; }
        public string Description { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
		
        public JsonRpcServiceAttribute(string path)
        {
            if (!path.StartsWith("/"))
            {
                path = "/" + path;
            }

            Path = path;
        }
    }
}