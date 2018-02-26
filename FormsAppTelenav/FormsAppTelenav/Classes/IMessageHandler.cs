using System;

namespace FormsAppTelenav.Classes
{
    public interface IMessageHandler
    {
        void OnMessageReceived(Databases.MessageAction message, object payload);
    }
}
