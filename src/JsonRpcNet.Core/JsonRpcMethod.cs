using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace JsonRpcNet
{
    public class JsonRpcMethod
    {
        public JsonRpcMethod(MethodInfo methodInfo, IEnumerable<ParameterInfo> parameters)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }
            Name = methodInfo.Name;

            Response = new JsonRpcResponseInfo(methodInfo.ReturnType);
            Parameters = parameters
                .Select(p => new JsonRpcParameterInfo(p.Name, p.ParameterType)).ToList();
        }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;

        [JsonProperty("response")]
        public JsonRpcResponseInfo Response { get; }
        
        [JsonProperty("params")]
        public IList<JsonRpcParameterInfo> Parameters { get; }
    }
}