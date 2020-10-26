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

        public async Task<IEnumerable<Show>> GetShowsAsync(int pagenumber)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"shows?page={pagenumber}");
            var httpResponse = await _client.SendAsync(request);

            if (httpResponse.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<IEnumerable<Show>>(await httpResponse.Content.ReadAsStringAsync());
            }

            return httpResponse.StatusCode switch
            {
                HttpStatusCode.NotFound => new List<Show>(),
                _ => throw new ApiException()
            };
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