using System.Collections.Generic;
using WebTask2.Data;

namespace WebTask2.Models
{
    public class HistoryViewModel
    {
        public List<UserRequest> Requests { get; set; } = new List<UserRequest>();
    }
}
