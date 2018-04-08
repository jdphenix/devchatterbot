﻿using System;
using System.Collections.Generic;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Systems.Chat;
using UnitTests.Fakes;
using Xunit;

namespace UnitTests.Core.Events.CommandHandlerTests
{
    public class CommandReceivedHandlerShould
    {
        [Fact]
        public void CallProcessOnCommandWhenEnabled()
        {
            var fakeCommand = new FakeCommand("FakeCommand", true);
            CommandHandler commandHandler = GetTestCommandHandler(fakeCommand);

            commandHandler.CommandReceivedHandler(new FakeChatClient(), 
                new CommandReceivedEventArgs { CommandWord = "Fake" });

            Assert.True(fakeCommand.ProcessWasCalled);
        }

        [Fact]
        public void NotCallProcessOnCommandWhenDisabled()
        {
            var fakeCommand = new FakeCommand("FakeCommand", false);
            CommandHandler commandHandler = GetTestCommandHandler(fakeCommand);

            commandHandler.CommandReceivedHandler(new FakeChatClient(), 
                new CommandReceivedEventArgs { CommandWord = "Fake" });

            Assert.False(fakeCommand.ProcessWasCalled);
        }

        private static CommandHandler GetTestCommandHandler(FakeCommand fakeCommand)
        {
            var commandUsageTracker = new CommandUsageTracker(new CommandHandlerSettings());
            var chatClients = new List<IChatClient> { new FakeChatClient() };

			// TODO: mock container and resolver
	        var commandMessages = new CommandContainer();
			var commandResolver = new CommandResolver();
			commandMessages.CommandAdded += (sender, args) => commandResolver.AddCommandResolution(args.CommandType);
			commandMessages.Add(fakeCommand);
            var commandHandler = new CommandHandler(commandUsageTracker, chatClients, commandMessages, commandResolver);
            return commandHandler;
        }
    }
}