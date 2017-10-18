﻿#region USING_DIRECTIVES
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using TheGodfather.Helpers.DataManagers;
using TheGodfather.Exceptions;

using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.Entities;
#endregion

namespace TheGodfather.Commands.SWAT
{
    [Description("SWAT4 related commands.")]
    [Cooldown(2, 5, CooldownBucketType.User), Cooldown(4, 5, CooldownBucketType.Channel)]
    public class CommandsSwat
    {
        #region PRIVATE_FIELDS
        private ConcurrentDictionary<ulong, bool> _UserIDsCheckingForSpace = new ConcurrentDictionary<ulong, bool>();
        private int _checktimeout = 200;
        #endregion
        

        #region COMMAND_SERVERLIST
        [Command("serverlist")]
        [Description("Print the serverlist with current player numbers")]
        [Aliases("slist", "swat4servers", "swat4stats")]
        public async Task Servers(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var embed = new DiscordEmbedBuilder() { Title = "Servers" };
            foreach (var server in ctx.Dependencies.GetDependency<SwatServerManager>().Servers) {
                var split = server.Value.Split(':');
                var info = await QueryIP(ctx, split[0], int.Parse(split[1]));
                if (info != null)
                    embed.AddField(info[0], $"IP: {split[0]}:{split[1]}\nPlayers: {Formatter.Bold(info[1] + " / " + info[2])}");
                else
                    embed.AddField(server.Key, $"IP: {split[0]}:{split[1]}\nPlayers: Offline");
            }
            await ctx.RespondAsync(embed: embed);
        }
        #endregion

        #region COMMAND_QUERY
        [Command("query")]
        [Description("Return server information.")]
        [Aliases("info", "check")]
        public async Task Query(CommandContext ctx, 
                               [Description("IP.")] string ip = null)
        {
            if (string.IsNullOrWhiteSpace(ip))
                throw new InvalidCommandUsageException("IP missing.");

            var servers = ctx.Dependencies.GetDependency<SwatServerManager>().Servers;
            if (servers.ContainsKey(ip))
                ip = servers[ip];

            try {
                var split = ip.Split(':');
                var info = await QueryIP(ctx, split[0], int.Parse(split[1]));
                if (info != null)
                    await SendEmbedInfo(ctx, split[0] + ":" + split[1], info);
                else
                    await ctx.RespondAsync("No reply from server.");
            } catch (Exception) {
                await ctx.RespondAsync("Invalid IP format.");
            }
        }
        #endregion

        #region COMMAND_SETTIMEOUT
        [Command("settimeout")]
        [Description("Set checking timeout.")]
        [RequireOwner]
        [Hidden]
        public async Task SetTimeout(CommandContext ctx,
                                    [Description("Timeout.")] int timeout = 200)
        {
            _checktimeout = timeout;
            await ctx.RespondAsync("Timeout changed to: " + Formatter.Bold(_checktimeout.ToString()));
        }
        #endregion

        #region COMMAND_STARTCHECK
        [Command("startcheck")]
        [Description("Notifies of free space in server.")]
        [Aliases("checkspace", "spacecheck")]
        [RequireUserPermissions(Permissions.ManageGuild)]
        public async Task StartCheck(CommandContext ctx, 
                                    [Description("Registered name or IP.")] string ip = null)
        {
            if (string.IsNullOrWhiteSpace(ip))
                throw new InvalidCommandUsageException("Name/IP missing.");

            if (_UserIDsCheckingForSpace.ContainsKey(ctx.User.Id))
                throw new CommandFailedException("Already checking space for you!");

            if (_UserIDsCheckingForSpace.Count > 10)
                throw new CommandFailedException("Maximum number of checks reached, please try later!");

            var servers = ctx.Dependencies.GetDependency<SwatServerManager>().Servers;
            if (servers.ContainsKey(ip))
                ip = servers[ip];

            string[] split;
            try {
                split = ip.Split(':');
            } catch (Exception) {
                throw new InvalidCommandUsageException("Invalid IP format.");
            }
            await ctx.RespondAsync($"Starting check on {split[0]}:{split[1]}...");

            _UserIDsCheckingForSpace.GetOrAdd(ctx.User.Id, true);
            while (_UserIDsCheckingForSpace[ctx.User.Id]) {
                try {
                    var info = await QueryIP(ctx, split[0], int.Parse(split[1]));
                    if (info == null) {
                        await ctx.RespondAsync("No reply from server. Should I try again?");
                        var interactivity = ctx.Client.GetInteractivityModule();
                        var msg = await interactivity.WaitForMessageAsync(
                            xm => xm.Author.Id == ctx.User.Id &&
                                (xm.Content.ToLower().StartsWith("yes") || xm.Content.ToLower().StartsWith("no")),
                            TimeSpan.FromMinutes(1)
                        );
                        if (msg == null || msg.Message.Content.StartsWith("no")) {
                            await StopCheck(ctx);
                            return;
                        }
                    } else if (int.Parse(info[1]) < int.Parse(info[2])) {
                        await ctx.RespondAsync(ctx.User.Mention + ", there is space on " + info[0]);
                    }
                } catch (Exception e) {
                    await StopCheck(ctx);
                    throw e;
                }
                await Task.Delay(3000);
            }

            bool outv;
            _UserIDsCheckingForSpace.TryRemove(ctx.User.Id, out outv);
        }
        #endregion

        #region COMMAND_STOPCHECK
        [Command("stopcheck")]
        [Description("Stops space checking.")]
        [Aliases("checkstop")]
        [RequireUserPermissions(Permissions.ManageGuild)]
        public async Task StopCheck(CommandContext ctx)
        {
            if (!_UserIDsCheckingForSpace.ContainsKey(ctx.User.Id))
                throw new CommandFailedException("No checks started from you.", new KeyNotFoundException());
            _UserIDsCheckingForSpace.TryUpdate(ctx.User.Id, false, true);
            await ctx.RespondAsync("Checking stopped.");
        }
        #endregion


        #region HELPER_FUNCTIONS
        private async Task<string[]> QueryIP(CommandContext ctx, string ip, int port)
        {
            var client = new UdpClient();
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port + 1);
            client.Connect(ep);
            client.Client.SendTimeout = _checktimeout;
            client.Client.ReceiveTimeout = _checktimeout;

            byte[] receivedData = null;
            try {
                string query = "\\status\\";
                await client.SendAsync(Encoding.ASCII.GetBytes(query), query.Length);
                receivedData = client.Receive(ref ep);
            } catch {
                return null;
            }

            if (receivedData == null)
                return null;

            client.Close();
            var data = Encoding.ASCII.GetString(receivedData, 0, receivedData.Length);

            var split = data.Split('\\');
            int index = 0;
            foreach (var s in split) {
                if (s == "hostname")
                    break;
                index++;
            }

            if (index < 10)
                return new string[] { split[index + 1], split[index + 3], split[index + 5], split[index + 7], split[index + 11] };

            return null;
        }
        
        private async Task SendEmbedInfo(CommandContext ctx, string ip, string[] info)
        {
            var embed = new DiscordEmbedBuilder() {
                Url = "https://swat4stats.com/servers/" + ip,
                Title = info[0],
                Description = ip,
                Timestamp = DateTime.Now,
                Color = DiscordColor.Gray
            };
            embed.AddField("Players", info[1] + "/" + info[2]);
            embed.AddField("Map", info[4]);
            embed.AddField("Game mode", info[3]);
            await ctx.RespondAsync(embed: embed);
        }
        #endregion


        [Group("servers", CanInvokeWithoutSubcommand = false)]
        [Description("SWAT4 serverlist manipulation commands.")]
        public class CommandsServers
        {
            #region COMMAND_SERVERS_ADD
            [Command("add")]
            [Description("Add a server to serverlist.")]
            [Aliases("+")]
            [RequireUserPermissions(Permissions.Administrator)]
            public async Task Add(CommandContext ctx,
                                 [Description("Name.")] string name = null,
                                 [Description("IP.")] string ip = null)
            {
                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(ip))
                    throw new InvalidCommandUsageException("Invalid name or IP.");

                var split = ip.Split(':');
                if (split.Length < 2)
                    throw new InvalidCommandUsageException("Invalid IP format.");

                if (split.Length < 3)
                    ip += ":" + (int.Parse(split[1]) + 1).ToString();

                if (ctx.Dependencies.GetDependency<SwatServerManager>().TryAdd(name, ip))
                    await ctx.RespondAsync("Server added. You can now query it using the name provided.");
                else
                    throw new CommandFailedException("Failed to add server to serverlist.");
            }
            #endregion

            #region COMMAND_SERVERS_DELETE
            [Command("delete")]
            [Description("Remove a server from serverlist.")]
            [Aliases("-", "remove")]
            [RequireUserPermissions(Permissions.Administrator)]
            public async Task Delete(CommandContext ctx,
                                    [Description("Name.")] string name = null)
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new InvalidCommandUsageException("Name missing.");

                if (ctx.Dependencies.GetDependency<SwatServerManager>().TryRemove(name))
                    await ctx.RespondAsync("Server removed.");
            }
            #endregion

            #region COMMAND_SERVERS_SAVE
            [Command("save")]
            [Description("Saves all the servers in the list.")]
            [RequireOwner]
            public async Task SaveServers(CommandContext ctx)
            {
                ctx.Dependencies.GetDependency<SwatServerManager>().Save(ctx.Client.DebugLogger);
                await ctx.RespondAsync("Servers successfully saved.");
            }
            #endregion
        }
    }
}
