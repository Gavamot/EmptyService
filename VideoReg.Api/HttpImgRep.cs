using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VideoReg.Core;

namespace VideoReg.Api
{
    public class HttpImgRep : IImgRep
    {
        private readonly IHttpClientFactory httpClientFactory;

        public HttpImgRep(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }



        public async Task<byte[]> GetImgAsync(string url, int timeoutMs)
        {
            using (var client = httpClientFactory.CreateClient(url))
            {
                client.Timeout = new TimeSpan(0, 0, timeoutMs/1000);
                return await client.GetByteArrayAsync(url);
            }
        }
    }
}
