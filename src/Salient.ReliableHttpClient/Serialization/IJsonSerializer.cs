namespace Salient.ReliableHttpClient.Serialization
{
    public interface IJsonSerializer
    {
        string SerializeObject(object value);
        T DeserializeObject<T>(string json);
    }
}