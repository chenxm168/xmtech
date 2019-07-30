using fastJSON;

namespace HF.BC.JSON
{
    public class JSONConverter<T>
    {
        public JSONConverter()
        {
            fastJSON.JSON.Parameters.UseExtensions = false;
        }

        public string ObjectToString(T t)
        {
            return fastJSON.JSON.ToJSON(t);
        }

        public T StringToObject(string s)
        {
            return fastJSON.JSON.ToObject<T>(s);
        }
    }
}
