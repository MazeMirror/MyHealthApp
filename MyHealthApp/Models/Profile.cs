using System;

namespace MyHealthApp.Models
{
    public class Profile
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Gender { get; set; }
        public DateTime Birthdate { get; set; } = DateTime.Now;
    }
}