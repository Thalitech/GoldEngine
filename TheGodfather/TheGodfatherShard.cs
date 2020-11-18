﻿#region USING_DIRECTIVES
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity; using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Net.WebSocket;
using DSharpPlus.VoiceNext;

using Microsoft.Extensions.DependencyInjection;
using Serilog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using TheGodfather.Common;
using TheGodfather.Common.Converters;
using TheGodfather.Database;
using TheGodfather.Extensions;
using TheGodfather.Modules.Administration.Services;
using TheGodfather.Modules.Search.Services;
#endregion

namespace TheGodfather
{
    public sealed class TheGodfatherShard
    {
        public static IReadOnlyList<(string Name, Command Command)> Commands;

        public static void UpdateCommandList(CommandsNextExtension cnext)
        {
            Commands = cnext.GetAllRegisteredCommands()
                .Where(cmd => cmd.Parent is null)
                .SelectMany(cmd => cmd.Aliases.Select(alias => (alias, cmd)).Concat(new[] { (cmd.Name, cmd) }))
                .ToList();
        }


        public int Id { get; }
        public DiscordClient Client { get; private set; }
        public CommandsNextExtension CNext { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }
        public VoiceNextExtension Voice { get; private set; }
        public SharedData SharedData { get; private set; }
        public DatabaseContextBuilder Database { get; private set; }

        public bool IsListening => this.SharedData.ListeningStatus;


        public TheGodfatherShard(int sid, DatabaseContextBuilder dbb, SharedData shared)
        {
            this.Id = sid;
            this.Database = dbb;
            this.SharedData = shared;
        }


        public async Task StartAsync()
            => await this.Client.ConnectAsync();

        public async Task DisposeAsync()
        {
            await this.Client.DisconnectAsync();
            this.Client.Dispose();
        }

        #region SETUP_FUNCTIONS
        public void Initialize()
        {
            this.SetupClient();
            this.SetupCommands();
            this.SetupInteractivity();
            this.SetupVoice();

            AsyncExecutionManager.RegisterEventListeners(this.Client, this);
        }

        private void SetupClient()
        {
            var cfg = new DiscordConfiguration {
                Token = this.SharedData.BotConfiguration.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                LargeThreshold = 250,
                ShardCount = this.SharedData.BotConfiguration.ShardCount,
                ShardId = this.Id,
                LoggerFactory = new SerilogLoggerFactory(dispose: true),
                Intents = DiscordIntents.All
                       & ~DiscordIntents.GuildMessageTyping
                       & ~DiscordIntents.DirectMessageTyping
            };

            this.Client = new DiscordClient(cfg);

            this.Client.Ready += (c, e) => {
                Serilog.Log.Information("Ready!", this.Id);
                return Task.CompletedTask;
            };
        }

        private void SetupCommands()
        {
            this.CNext = this.Client.UseCommandsNext(new CommandsNextConfiguration {
                EnableDms = false,
                CaseSensitive = false,
                EnableMentionPrefix = true,
                PrefixResolver = this.PrefixResolverAsync,
                Services = new ServiceCollection()
                    .AddSingleton(this)
                    .AddSingleton(this.SharedData)
                    .AddSingleton(this.Database)
                    .AddSingleton(new AntifloodService(this))
                    .AddSingleton(new AntiInstantLeaveService(this))
                    .AddSingleton(new AntispamService(this))
                    .AddSingleton(new GiphyService(this.SharedData.BotConfiguration.GiphyKey))
                    .AddSingleton(new GoodreadsService(this.SharedData.BotConfiguration.GoodreadsKey))
                    .AddSingleton(new ImgurService(this.SharedData.BotConfiguration.ImgurKey))
                    .AddSingleton(new LinkfilterService(this))
                    .AddSingleton(new OMDbService(this.SharedData.BotConfiguration.OMDbKey))
                    .AddSingleton(new RatelimitService(this))
                    .AddSingleton(new SteamService(this.SharedData.BotConfiguration.SteamKey))
                    .AddSingleton(new WeatherService(this.SharedData.BotConfiguration.WeatherKey))
                    .AddSingleton(new YtService(this.SharedData.BotConfiguration.YouTubeKey))
                    .BuildServiceProvider()
            });

            this.CNext.SetHelpFormatter<CustomHelpFormatter>();

            this.CNext.RegisterCommands(Assembly.GetExecutingAssembly());

            this.CNext.RegisterConverter(new CustomActivityTypeConverter());
            this.CNext.RegisterConverter(new CustomBoolConverter());
            this.CNext.RegisterConverter(new CustomTimeWindowConverter());
            this.CNext.RegisterConverter(new CustomIPAddressConverter());
            this.CNext.RegisterConverter(new CustomIPFormatConverter());
            this.CNext.RegisterConverter(new CustomPunishmentActionTypeConverter());

            UpdateCommandList(this.CNext);
        }

        private void SetupInteractivity()
        {
            this.Interactivity = this.Client.UseInteractivity(new InteractivityConfiguration {
                PaginationBehaviour = PaginationBehaviour.WrapAround,
                PaginationDeletion = PaginationDeletion.DeleteEmojis,
                PaginationEmojis = new PaginationEmojis(),
                PollBehaviour = PollBehaviour.KeepEmojis,
                Timeout = TimeSpan.FromMinutes(1)
            });
        }

        private void SetupVoice()
        {
            this.Voice = this.Client.UseVoiceNext();
        }

        private Task<int> PrefixResolverAsync(DiscordMessage m)
        {
            string p = this.SharedData.GetGuildPrefix(m.Channel.Guild.Id) ?? this.SharedData.BotConfiguration.DefaultPrefix;
            return Task.FromResult(m.GetStringPrefixLength(p));
        }
        #endregion
    }
}