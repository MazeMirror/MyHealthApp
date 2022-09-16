using SQLite;

namespace MyHealthApp.Models.SqlLite
{
    public class UserEntity
    {
        [PrimaryKey]
        public long Id { get; set; }
        [MaxLength(35)]
        public string Email { get; set; }
    }
}