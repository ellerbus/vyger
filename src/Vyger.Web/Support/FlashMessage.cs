namespace Vyger.Web
{
    //https://github.com/ellerbus/vyger/blob/xml/vyger/Controllers/_BaseController.cs
    //https://github.com/ellerbus/vyger/blob/xml/vyger/Views/Shared/_FlashMessage.cshtml

    public class FlashMessage
    {
        public FlashMessage(string type, string message)
        {
            Type = type;
            Message = message;
        }

        public string Type { get; }
        public string Message { get; }
    }
}