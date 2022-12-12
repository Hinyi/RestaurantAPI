using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Entities
{
    public class Role
    {
        //[Key]
        public int Id { get; set; }
        public string Name { get; set; }
        //public virtual User User { get; set; }
    }
}
