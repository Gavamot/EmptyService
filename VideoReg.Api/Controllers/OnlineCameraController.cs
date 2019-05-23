using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageMagick;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoReg.Core;

namespace VideoReg.Api.Controllers
{
    [ApiController]
    public class OnlineCameraController : ControllerBase
    {
        readonly ICache cache;
        readonly ApiDataTransformer dataTransformer = new ApiDataTransformer();

        public OnlineCameraController(ICache cache)
        {
            this.cache = cache;
        }

        [Route("/[controller]/Img")]
        public ActionResult GetImg(int number, string timestamp, bool isNeedConvert = false, int width = 0, int height = 0, int quality = 0)
        {
            CameraResponce res;
            try
            {
                res = cache.GetCamera(number);
            }
            catch(KeyNotFoundException e)
            {
                return StatusCode(404);
            }

            if (res.Timestamp == dataTransformer.ToDate(timestamp))
                return StatusCode(304); // Not modified

            Response.Headers.Add("x-img-date", dataTransformer.ToString(res.Timestamp));

            byte[] img = res.Img;
            if (isNeedConvert)
            {
                var stream = new MemoryStream();
                using (MagickImage image = new MagickImage(img))
                {
                    image.Resize(width, height);
                    image.Quality = quality;
                    image.Write(stream);
                }
                img = stream.ToArray();
            }

            return File(img, "image/jpeg");
        }
    }
}