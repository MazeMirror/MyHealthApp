using System.Collections.Generic;

namespace MyHealthApp.Models
{
    public class Patient
    {
        public long Id { get; set; }
        public long ProfileId { get; set; }
        public double Height { get; set; } // Entero ?
        public double Weight { get; set; } // Entero ?
        public long EmergencyPhone { get; set; } //String ?
        //public IList<Specialist> Specialists { get; set; } = new List<Specialist>();
    }
}