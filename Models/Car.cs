using CarRental.Models.ModelsValidators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Models
{
    public class Car
    {
        public int CarID { get; set; }
        public string Brand { get; set; }
        public int ProductionYear { get; set; }
        public string Color { get; set; }
        public int Doors { get; set; }
        public int Seats { get; set; }
        public bool ManualGearbox { get; set; }
        public bool AirConditioning { get; set; }
        public string Image { get; set; }

    }


    public interface Gearbox
    {

    }
}
