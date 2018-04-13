﻿using System;
using DevChatter.Bot.Core.Events;
using DevChatter.Bot.Core.Util;

namespace DevChatter.Bot.Core.Automation
{
    public class CurrencyUpdate : IIntervalAction
    {
        private readonly int _intervalSettings;
        private readonly ICurrencyGenerator _currencyGenerator;
        private readonly IClock _clock;
        private DateTime _nextRunTime;

        public CurrencyUpdate(IntervalSettings intervalSettings, ICurrencyGenerator currencyGenerator, IClock clock)
        {
            _intervalSettings = intervalSettings.MinutesInterval;
            _currencyGenerator = currencyGenerator;
            _clock = clock;
            SetNextRunTime();
        }

        public bool IsTimeToRun() => _clock.Now >= _nextRunTime;

        public void Invoke()
        {
            _currencyGenerator.UpdateCurrency();
            SetNextRunTime();
        }

        private void SetNextRunTime()
        {
            _nextRunTime = _clock.Now.AddMinutes(_intervalSettings);
        }
    }
}
