using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VideoReg.Core;
using VideoReg.Core.Thrends;

namespace VideoReg.Api.Controllers
{
    [ApiController]
    public class VideoRegController : ControllerBase
    {
        readonly IVideoRegInfoRep videoRegInfo;
        readonly ITrendsRep trendsRep;
        public VideoRegController(IVideoRegInfoRep videoRegInfo, ITrendsRep trendsRep)
        {
            this.videoRegInfo = videoRegInfo;
            this.trendsRep = trendsRep;
        }

        // GET api/values
        [HttpGet]
        [Route("/[controller]/Info")]
        public ActionResult<VideoRegResponce> GetInfo()
        {
            var res = videoRegInfo.GetInfo();
            return res;
        }

        [HttpGet]
        [Route("/[controller]/Trends")]
        public async Task<ActionResult> GetTrends()
        {
            var file = await trendsRep.GetThrends();
            return File(file, "application/json");
        }
    }
}
