using System.Collections.Generic;
using Newtonsoft.Json;

namespace JsonRpcNet.Docs
{
    public class JsonRpcDoc
    {
        [JsonProperty("info")]
        public JsonRpcInfo Info { get; }
        [JsonProperty("services")]
        public IList<JsonRpcService> Services { get; } = new List<JsonRpcService>();

        public JsonRpcDoc(JsonRpcInfo info)
        {
            Info = info;
        }
    }
}