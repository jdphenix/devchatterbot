﻿using System;
using DevChatter.Bot.Core.Attributes;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Automation
{
    [RegistrationNotAllowed]
    public class DelayedMessageAction : IIntervalAction
    {
        private readonly string _message;
        private readonly IChatClient _chatClient;
        private DateTime _nextRunTime;

        public DelayedMessageAction(int delayInSeconds, string message, IChatClient chatClient)
        {
            _message = message;
            _chatClient = chatClient;
            _nextRunTime = DateTime.Now.AddSeconds(delayInSeconds);
        }

        public bool IsTimeToRun() => DateTime.Now > _nextRunTime;

        public void Invoke()
        {
            _chatClient.SendMessage(_message);
            _nextRunTime = DateTime.MaxValue;
        }
    }
}
