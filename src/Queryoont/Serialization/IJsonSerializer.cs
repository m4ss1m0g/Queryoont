namespace Queryoont.Serialization
{
    public interface IJsonSerializer
    {
        string Serialize(object o);
        
        T Deserialize<T>(string json);
    }
}