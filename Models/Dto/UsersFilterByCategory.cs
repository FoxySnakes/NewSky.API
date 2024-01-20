namespace NewSky.API.Models.Dto
{
    public class UsersFilterByCategory
    {
        public string CategoryName { get; set; }

        public List<UserDto> Users { get; set; } = new List<UserDto>();
    }
}
