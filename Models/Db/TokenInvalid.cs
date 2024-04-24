namespace NewSky.API.Models.Db
{
    public class TokenInvalid : EntityBaseWithId
    {
        public TokenInvalid(string value)
        {
            Value = value;
        }

        public string Value { get; set; }
    }
}
