namespace JsonRpcNet.Attributes
{
    public class JsonRpcNotificationAttribute : System.Attribute
    {
        public string Name { get; }
		
        public string Description { get; set; }

        public JsonRpcNotificationAttribute(string name)
        {
            Name = name;
        }
    }
}