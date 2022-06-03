// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BoysheO.Extensions.Logging
{
    internal class StaticFilterOptionsMonitor : IOptionsMonitor<LoggerFilterOptions>
    {
        public StaticFilterOptionsMonitor(LoggerFilterOptions currentValue)
        {
            CurrentValue = currentValue;
        }

        public IDisposable OnChange(Action<LoggerFilterOptions, string> listener) => null;

        public LoggerFilterOptions Get(string name) => CurrentValue;

        public LoggerFilterOptions CurrentValue { get; }
    }
}
