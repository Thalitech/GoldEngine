﻿#region USING_DIRECTIVES
using DSharpPlus.CommandsNext;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TheGodfather.Services.Database;
#endregion

namespace TheGodfather.Modules
{
    public abstract class TheGodfatherModule : BaseCommandModule
    {
        protected static readonly HttpClient _http = new HttpClient(_handler, true);
        private static readonly HttpClientHandler _handler = new HttpClientHandler { AllowAutoRedirect = false };

        protected SharedData Shared { get; }
        protected DBService Database { get; }


        protected TheGodfatherModule(SharedData shared = null, DBService db = null)
        {
            this.Shared = shared;
            this.Database = db;
        }
        

        protected async Task<bool> IsValidImageUriAsync(Uri uri)
        {
            try {
                HttpResponseMessage response = await _http.GetAsync(uri).ConfigureAwait(false);
                if (response.Content.Headers.ContentType.MediaType.StartsWith("image/"))
                    return true;
            } catch {

            }

            return false;
        }
    }
}
