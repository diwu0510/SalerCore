using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zodo.Saler.Website.Models
{
    public class ChangePwViewModel
    {
        public int Id { get; set; }

        public string OldPw { get; set; }

        public string NewPw { get; set; }
    }
}
