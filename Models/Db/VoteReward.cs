using System.ComponentModel.DataAnnotations.Schema;

namespace NewSky.API.Models.Db
{
    public class VoteReward : EntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Position { get; set; }
        public string Reward { get; set; }
    }
}
