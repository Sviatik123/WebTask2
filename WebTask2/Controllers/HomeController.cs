using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using WebTask2.Data;
using WebTask2.Models;

namespace WebTask2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private static CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();
        private static CancellationToken Token = CancellationTokenSource.Token;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
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

            var url1 = $"https://localhost:44310/main?number1={num1}&number2={num2}";
            var url2 = $"https://localhost:44376/main?number1={num1}&number2={num2}";
            string result = "";

            if (num1 > 1000 || num2 > 1000)
            {
                result = "Numbers can't be greater than 1000";
            }
            else
            {
                try
                {
                    var server = ChooseServer().Result;
                    if (server == 1)
                    {
                        response = await httpClient.GetAsync(url1, Token);
                    }
                    else if (server == 2)
                    {
                        response = await httpClient.GetAsync(url2, Token);
                    }
                    else
                    {
                        result = "Servers are busy";
                    }

                }
                catch (OperationCanceledException ex)
                {
                    _db.Requests.Add(new UserRequest() { Number1 = num1, Number2 = num2, Status = "Cancelled", User = User.Identity.Name});
                    _db.SaveChanges();
                    result = ex.Message;
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

                        var data = result.Split();

                        _db.Requests.Add(new UserRequest() { Number1 = num1, Number2 = num2, Result = int.Parse(data[1]), Status = "Completed", ServerNumber = data[0], User = User.Identity.Name });
                        _db.SaveChanges();
                    }
                    else
                    {
                        result = "Error" + response.StatusCode;
                    }

                }
            }

            ViewBag.Result = result;
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult History()
        {
            return View(new HistoryViewModel() {Requests = _db.Requests.Where(r => r.User == User.Identity.Name).ToList()});
        }

        public IActionResult Cancel()
        {
            CancellationTokenSource.Cancel();
            CancellationTokenSource = new CancellationTokenSource();
            Token = CancellationTokenSource.Token;
            ViewBag.Result = "Your request is cancelled!";
            return View("SendRequest");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<int> ChooseServer()
        {
            
                var httpClient = new HttpClient();

                var url1 = "https://localhost:44310/main/busy";
                var url2 = "https://localhost:44376/main/busy";
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
