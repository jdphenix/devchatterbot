using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public class PermissionCommand : BaseCommand
    {
        private readonly CommandContainer _allCommands;
	    private readonly ICommandResolver _commandResolver;

        public PermissionCommand(CommandContainer allCommands, ICommandResolver commandResolver)
            : base(UserRole.Everyone)
        {
	        _allCommands = allCommands;
	        _commandResolver = commandResolver;
        }

        public override void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {/*
            var listOfCommands = _allCommands.Where(x => eventArgs.ChatUser.CanUserRunCommand(x)).Select(x => $"!{x.PrimaryCommandText}").ToList();
			*/
	        var listOfCommands = _commandResolver.CommandWords;
	        // TODO: Fix command permission search, new commadn resolver has no knowledge of permission.
			string stringOfCommands = string.Join(", ", listOfCommands);
            chatClient.SendMessage($"These are the commands that {eventArgs.ChatUser.DisplayName} is allowed to run: ({stringOfCommands})");
        }
    }
}