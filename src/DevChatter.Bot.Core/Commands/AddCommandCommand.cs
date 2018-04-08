﻿using System;
using System.Collections.Generic;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Extensions;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands
{
    public class AddCommandCommand : BaseCommand
    {
        private readonly IRepository _repository;
        private readonly CommandContainer _allCommands;

        public AddCommandCommand(IRepository repository, CommandContainer allCommands)
            : base(UserRole.Mod)
        {
            _repository = repository;
            _allCommands = allCommands;
            HelpText = "To add a command to the bot use \"!AddCommand Command Text PermissionLevel\" Example: !AddCommand Twitter \"https://twitter.com/DevChatter_\" Everyone";
        }

        public override void Process(IChatClient chatClient, CommandReceivedEventArgs eventArgs)
        {
            try
            {
                // !AddCommand Twitter "https://twitter.com/DevChatter_" Everyone
                SimpleCommand command = eventArgs.Arguments.ToSimpleCommand();

                if (command == null)
                {
                    chatClient.SendMessage("Failed to create command.");
                    chatClient.SendMessage(HelpText);
                    return;
                }

                if (_repository.Single(CommandPolicy.ByCommandText(command.CommandText)) != null)
                {
                    chatClient.SendMessage($"There's already a command using !{command.CommandText}");
                    return;
                }

                chatClient.SendMessage($"Adding a !{command.CommandText} command for {command.RoleRequired}. It will respond with {command.StaticResponse}.");

                _repository.Create(command);
                _allCommands.Add(command);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}