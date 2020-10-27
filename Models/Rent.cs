using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Models
{
    public class Rent
    {
        public int RentID { get; set; }
        public int CarID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public ApplicationUser User { get; set; }
    }
}
