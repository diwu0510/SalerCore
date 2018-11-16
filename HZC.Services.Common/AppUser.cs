using HZC.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace HZC.Common.Services
{
    public class AppUser : IAppUser
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Role { get; set; }
    }
}
