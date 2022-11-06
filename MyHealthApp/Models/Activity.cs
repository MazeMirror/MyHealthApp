using System.ComponentModel;

namespace MyHealthApp.Models
{
    public class Activity : IPrototype<Activity>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        
        public string Unit { get; set; }
        
        public Activity CreateDeepCopy()
        {
            var activity = (Activity)MemberwiseClone();
            return activity;
        }
    }
}