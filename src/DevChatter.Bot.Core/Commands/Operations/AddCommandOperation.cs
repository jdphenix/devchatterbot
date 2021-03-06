using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Data.Specifications;
using DevChatter.Bot.Core.Events.Args;

namespace DevChatter.Bot.Core.Commands.Operations
{
    public class AddCommandOperation : BaseCommandOperation
    {
        private readonly IRepository _repository;
        private readonly IList<IBotCommand> _allCommands;

        public AddCommandOperation(IRepository repository, IList<IBotCommand> allCommands)
        {
            _repository = repository;
            _allCommands = allCommands;
        }

        public override List<string> OperandWords { get; } = new List<string> { "add" };
        public override string TryToExecute(CommandReceivedEventArgs eventArgs)
        {
            string commandWord = eventArgs?.Arguments?.ElementAtOrDefault(1);
            string staticResponse = eventArgs?.Arguments?.ElementAtOrDefault(2);
            string roleText = eventArgs?.Arguments?.ElementAtOrDefault(3);

            ChatUser chatUser = eventArgs.ChatUser;
            try
            {
                if (!chatUser.CanUserRunCommand(UserRole.Mod))
                {
                    return "You need to be a moderator to add a command.";
                }

                if (!Enum.TryParse(roleText, true, out UserRole role))
                {
                    role = UserRole.Everyone;
                }

                SimpleCommand command = new SimpleCommand(commandWord, staticResponse, role);

                if (_repository.Single(CommandPolicy.ByCommandText(command.CommandText)) != null)
                {
                    return $"There's already a command using !{command.CommandText}";
                }

                _repository.Create(command);
                _allCommands.Add(command);

                return $"Adding a !{command.CommandText} command for {command.RoleRequired}. It will respond with {command.StaticResponse}.";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "";
            }
        }
    }
}
