using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Somfic.Http
{
    public class GetService
    {
        private readonly ILogger<GetService> _log;

        public GetService(ILogger<GetService> log)
        {
            _log = log;
        }

        public async Task<T> Json<T>(T type, string url)
        {
            try
            {
                string json = await String(url);
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (JsonException ex)
            {
                _log.LogTrace(ex, "Could not download json from '{url}'", url);
                throw;
            }
        }

        public async Task<string> String(string url)
        {
            Uri uri;

            try
            {
                uri = new Uri(url);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Url is invalid", nameof(url), ex);
            }

            try
            {
                using (WebClient client = new WebClient())
                {
                    return await client.DownloadStringTaskAsync(uri);
                }
            }
            catch (Exception ex)
            {
                _log.LogTrace(ex, "Could not download string from '{url}'", url);
                throw;
            }

        }

        public async Task<byte[]> Data(string url)
        {
            Uri uri;

            try
            {
                uri = new Uri(url);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Url is invalid", nameof(url), ex);
            }

            try
            {
                using (WebClient client = new WebClient())
                {
                    return await client.DownloadDataTaskAsync(uri);
                }
            }
            catch (Exception ex)
            {
                _log.LogTrace(ex, "Could not download data from '{url}'", url);
                throw;
            }

        }

        public async Task File(string url, string path)
        {
            Uri uri;

            try
            {
                uri = new Uri(url);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Url is invalid", nameof(url), ex);
            }

            try
            {
                using (WebClient client = new WebClient())
                {
                    await client.DownloadFileTaskAsync(uri, path);
                }
            }
            catch (Exception ex)
            {
                _log.LogTrace(ex, "Could not download file from '{url}' to '{path}'", url, path);
                throw;
            }
        }
    }
}
