namespace JsonRpcNet.Attributes
{
	[System.AttributeUsage(System.AttributeTargets.Method)]
	public class JsonRpcMethodAttribute : System.Attribute
	{
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
	}
}
