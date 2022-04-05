using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;

namespace WebAppWebAPIs.Controllers
{
    // https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/adding-controller?view=aspnetcore-6.0&tabs=visual-studio
    [Authorize]
    public class WebAPIController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        readonly ITokenAcquisition tokenAcquisition;
        private IConfiguration _configuration;
        public WebAPIController(ILogger<HomeController> logger, ITokenAcquisition tokenAcquisition, IConfiguration configuration)
        {
            _logger = logger;
            this.tokenAcquisition = tokenAcquisition;
            _configuration = configuration;
        }

        [AuthorizeForScopes(ScopeKeySection = "ApiScope:CalledApiScopes")]
        public async Task<IActionResult> Api()
        {
            string[] scopes = _configuration.GetValue<string>("ApiScope:CalledApiScopes").Split(" "); ;
            string token = await tokenAcquisition.GetAccessTokenForUserAsync(scopes);
            ViewBag.ApiToken = token;
            return View();
        }
        public IActionResult Index()
        // public string Index()
        {
            return View();
            // return "This is my default action...";
        }
    }
}
