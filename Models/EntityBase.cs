namespace NewSky.API.Models
{
    public abstract class EntityBase
    {
        
    }

    public abstract class EntityBaseWithId : EntityBase
    {
        public int Id { get; set; }
    }
}
