using ECommerceAPI.Shared.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Shared.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        public ConfigController(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }


        [HttpGet]
        public IActionResult GetConfig()
        {
            var appName = _config["AppSettings:AppName"];
            var settings = _config["AppSettings:Settings"];
            var conString = _config["ConnectionStrings:DefaultConnection"];
            var environment = _env.EnvironmentName;

            return Ok(new
            {
                AppName = appName,
                Settings = settings,
                ConnectionString = conString,
                Environment = environment

            });
        }
    }
}
