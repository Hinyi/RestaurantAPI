using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace RestaurantAPI.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string? FirstName { get; set; }// = string.Empty;
        public string? LastName { get; set; }// = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string Nationality { get; set; }
        public string PasswordHash { get; set; } = string.Empty;

        public int? RoleId { get; set; }
        //public virtual Role Role { get; set; }
    }
}
