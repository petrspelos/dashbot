﻿using System;
using Microsoft.Extensions.DependencyInjection;
using DashBot.Abstractions;
using DashBot.Bot.Credentials;
using DashBot.JsonStorage;
using DashBot.Bot;
using Logger;

namespace DashBot.InversionOfControl
{
    public static class Container
    {
        public static IServiceCollection AddDashBotDependencies(this IServiceCollection collection)
            => collection
                .AddSingleton<Random>()
                .AddSingleton<IPersistentStorage, JsonPersistentStorage>()
                .AddSingleton<ICredentials, CredentialsProvider>()
                .AddSingleton<IDiscordBot, DiscordBot>()
                .AddSingleton<ILogger, InMemoryLogger>();
    }
}
