using System;
using DevChatter.Bot.Core.Attributes;
using DevChatter.Bot.Core.Automation;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Games.RockPaperScissors
{
    [RegistrationNotAllowed]
    public class RockPaperScissorsEndGame : IIntervalAction
    {
        private readonly RockPaperScissorsGame _rockPaperScissorsGame;
        private readonly IChatClient _chatClient;

        private DateTime _timeOfNextRun;

        public RockPaperScissorsEndGame(int intervalInSeconds,
            RockPaperScissorsGame rockPaperScissorsGame,
            IChatClient chatClient)
        {
            _rockPaperScissorsGame = rockPaperScissorsGame;
            _chatClient = chatClient;
            _timeOfNextRun = DateTime.Now.AddSeconds(intervalInSeconds);
        }

        public bool IsTimeToRun() => DateTime.Now > _timeOfNextRun;

        public void Invoke()
        {
            _rockPaperScissorsGame.PlayMatch(_chatClient);
            _timeOfNextRun = DateTime.MaxValue;
        }
    }
}
