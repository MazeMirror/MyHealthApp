namespace MyHealthApp.Models.SqlLite
{
    public static class ConvertToEntity
    {
        public static ProfileEntity ConvertToProfileEntity(Profile profile)
        {
            return new ProfileEntity()
            {
                Id = profile.Id,
                Gender = profile.Gender,
                Name = profile.Name,
                LastName = profile.LastName,
                BirthDate = profile.BirthDate,
                ImageUrl = profile.ImageUrl,
                RoleId = profile.RoleId,
                UserId = profile.UserId,
            };
        }
        
        public static UserEntity ConvertToUserEntity(User user)
        {
            return new UserEntity()
            {
                Id = user.Id,
                Email = user.Email,
            };
        }
        
    }
}