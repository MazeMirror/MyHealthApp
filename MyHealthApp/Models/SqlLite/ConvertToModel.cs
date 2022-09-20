namespace MyHealthApp.Models.SqlLite
{
    public static class ConvertToModel
    {
        public static User ConvertToUserModel(UserEntity user)
        {
            return new User()
            {
                Id = user.Id,
                Email = user.Email,
            };
        }
    }
}