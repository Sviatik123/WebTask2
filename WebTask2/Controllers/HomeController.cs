using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using WebTask2.Models;

namespace WebTask2.Controllers
{
    public class HomeController : Controller
    {
        private static object _locker = new object();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult SendRequest()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SendRequest(int num1, int num2)
        {

            var httpClient = new HttpClient();
            var response = new HttpResponseMessage();

            var url1 = $"https://localhost:44310/Main?number1={num1}&number2={num2}";
            var url2 = $"https://localhost:44376/Main?number1={num1}&number2={num2}";
            string result = "";

            try
            {
                var server = ChooseServer().Result;
                if (server == 1)
                {
                    var res = Task.Run(() => httpClient.GetAsync(url1));
                    response = res.Result;

                }
                else if (server == 2)
                {
                    var res = Task.Run(() => httpClient.GetAsync(url2));
                    response = res.Result;
                }
                else
                {
                    result = "Servers are busy";
                }

            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            if (result == "")
            {
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    result = "Error" + response.StatusCode;
                }

            }

            ViewBag.Result = result;
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult History()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<int> ChooseServer()
        {
            
                var httpClient = new HttpClient();

                var url1 = "https://localhost:44310/Main/isBusy";
                var url2 = "https://localhost:44376/Main/isBusy";
                var res = await httpClient.GetAsync(url1);

                if (await res.Content.ReadAsStringAsync() == "false")
                {
                    return 1;
                }

                res = await httpClient.GetAsync(url2);
                if (await res.Content.ReadAsStringAsync() == "false")
                {
                    return 2;
                }

                return 3;
            
        }
    }
}
