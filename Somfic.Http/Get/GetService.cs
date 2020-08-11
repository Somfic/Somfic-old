using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Somfic.Http.Get
{
    public class GetService
    {
        private readonly ILogger<GetService> _log;
        private readonly HttpClient _client;

        public GetService(ILogger<GetService> log, HttpClient client)
        {
            _log = log;
            _client = client;
        }

        private HttpClient GetClient()
        {
            return _client;
        }

        public async Task<string> String(Uri uri)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);
            HttpResponseMessage result = await GetClient().SendAsync(request);

            try
            {
                result.EnsureSuccessStatusCode();
                return await result.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                _log.LogTrace(ex, "Could not GET string ({code}, {reason}) from '{url}'", result.StatusCode.ToString(),
                    result.ReasonPhrase, uri.AbsolutePath);
                throw;
            }
        }

        public async Task<T> Json<T>(Uri uri)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);
            HttpResponseMessage result = await GetClient().SendAsync(request);

            try
            {
                result.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                _log.LogTrace(ex, "Could not GET json ({code}, {reason}) from '{url}'", result.StatusCode.ToString(),
                    result.ReasonPhrase, uri.AbsolutePath);
                throw;
            }
        }

        public async Task<byte[]> Data(Uri uri)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);
            HttpResponseMessage result = await GetClient().SendAsync(request);

            try
            {
                result.EnsureSuccessStatusCode();
                return await result.Content.ReadAsByteArrayAsync();
            }
            catch (Exception ex)
            {
                _log.LogTrace(ex, "Could not GET data ({code}, {reason}) from '{url}'", result.StatusCode.ToString(),
                    result.ReasonPhrase, uri.AbsolutePath);
                throw;
            }
        }

        public async Task<Stream> Stream(Uri uri)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);
            HttpResponseMessage result = await GetClient().SendAsync(request);

            try
            {
                result.EnsureSuccessStatusCode();
                return await result.Content.ReadAsStreamAsync();
            }
            catch (Exception ex)
            {
                _log.LogTrace(ex, "Could not GET stream ({code}, {reason}) from '{url}'", result.StatusCode.ToString(),
                    result.ReasonPhrase, uri.AbsolutePath);
                throw;
            }
        }
    }
}
