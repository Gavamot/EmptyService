using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OnlineCamera.Core.Value.Api;
using System.Web;
using System.Linq;

namespace OnlineCamera.Core.Service
{

    class Responce
    {
        public byte[] Data { get; set; }
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
    }

    public class Http1Api : IVideoRegApi
    {
        const string GET_VIDEO_REG_INFO = "/VideoReg/Info/";
        const string GET_ONLINE_CAMERA_IMG = "/OnlineCamera/Img";

        ApiDataTransformer transformer = new ApiDataTransformer();

        public async Task<CameraResponce> GetImgAsync(Size size, int timeoutMs, CancellationTokenSource source)
        {
           var responce = await RetriveByteArrayFromUrlAsync(GET_ONLINE_CAMERA_IMG, HttpMethod.Get);
           var dateTime = transformer.ToDate(responce.Headers["x-img-date"]);
            return new CameraResponce
            {
                Img = responce.Data,
                SourceTimestamp = dateTime
            };
        }

        public async Task<VideoRegResponce> GetVideoRegInfoAsync(int timeoutMs, CancellationTokenSource source)
        {
            return await RetriveDataFromUrlAsync<VideoRegResponce>(GET_VIDEO_REG_INFO, HttpMethod.Get);
        }

        Dictionary<string, string> GetHeaders(WebHeaderCollection collection) 
        {
            var res = new Dictionary<string, string>();
            foreach (var key in collection.AllKeys)
            {
                res.Add(key, collection[key]);
            }
            return res;
        }

        async Task<Responce> RetriveByteArrayFromUrlAsync(string url, HttpMethod method, Dictionary<string, string> parameters = null)
        {
            var request = new WebReqvestBuilder(url, method)
               .AddParameters(parameters)
               .CreateWebReqvest();

            var res = new Responce();
            using (WebResponse response = await request.GetResponseAsync())
            {
                res.Headers = GetHeaders(response.Headers);
                using (var ms = new MemoryStream())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        stream.CopyTo(ms);
                    }
                    res.Data = ms.ToArray();
                }
            }
            return res;
        }

        async Task<T> RetriveDataFromUrlAsync<T>(string url, HttpMethod method, Dictionary<string, string> parameters = null)
        {
            var request = new WebReqvestBuilder(url, method)
                .AddParameters(parameters)
                .CreateWebReqvest();

            using (WebResponse response = await request.GetResponseAsync())
            {
                using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string responseText = await reader.ReadToEndAsync();
                    var res = JsonConvert.DeserializeObject<T>(responseText);
                    return res;
                }
            }
        }
    }
}
