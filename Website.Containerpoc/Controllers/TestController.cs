using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Revera.Stnd.Common.HashAlgorithm;

namespace Website.Containerpoc.Controllers
{
    [Route("api/dostuff")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            var value = System.Guid.NewGuid().ToString().Replace("-", "");
            if (value != null)
                return Ok(PBKDF2WithHMACSHA512.ComputeHash(value, PBKDF2WithHMACSHA512.GetNewHexedSalt(), 1000000, 64));
            return Ok();
        }

    }
}
