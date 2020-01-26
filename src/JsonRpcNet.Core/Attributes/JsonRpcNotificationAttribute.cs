namespace JsonRpcNet.Attributes
{
    public class JsonRpcNotificationAttribute : System.Attribute
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}