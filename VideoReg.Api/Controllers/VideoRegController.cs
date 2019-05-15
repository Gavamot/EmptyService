using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VideoReg.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VideoRegController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        [Route("[controller]/Info")]
        public ActionResult<IEnumerable<string>> GetInfo()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
