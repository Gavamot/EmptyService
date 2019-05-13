using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;


namespace OnlineCamera.Core.Service
{
    static class HttpMethodExt
    {
        public static string GetStringValue(this HttpMethod self)
        {
            switch (self)
            {
                case HttpMethod.Get: return "GET";
                case HttpMethod.Post: return "POST";
                case HttpMethod.Delete: return "DELETE";
                case HttpMethod.Put: return "PUT";
                case HttpMethod.Update: return "UPDATE";
                default: throw new NotImplementedException();
            }
        }
    }

    enum HttpMethod
    {
        Get,
        Post,
        Put,
        Update,
        Delete
    }
    class WebReqvestBuilder
    {
        protected readonly HttpMethod[] UrlParameters = new[]{
                HttpMethod.Get
            };
        string GenerateParametersString(Dictionary<string, string> parameters)
        {
            if (parameters == null || !parameters.Any())
                return string.Empty;

            var parametersArray = parameters.Select(x => $"{x.Key}={x.Value}").ToArray();
            string res = string.Join("&", parametersArray);
            return res;
        }

        WebRequest GetWithUrlParameters(string url, string method, Dictionary<string, string> parameters)
        {
            string getParameters = GenerateParametersString(parameters);
            url += $"?{getParameters}";
            return WebRequest.Create(url);
        }

        protected WebRequest GetWithBodyParameters(string url, string method, Dictionary<string, string> parameters)
        {
            var request = WebRequest.Create(url);
            string postParameters = GenerateParametersString(parameters);
            byte[] postData = Encoding.UTF8.GetBytes(WebUtility.UrlEncode(postParameters));
            request.ContentLength = postData.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(postData, 0, postData.Length);
            }
            return request;
        }

        Dictionary<string, string> parameters { get; set; }
        string url { get; set; }
        HttpMethod method { get; set; }
        public WebReqvestBuilder(string url, HttpMethod method)
        {
            this.url = url;
            this.method = method;
        }

        public WebReqvestBuilder AddParameters(Dictionary<string, string> parameters)
        {
            this.parameters = parameters;
            return this;
        }

        public WebRequest CreateWebReqvest()
        {
            if (UrlParameters.Contains(method))
                return GetWithUrlParameters(url, method.GetStringValue(), parameters);
            else
                return GetWithBodyParameters(url, method.GetStringValue(), parameters);
        }

    }
}
