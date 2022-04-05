using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using System.Diagnostics;
using WebAppWebAPIs.Models;

namespace WebAppWebAPIs.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        readonly ITokenAcquisition tokenAcquisition;
        private IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, ITokenAcquisition tokenAcquisition, IConfiguration configuration)
        {
            _logger = logger;
            this.tokenAcquisition = tokenAcquisition;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AuthorizeForScopes(ScopeKeySection = "GraphScope:CalledApiScopes")]
        public async Task<IActionResult> Graph()
        {

            string[] scopes = _configuration.GetValue<string>("GraphScope:CalledApiScopes").Split(" ");

            string token = await tokenAcquisition.GetAccessTokenForUserAsync(scopes);

            ViewBag.GraphToken = token;
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}