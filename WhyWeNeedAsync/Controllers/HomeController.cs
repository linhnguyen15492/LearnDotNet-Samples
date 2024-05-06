using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WhyWeNeedAsync.Models;

namespace WhyWeNeedAsync.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return Content("ASP.NET threadpool starvation demo - https://github.com/daohainam/LearnDotNet-Samples");
        }

        private async Task<Product> FindProduct(int productId, string name)
        {
            await Task.Delay(1000);
            return new Product() { Id = productId, Name = name };
        }
        [Route("/taskresultwait")]
        public IActionResult TaskResultWait()
        {
            var product = FindProduct(1, "Apple iPhone").Result;

            return Ok("/taskresultwait:success");
        }

        [Route("/taskawait")]
        public async Task<IActionResult> TaskAwait()
        {
            var product = await FindProduct(1, "Apple iPhone");

            return Ok("/taskawait:success");
        }
    }
}
