using Microsoft.AspNetCore.Identity;
using Restaurant.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Domain.Entities
{
    public class User : IdentityUser
    {
        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }

        public UserRole Role { get; set; }

        public byte[]? Picture { get; set; }

        public virtual ICollection<Table> Tables { get; set; }
    }
}
