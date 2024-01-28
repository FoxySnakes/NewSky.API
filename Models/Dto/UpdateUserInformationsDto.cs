namespace NewSky.API.Models.Dto
{
    public class UpdateUserInformationsDto
    {
        public string UserName { get; set; }

        public string Uuid { get; set; }

        public List<string> Roles { get; set; }
    }
}
