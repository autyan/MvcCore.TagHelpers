using System.Collections.Generic;
using McvCore.TagHelpers.Samples.Models;
using Microsoft.AspNetCore.Mvc;

namespace McvCore.TagHelpers.Samples.Controllers
{
    public class SampleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Table()
        {
            var tableItem = new List<TableSampleData>
            {
                new TableSampleData
                {
                    Id = 0,
                    Name = "Alex",
                    Age = 15
                },
                new TableSampleData
                {
                    Id = 1,
                    Name = "Lucy",
                    Age = 18
                },
                new TableSampleData
                {
                    Id = 2,
                    Name = "Baker",
                    Age = 16
                }
            };
            return View(tableItem);
        }
    }
}