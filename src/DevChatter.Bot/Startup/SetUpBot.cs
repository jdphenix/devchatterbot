using System;
using Autofac;
using DevChatter.Bot.Core;
using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Data;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Games.Hangman;
using DevChatter.Bot.Core.Games.RockPaperScissors;
using DevChatter.Bot.Core.Systems.Streaming;
using DevChatter.Bot.Infra.Twitch;
using DevChatter.Bot.Infra.Twitch.Events;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DevChatter.Bot.Core.Attributes;
using DevChatter.Bot.Core.Commands.Trackers;
using DevChatter.Bot.Core.Util;
using TwitchLib;

namespace DevChatter.Bot.Startup
{
    public static class SetUpBot
    {
        private static bool CommandFilterPredicate(Type type) => type.IsAssignableTo<IBotCommand>() &&
                                                                 type.Name.EndsWith("Command");

        public static IContainer NewBotDepedencyContainer(BotConfiguration botConfiguration)
        {
            var repository = SetUpDatabase.SetUpRepository(botConfiguration.DatabaseConnectionString);
            var container = BuildContainer(botConfiguration, repository);

            WireUpAliasNotifications(container);

            return container;
        }

        private static IContainer BuildContainer(BotConfiguration botConfiguration, IRepository repository)
        {
            var referencedAssemblies = Assembly
                .GetExecutingAssembly()
                .GetReferencedAssemblies()
                .Select(Assembly.Load)
                .ToArray();

            var builder = new ContainerBuilder();

            RegisterServices(referencedAssemblies, builder);
            RegisterCommands(referencedAssemblies, builder, repository);
            RegisterOverrides(botConfiguration, builder, repository);

            var container = builder.Build();
            return container;
        }

        private static void RegisterServices(Assembly[] assemblies, ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(assemblies)
                .Where(type => !CommandFilterPredicate(type))
                .Where(type => type.GetCustomAttribute<RegistrationNotAllowedAttribute>() == null)
                .AsSelf()
                .AsImplementedInterfaces()
                .PreserveExistingDefaults()
                .SingleInstance();
        }

        private static void RegisterCommands(Assembly[] assemblies, ContainerBuilder builder, IRepository repository)
        {
            builder
                .RegisterAssemblyTypes(assemblies)
                .Where(CommandFilterPredicate)
                .Except<BaseCommand>()
                .AsImplementedInterfaces()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .SingleInstance();


            var simpleCommands = repository.List<SimpleCommand>();
            foreach (var command in simpleCommands)
            {
                builder.Register(ctx => command).AsImplementedInterfaces().SingleInstance();
            }

            builder.Register(ctx => new CommandList(ctx.Resolve<IList<IBotCommand>>()))
                .AsSelf()
                .SingleInstance();
        }

        private static void RegisterOverrides(BotConfiguration botConfiguration, ContainerBuilder builder,
            IRepository repository)
        {
            builder.Register(ctx => botConfiguration.CommandHandlerSettings).AsSelf().SingleInstance();
            builder.Register(ctx => botConfiguration.TwitchClientSettings).AsSelf().SingleInstance();
            builder.Register(ctx => botConfiguration.IntervalSettings).AsSelf().SingleInstance();

            builder.Register(ctx => new TwitchAPI(botConfiguration.TwitchClientSettings.TwitchClientId))
                .AsImplementedInterfaces();

            builder.Register(ctx => repository).AsImplementedInterfaces().SingleInstance();
            builder.Register(ctx => new CurrencyUpdate(botConfiguration.IntervalSettings,
                    ctx.Resolve<ICurrencyGenerator>(),
                    ctx.Resolve<IClock>()))
                .AsImplementedInterfaces()
                .SingleInstance();
        }

        private static void WireUpAliasNotifications(IContainer container)
        {
            var commandList = container.Resolve<CommandList>();

            AliasCommand aliasCommand = commandList.OfType<AliasCommand>().SingleOrDefault();

            if (aliasCommand != null)
            {
                foreach (var command in commandList.OfType<BaseCommand>())
                {
                    aliasCommand.CommandAliasModified += (s, e) => command.NotifyWordsModified();
                }
            }
        }
    }
}
