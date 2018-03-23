using System;
using System.Collections.Generic;
using FormsAppTelenav.Databases;

namespace FormsAppTelenav.Classes
{
    public interface IMessageHandler
    {
        DealerResponse OnMessageReceived(Databases.MessageAction message, List<object> payload);
    }
}
