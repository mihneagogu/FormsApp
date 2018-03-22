using System;
using System.Collections.Generic;

namespace FormsAppTelenav.Classes
{
    public interface IMessageHandler
    {
        int OnMessageReceived(Databases.MessageAction message, List<object> payload);
    }
}
