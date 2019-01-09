using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace Vyger.Web
{
    public static class TempDataExtensions
    {
        public static void Put<T>(this ITempDataDictionary tempData, T value) where T : class
        {
            string key = typeof(T).FullName;

            tempData[key] = JsonConvert.SerializeObject(value);
        }

        public static T Get<T>(this ITempDataDictionary tempData) where T : class
        {
            string key = typeof(T).FullName;

            object o = null;

            tempData.TryGetValue(key, out o);

            return o == null ? null : JsonConvert.DeserializeObject<T>((string)o);
        }
    }
}