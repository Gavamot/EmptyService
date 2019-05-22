using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineCamera.Api.Controllers
{
    [ApiController]
    public class OnlineTestController : ControllerBase
    {
        public OnlineTestController()
        {

        }

        [Route("[controller]/res")]
        public string GetResult(string p)
        {
            return "1";
        }
    }
}