using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Somfic.Logging;

namespace Somfic.HTTP
{
    public class GetResult
    {
        internal GetResult(HttpWebResponse response)
        {
            StringBuilder s = new StringBuilder();
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                s.Append(reader.ReadToEnd());
            }

            Response = response;

            Exception = null;

            Content = s.ToString();
        }

        internal GetResult(Exception ex)
        {
            Exception = ex;
        }

        public HttpWebResponse Response { get; }

        public Exception Exception { get; }

        public string Content { get; }
    }

    public class GetRequest
    {
        private readonly string url;

        public GetRequest(string url)
        {
            this.url = url;
        }

        public GetResult Execute()
        {
            try
            {
                Stopwatch s = Stopwatch.StartNew();
                HttpWebResponse result = Get(url);

                Logger.Verbose($"Executed GET request from '{url}' in {s.Elapsed}.");

                return new GetResult(result);
            }
            catch (Exception ex)
            {
                Logger.Verbose($"Could not execute GET request from '{url}'.");
                return new GetResult(ex);
            }
        }

        private static HttpWebResponse Get(string uri)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(uri);
            request.UserAgent = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36";
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            return (HttpWebResponse)request.GetResponse();
        }
    }
}
