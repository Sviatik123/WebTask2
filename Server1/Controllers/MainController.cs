using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Server1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {
        private readonly ILogger<MainController> _logger;

        private static bool IsBusy { get; set; }

        public MainController(ILogger<MainController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get(int number1, int number2)
        {
            IsBusy = true;
            var res = number1 * number2;
            Thread.Sleep(10000);
            IsBusy = false;
            return $"Server1 {res}";
        }

        [HttpGet ("{isBusy}")]
        public bool GetIsBusy()
        {
            return IsBusy;
        }
    }
}
