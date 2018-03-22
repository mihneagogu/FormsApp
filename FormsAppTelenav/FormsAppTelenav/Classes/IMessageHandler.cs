using System;
using System.Collections.Generic;

namespace FormsAppTelenav.Classes
{
    public interface IMessageHandler
    {
        void OnMessageReceived(Databases.MessageAction message, List<object> payload);
    }
}
