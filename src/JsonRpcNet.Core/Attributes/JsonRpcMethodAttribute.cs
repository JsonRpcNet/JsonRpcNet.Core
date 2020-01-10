﻿namespace JsonRpcNet.Attributes
{
	[System.AttributeUsage(System.AttributeTargets.Method)]
	public class JsonRpcMethodAttribute : System.Attribute
	{
		public string Name { get; }
		public string Description { get; set; }
		
		public JsonRpcMethodAttribute(string name)
		{
			Name = name;
		}
	}
}
