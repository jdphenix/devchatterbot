using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Overlay;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public class HypeCommand : OverlayCommand
    {
        public HypeCommand(IDisplayEventTrigger displayEventTrigger) : base(displayEventTrigger, UserRole.Subscriber, "Hype")
        {
        }

        public override void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            chatClient.SendMessage("You triggered the hype command... Hype Hype!");
            DisplayEventTrigger.ShowImage();
        }
    }
}