﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JsonRpcNet
{
    public class JsonRpcTypeInfo
    {
        [JsonIgnore]
        public Type Type { get; }

        public JsonRpcTypeInfo(Type type)
        {
            if (typeof(Task).IsAssignableFrom(type) && type.IsGenericType)
            {
                Type = type.GetGenericArguments().Single();
            }
            else
            {
                Type = type; 
            }
            
            var schemaType = JsonTypeHelper.GetSchemaTypeString(Type);
            Schema = new Dictionary<string, object>
            {
                ["type"] = schemaType
            };

            if (schemaType == "object")
            {
                Schema["$ref"] = $"#/definitions/{Type.Name}";
            }
            else if (schemaType == "array")
            {

                var arrayType = Type.IsArray ? Type.GetElementType() : Type.GetGenericArguments().Single();
                if (arrayType == null)
                {
                    throw new InvalidOperationException($"Could not get element type for given type: {type}");
                }

                Schema["items"] =
                    new Dictionary<string, object>
                    {
                        ["$ref"] = $"#/definitions/{arrayType.Name}"
                    };
            }
        }

        [JsonProperty("required")]
        public bool Required { get; } = true;

        [JsonProperty("schema", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> Schema { get; set; }
    }
}