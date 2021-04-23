using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppTemplate.Models.Request
{
    public class Register : Login
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
