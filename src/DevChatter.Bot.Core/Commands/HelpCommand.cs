using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public class HelpCommand : BaseCommand
    {
        private readonly CommandContainer _allCommands;
	    private readonly ICommandResolver _commandResolver;

	    public HelpCommand(CommandContainer allCommands, ICommandResolver commandResolver)
            : base(UserRole.Everyone)
        {
            _allCommands = allCommands;
	        _commandResolver = commandResolver;
	        HelpText = "I think you figured this out already...";
        }

        public override void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            if (eventArgs.Arguments.Count == 0)
            {
                ShowAvailableCommands(chatClient, eventArgs.ChatUser);
                return;
            }

            string argOne = eventArgs?.Arguments?.ElementAtOrDefault(0);

            if (argOne == "?")
            {
                chatClient.SendMessage($"Use !help to see available commands. To request help for a specific command just type !help [commandname] example: !help hangman");
                return;
            }

            if (argOne == "↑, ↑, ↓, ↓, ←, →, ←, →, B, A, start, select")
            {
                chatClient.SendMessage("Please be sure to drink your ovaltine.");
            }

            IBotCommand requestedCommand = _allCommands.SingleOrDefault(x => x.ShouldExecute(argOne));

            if (requestedCommand != null)
            {
                chatClient.SendMessage(requestedCommand.HelpText);
            }
        }

        private void ShowAvailableCommands(IChatClient chatClient, ChatUser chatUser)
        {
			// TODO: Fix command permission search, new commadn resolver has no knowledge of permission.

            var commands = _commandResolver.CommandWords;
			
	        string stringOfCommands = string.Join(", ", commands);

            string message = $"These are the commands that {chatUser.DisplayName} is allowed to run: ({stringOfCommands})";
            chatClient.SendMessage(message);

        }
    }
}