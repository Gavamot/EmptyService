using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineCamera.Core;

namespace OnlineCamera.Api.Controllers
{
    [ApiVersion("1.0", Deprecated = false)]
    [ApiController]
    public class CameraController : ControllerBase
    {
        readonly ICache cache;
        public CameraController(ICache cache)
        {
            this.cache = cache;
        }

        // GET api/values
        [HttpGet]
        //[Route("/v{version:apiVersion}/[controller]")]
        [Route("/[controller]/Img")]
        public ActionResult Get(string ip, int number)
         {
            try
            {
                var img = cache.GetImg(new Camera()
                {
                    VideoRegIp = ip,
                    Number = number
                });
                return File(img, "image/jpeg");
            }
            catch (KeyNotFoundException e)
            {
                return StatusCode(404);
            }
         }
    }
}
