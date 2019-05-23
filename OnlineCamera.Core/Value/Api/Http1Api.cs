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
        public bool IsSecsessResponce => StatusCode == HttpStatusCode.OK;
        public HttpStatusCode StatusCode { get; set; } 
        public byte[] Data { get; set; }
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
    }

    public class Http1Api : IVideoRegApi
    {
        const string GET_VIDEO_REG_INFO = "/VideoReg/Info/";
        const string GET_ONLINE_CAMERA_IMG = "/OnlineCamera/Img";

        ApiDataTransformer transformer = new ApiDataTransformer();

        public async Task<CameraResponce> GetImgAsync(Camera camera, DateTime lastSnapshot, int timeoutMs, CancellationTokenSource source)
        {
            var parametersCollection = new ParametersCollection();
            parametersCollection.Add("number", camera.Number);
            parametersCollection.Add("timestamp", lastSnapshot);

            if (camera.Settings.IsNeedConvert)
            {
                parametersCollection.Add("isNeedConvert", camera.Settings.IsNeedConvert);
                parametersCollection.Add("width", camera.Settings.Size.Width);
                parametersCollection.Add("height", camera.Settings.Size.Height);
                parametersCollection.Add("quality", camera.Settings.Quality);
            }

            var responce = await RetriveByteArrayFromUrlAsync(camera.Settings.Ip + GET_ONLINE_CAMERA_IMG, HttpMethod.Get, parametersCollection);

            if(responce.StatusCode == HttpStatusCode.NotFound)
            {
                throw new CameraNotFoundException(camera);
            }

            if (responce.IsSecsessResponce)
            {
                var dateTime = transformer.ToDate(responce.Headers["x-img-date"]);
                return new CameraResponce
                {
                    Img = responce.Data,
                    SourceTimestamp = dateTime
                };
            }
            else
            {
                return null;
            }
        }


        public async Task<VideoRegResponce> GetVideoRegInfoAsync(string ip, int timeoutMs, CancellationTokenSource source)
        {
            return await RetriveDataFromUrlAsync<VideoRegResponce>(ip + GET_VIDEO_REG_INFO, HttpMethod.Get);
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

        async Task<Responce> RetriveByteArrayFromUrlAsync(string url, HttpMethod method, ParametersCollection parametersCollection = null)
        {
            var request = new WebReqvestBuilder(url, method)
               .AddParameters(parametersCollection)
               .CreateWebReqvest();

            var res = new Responce();
            request.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
            using (var response = (HttpWebResponse)await request.GetResponseAsync())
            {
                res.StatusCode = response.StatusCode;
                if(response.StatusCode == HttpStatusCode.OK)
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
            }
            return res;
        }

        async Task<T> RetriveDataFromUrlAsync<T>(string url, HttpMethod method, ParametersCollection parametersCollection = null)
        {
            var request = new WebReqvestBuilder(url, method)
                .AddParameters(parametersCollection)
                .CreateWebReqvest();

            using (var response = (HttpWebResponse) await request.GetResponseAsync())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        string responseText = await reader.ReadToEndAsync();
                        var res = JsonConvert.DeserializeObject<T>(responseText);
                        return res;
                    }
                }
                else
                {
                    return default;
                }
            }
        }
    }
}
