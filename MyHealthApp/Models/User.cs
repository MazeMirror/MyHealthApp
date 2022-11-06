namespace MyHealthApp.Models
{
    public class User : IPrototype<User>
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public User CreateDeepCopy()
        {
            var user = (User)MemberwiseClone();
            return user;
        }
    }
}