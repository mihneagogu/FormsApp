﻿using System;
using System.Collections.Generic;
using static FormsAppTelenav.Databases.Dealer;

namespace FormsAppTelenav.Databases
{
    public enum MessageAction
    {
        AddedAuctionBundle,
        SellAuctionBundle
    }

    public class MessageHandler
    {
        public MessageHandler(MessageAction type, Classes.IMessageHandler handler){
            Type = type;
            Handler = handler;
        }
        public MessageAction Type { get; set; }
        public Classes.IMessageHandler Handler { get; set; }


    }

    public class Dealer
    {


        public Dealer()
        {
            
        }

        public void OnEvent(MessageAction message, List<object> payload){
            foreach (var r in registeredHandlers){
                if (r.Type == message){
                    r.Handler.OnMessageReceived(r.Type, payload);
                }
            }
        }

        public void RegisterMessage(MessageAction message, Classes.IMessageHandler handler){
            registeredHandlers.Add(new MessageHandler(message, handler));
        }

      

        private List<Databases.MessageHandler> registeredHandlers = new List<Databases.MessageHandler>();
    }




}
