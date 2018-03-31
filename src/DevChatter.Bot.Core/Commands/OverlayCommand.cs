using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Overlay;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public abstract class OverlayCommand : IBotCommand
    {
        protected readonly IDisplayEventTrigger DisplayEventTrigger;

        protected OverlayCommand(IDisplayEventTrigger displayEventTrigger, UserRole roleRequired, string commandText)
        {
            DisplayEventTrigger = displayEventTrigger;
            RoleRequired = roleRequired;
            CommandText = commandText;
        }

        public UserRole RoleRequired { get; }
        public string CommandText { get; }

        public virtual void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
        }
    }
}