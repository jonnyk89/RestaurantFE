using Restaurant.Domain.Entities.Enums;
using Restaurant.Domain.Entities.MessageResponses;
using Restaurant.Domain.Entities.RegularExpressions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Business.Requests
{
    public class UserCreateRequest
    {
        [Required(ErrorMessage = Messages.UserNameRequired)]
        [MaxLength(100, ErrorMessage = Messages.UserNameLength)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = Messages.UserNameRequired)]
        [MaxLength(100, ErrorMessage = Messages.UserNameLength)]
        public string LastName { get; set; }

        [Required(ErrorMessage = Messages.UserEmailRequired)]
        [MaxLength(255, ErrorMessage = Messages.UserEmailLength)]
        [EmailAddress(ErrorMessage = Messages.UserEmailInvalid)]
        public string Email { get; set; }

        [Required(ErrorMessage = Messages.UserRoleRequired)]
        public UserRole Role { get; set; }

        [Required(ErrorMessage = Messages.UserPasswordRequired)]
        [MinLength(8, ErrorMessage = Messages.UserPasswordLength),
            MaxLength(100, ErrorMessage = Messages.UserPasswordLength)]
        public string Password { get; set; }
    }
}
