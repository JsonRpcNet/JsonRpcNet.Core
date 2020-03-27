using System;
using Newtonsoft.Json;

namespace JsonRpcNet
{
    public class JsonRpcParameterInfo : JsonRpcTypeInfo
    {
        public JsonRpcParameterInfo(string name, Type type) : base(type)
        {
            Name = name;
        }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; }
    }
}