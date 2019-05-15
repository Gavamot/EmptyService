using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoReg.Core;

namespace VideoReg.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OnlineCameraController : ControllerBase
    {
        readonly IImgRep imgRep;
        readonly ApiDataTransformer dataTransformer = new ApiDataTransformer();
        public OnlineCameraController(IImgRep imgRep)
        {
            this.imgRep = imgRep;
        }
        

        [Route("[controller]/Img")]
        public ActionResult<byte[]> GetImg(int number, string timestamp)
        {
            var res = imgRep.GetImg(number);

            if (res == null)
                return StatusCode(404);
            if (res.TimeStampt == dataTransformer.ToDate(timestamp))
                return StatusCode(304); // Not modified

            Response.Headers.Add("x-img-date", dataTransformer.ToString(res.TimeStampt));
            return res.Img;
        }
    }
}