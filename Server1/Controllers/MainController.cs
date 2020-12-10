using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace Server1.Controllers
{
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly ILogger<MainController> _logger;

        private static bool IsBusy { get; set; }
        private static string Progress { get; set; } = string.Empty;

        public MainController(ILogger<MainController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("main")]
        public string Get(int number1, int number2)
        {
            IsBusy = true;
            var res = number1 * number2;
            Thread.Sleep(number1 * 100);
            Progress = "Part 1 is done";
            Thread.Sleep(number2 * 100);
            Progress = "Part 2 is done";
            Thread.Sleep((number1 + number2) * 100);
            Progress = "Finished";
            IsBusy = false;
            return $"Server1 {res}";
        }

        [HttpGet]
        [Route("main/busy")]
        public bool GetIsBusy()
        {
            return IsBusy;
        }

        [HttpGet]
        [Route("main/progress")]
        public string GetProgress()
        {
            return Progress;
        }
    }
}
