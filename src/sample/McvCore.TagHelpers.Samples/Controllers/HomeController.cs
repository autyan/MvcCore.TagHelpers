using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using McvCore.TagHelpers.Samples.Models;

namespace McvCore.TagHelpers.Samples.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
