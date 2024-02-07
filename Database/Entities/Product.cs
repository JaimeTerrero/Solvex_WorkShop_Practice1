using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public int Price { get; set; }
    }
}
