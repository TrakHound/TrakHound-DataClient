﻿// Copyright (c) 2017 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.

using NLog;
using System;
using System.ServiceModel;
using TrakHound.Api.v2.WCF;

namespace TrakHound.DataClient.SystemTray.Messages
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class MessageServer : IMessage
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private static IMessageCallback callback;

        public MessageServer()
        {
            try
            {
                callback = OperationContext.Current.GetCallbackChannel<IMessageCallback>();
            }
            catch (Exception ex)
            {
                log.Error("Error during MessageServer Start");
                log.Trace(ex);
            }
        }

        public object SendData(Message data)
        {
            if (data != null && data.Text != null)
            {
                var notifyIcon = DataClientSystemTray.NotifyIcon;

                notifyIcon.BalloonTipTitle = "TrakHound DataClient";
                notifyIcon.BalloonTipText = data.Text;
                notifyIcon.Icon = Properties.Resources.dataclient;
                notifyIcon.ShowBalloonTip(5000);
            }

            return "Data Sent Successfully!";
        }

        public static void SendCallback(Message data)
        {
            try
            {
                callback.OnCallback(data);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

    }
}
