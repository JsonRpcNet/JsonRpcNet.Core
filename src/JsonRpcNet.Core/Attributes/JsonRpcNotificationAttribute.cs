namespace JsonRpcNet.Attributes
{
    public class JsonRpcNotificationAttribute : System.Attribute
    {
        public string Name { get; set; }
		
        public string Description { get; set; }
    }
}