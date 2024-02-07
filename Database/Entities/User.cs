using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IdNumber { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
