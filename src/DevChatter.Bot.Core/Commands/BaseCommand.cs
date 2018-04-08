using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public abstract class BaseCommand : IBotCommand
    {
        private readonly bool _isEnabled;
        public UserRole RoleRequired { get; }
        public string HelpText { get; protected set; }

        protected BaseCommand(UserRole roleRequired)
            : this(roleRequired, true)
        {
        }

        protected BaseCommand(UserRole roleRequired, bool isEnabled)
        {
            RoleRequired = roleRequired;
            _isEnabled = isEnabled;
        }


        public bool ShouldExecute(string commandText) => _isEnabled;

        public abstract void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs);
    }
}