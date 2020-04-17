using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace aspnet.secret.manager.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class SecretsController : ControllerBase
    {
        private readonly ILogger<SecretsController> _logger;

        private readonly IConfiguration _config;

        public SecretsController(ILogger<SecretsController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        [HttpGet("carlos")]
        public string GetSecret()
        {
            return _config["USERSECRET"];
        }
    }
}
