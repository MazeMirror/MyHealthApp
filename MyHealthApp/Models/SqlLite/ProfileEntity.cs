using System;
using SQLite;

namespace MyHealthApp.Models.SqlLite
{
    public class ProfileEntity
    {
        [PrimaryKey]
        public long Id { get; set; }
        
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string ImageUrl { get; set; }
        public DateTime BirthDate { get; set; }
    }
}