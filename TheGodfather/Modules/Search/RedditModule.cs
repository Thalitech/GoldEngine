﻿#region USING_DIRECTIVES
using System;
using System.Linq;
using System.Threading.Tasks;

using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

using RedditSharp;
using static RedditSharp.Things.VotableThing;
#endregion


namespace TheGodfather.Modules.Search
{
    [Group("reddit", CanInvokeWithoutSubcommand = true)]
    [Description("Reddit commands.")]
    [Aliases("r")]
    [Cooldown(2, 5, CooldownBucketType.User), Cooldown(4, 5, CooldownBucketType.Channel)]
    [PreExecutionCheck]
    [RequireOwner]
    [Hidden]
    public class RedditModule
    {
        /*
        public async Task ExecuteGroupAsync(CommandContext ctx)
        {
            var wa = new BotWebAgent();
            var reddit = new Reddit(wa, false);
            var subreddit = reddit.GetSubreddit("/r/all");
            subreddit.Subscribe();
            foreach (var post in subreddit.New.Take(5)) {
                await ctx.RespondAsync(post.Title);
            }
        }
        */
    }
}