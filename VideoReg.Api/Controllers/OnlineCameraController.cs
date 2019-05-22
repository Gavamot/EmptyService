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
    public class Size
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public override string ToString()
        {
            return $"{Width}x{Height}";
        }
    }

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
        public ActionResult GetImg(int number, string timestamp, [FromQuery]Size size, int quality)
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
            

            var stream = new MemoryStream();
            using (MagickImage image = new MagickImage(res.Img))
            {
                image.Resize(size.Width, size.Height);
                image.Quality = quality;
                image.Write(stream);
            }
            return File(stream.ToArray(), "image/jpeg");
        }
    }
}