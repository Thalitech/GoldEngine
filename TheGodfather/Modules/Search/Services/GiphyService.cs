﻿#region USING_DIRECTIVES
using System;
using System.Threading.Tasks;
using GiphyDotNet.Manager;
using GiphyDotNet.Model.Parameters;
using TheGodfather.Services;
using ImageData = GiphyDotNet.Model.GiphyImage.Data;
using RandomImageData = GiphyDotNet.Model.GiphyRandomImage.Data;
using RandomImageResult = GiphyDotNet.Model.Results.GiphyRandomResult;
using SearchResult = GiphyDotNet.Model.Results.GiphySearchResult;
#endregion

namespace TheGodfather.Modules.Search.Services
{
    public class GiphyService : ITheGodfatherService
    {
        public bool IsDisabled => this.giphy is null;

        private readonly Giphy giphy;


        public GiphyService(BotConfigService cfg)
        {
            if (!string.IsNullOrWhiteSpace(cfg.CurrentConfiguration.GiphyKey))
                this.giphy = new Giphy(cfg.CurrentConfiguration.GiphyKey);
        }


        public async Task<ImageData[]> SearchAsync(string query, int amount = 1)
        {
            if (this.IsDisabled)
                return null;

            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentException("Query missing!", nameof(query));

            if (amount < 1 || amount > 20)
                throw new ArgumentException("Result amount out of range (max 20)", nameof(amount));

            SearchResult res = await this.giphy.GifSearch(new SearchParameter {
                Query = query,
                Limit = amount
            }).ConfigureAwait(false);

            return res.Data;
        }

        public async Task<RandomImageData> GetRandomGifAsync()
        {
            if (this.IsDisabled)
                return null;

            RandomImageResult res = await this.giphy.RandomGif(new RandomParameter()).ConfigureAwait(false);
            return res?.Data;
        }

        public async Task<ImageData[]> GetTrendingGifsAsync(int amount = 1)
        {
            if (this.IsDisabled)
                return null;

            if (amount < 1 || amount > 20)
                throw new ArgumentException("Result amount out of range (max 20)", nameof(amount));

            SearchResult res = await this.giphy.TrendingGifs(new TrendingParameter {
                Limit = amount
            }).ConfigureAwait(false);

            return res.Data;
        }
    }
}
