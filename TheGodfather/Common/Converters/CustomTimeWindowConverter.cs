﻿using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;
using Imgur.API.Enums;

namespace TheGodfather.Common.Converters
{
    public class CustomTimeWindowConverter : IArgumentConverter<TimeWindow>
    {
        public static TimeWindow? TryConvert(string value)
        {
            TimeWindow result = TimeWindow.Day;
            bool parses = true;
            switch (value.ToLowerInvariant()) {
                case "day":
                case "24h":
                case "d":
                    result = TimeWindow.Day;
                    break;
                case "week":
                case "7d":
                case "w":
                    result = TimeWindow.Week;
                    break;
                case "month":
                case "1mo":
                case "1m":
                case "mo":
                case "m":
                    result = TimeWindow.Month;
                    break;
                case "year":
                case "1y":
                case "y":
                    result = TimeWindow.Year;
                    break;
                case "all":
                case "a":
                    result = TimeWindow.All;
                    break;
                default:
                    parses = false;
                    break;
            }

            return parses ? result : (TimeWindow?)null;
        }


        public Task<Optional<TimeWindow>> ConvertAsync(string value, CommandContext ctx)
            => Task.FromResult(new Optional<TimeWindow>(TryConvert(value).GetValueOrDefault()));
    }
}