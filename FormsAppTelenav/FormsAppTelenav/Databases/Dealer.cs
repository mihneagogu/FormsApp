using System;
using System.Collections.Generic;
using static FormsAppTelenav.Databases.Dealer;

namespace FormsAppTelenav.Databases
{
    public enum MessageAction
    {
        AddedAuctionBundle
    }

    public class MessageHandler
    {

        public MessageAction Type { get; set; }
        public Classes.IMessageHandler Handler { get; set; }


    }

    public class Dealer
    {


        public Dealer()
        {
            
        }

        public void OnEvent(MessageAction message, object payload){
            
        }

        public void RegisterMessage(MessageAction message){
            
        }

      

        private List<Databases.MessageHandler> registeredHandlers = new List<Databases.MessageHandler>();
    }




}
