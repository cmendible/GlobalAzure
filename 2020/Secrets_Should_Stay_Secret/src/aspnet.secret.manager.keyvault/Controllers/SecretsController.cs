using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace aspnet.secret.manager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SecretsController : ControllerBase
    {
        private readonly ILogger<SecretsController> _logger;

        private readonly IConfiguration _config;

        IOptions<Secrets> _secrets;

        public SecretsController(ILogger<SecretsController> logger, IConfiguration config, IOptions<Secrets> secrets)
        {
            _logger = logger;
            _config = config;
            _secrets = secrets;
        }


        [HttpGet("aurora")]
        public string GetAurora()
        {
            return _config["supersecret"];
        }

        [HttpGet("carlos")]
        public string GetCarlos()
        {
            return _secrets.Value.DontTell;
        }
    }
}
