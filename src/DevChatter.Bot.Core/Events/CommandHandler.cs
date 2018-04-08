using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Events
{
    public class CommandHandler
    {
        private readonly CommandUsageTracker _usageTracker;
        private readonly CommandContainer _commandMessages;
	    private readonly ICommandResolver _commandResolver;

        public CommandHandler(CommandUsageTracker usageTracker, List<IChatClient> chatClients,
	        CommandContainer commandMessages, ICommandResolver commandResolver)
        {
            _usageTracker = usageTracker;
            _commandMessages = commandMessages;
	        _commandResolver = commandResolver;

	        foreach (var chatClient in chatClients)
            {
                chatClient.OnCommandReceived += CommandReceivedHandler;
            }
        }

        public void CommandReceivedHandler(object sender, CommandReceivedEventArgs e)
        {
	        if (!(sender is IChatClient chatClient)) return;

	        var userDisplayName = e.ChatUser.DisplayName;

	        _usageTracker.PurgeExpiredUserCommandCooldowns(DateTimeOffset.Now);

	        var previousUsage = _usageTracker.GetByUserDisplayName(userDisplayName);
	        if (previousUsage != null)
	        {
		        if (previousUsage.WasUserWarned) return;

		        chatClient.SendMessage($"Whoa {userDisplayName}! Slow down there cowboy!");
		        previousUsage.WasUserWarned = true;
		        return;
	        }

	        var commandType = _commandResolver.CommandFor(e.CommandWord);

	        var botCommand = _commandMessages.FirstOrDefault(c => c.GetType() == commandType && c.ShouldExecute(e.CommandWord));

	        if (botCommand != null)
	        {
		        AttemptToRunCommand(e, botCommand, chatClient);
		        _usageTracker.RecordUsage(new CommandUsage(userDisplayName, DateTimeOffset.Now, false));
	        }
        }

        private void AttemptToRunCommand(CommandReceivedEventArgs e, IBotCommand botCommand, IChatClient chatClient1)
        {
            try
            {
                if (e.ChatUser.CanUserRunCommand(botCommand))
                {
                    botCommand.Process(chatClient1, e);
                }
                else
                {
                    chatClient1.SendMessage(
                        $"Sorry, {e.ChatUser.DisplayName}! You don't have permission to use the !{e.CommandWord} command.");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

    }
}