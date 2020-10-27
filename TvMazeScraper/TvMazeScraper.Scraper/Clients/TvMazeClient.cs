using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Polly;
using Polly.Extensions.Http;
using TvMazeScraper.Scraper.Exceptions;
using TvMazeScraper.Scraper.Models;

namespace TvMazeScraper.Scraper.Clients
{
    public class TvMazeClient : ITvMazeClient
    {
        private readonly HttpClient _client;

        public TvMazeClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<ShowDto>> GetShowsAsync(int pagenumber)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"shows?page={pagenumber}");
            var httpResponse = await _client.SendAsync(request);

            if (httpResponse.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<List<ShowDto>>(await httpResponse.Content.ReadAsStringAsync());
            }

            if (httpResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return new List<ShowDto>();
            }

            //Todo: implement logging
            throw new ApiCallException();
        }

        public async Task<List<Actor>> GetCastAsync(int showId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"shows/{showId}/cast");
            var httpResponse = await _client.SendAsync(request);

            if (httpResponse.IsSuccessStatusCode)
            {
                return JsonConvert
                    .DeserializeObject<List<Actor>>(await httpResponse.Content.ReadAsStringAsync());
            }

            //Todo: implement logging
            throw new ApiCallException();
        }

        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            var jitterer = new Random();
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(6,    // exponential back-off plus some jitter
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                                    + TimeSpan.FromMilliseconds(jitterer.Next(0, 100))
                );
        }
    }
}