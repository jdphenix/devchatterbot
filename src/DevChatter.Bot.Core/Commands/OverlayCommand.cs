using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public class OverlayCommand : IBotCommand
    {
        public OverlayCommand(UserRole roleRequired, string commandText)
        {
            RoleRequired = roleRequired;
            CommandText = commandText;
        }

        public UserRole RoleRequired { get; }
        public string CommandText { get; }

        public virtual void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
        }
    }

    public class HypeCommand : OverlayCommand
    {
        public HypeCommand() : base(UserRole.Subscriber, "Hype")
        {
        }

        public override void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            chatClient.SendMessage("You triggered the hype command... Hype Hype!");
        }
    }
}