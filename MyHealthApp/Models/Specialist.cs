using System.Collections.Generic;

namespace MyHealthApp.Models
{
    public class Specialist : IPrototype<Specialist>
    {
        public long Id { get; set; }
        public long ProfileId { get; set; }
        public string Specialty { get; set; }
        
        public IList<Patient> Patients { get; set; } = new List<Patient>();
        public Specialist CreateDeepCopy()
        {
            var specialist = (Specialist)MemberwiseClone();
            return specialist;
        }
    }
}