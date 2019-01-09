namespace vyger
{
    public enum FlashMessageType
    {
        Success,
        Info,
        Warning,
        Danger
    }

    public class FlashMessage
    {
        public FlashMessage(FlashMessageType type, string message)
        {
            Type = type;
            Message = message;
        }

        public FlashMessageType Type { get; private set; }

        public string Message { get; private set; }
    }
}