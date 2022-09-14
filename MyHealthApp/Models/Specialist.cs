using System.Collections.Generic;

namespace MyHealthApp.Models
{
    public class Specialist
    {
        public long Id { get; set; }
        public long ProfileId { get; set; }
        public string Specialty { get; set; }
        
        public IList<Patient> Specialists { get; set; } = new List<Patient>();
    }
}