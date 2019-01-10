using Newtonsoft.Json.Converters;

namespace Vyger.Common
{
    public class YmdDateConverter : IsoDateTimeConverter
    {
        public YmdDateConverter()
        {
            base.DateTimeFormat = "yyyy-MM-dd";
        }
    }
}