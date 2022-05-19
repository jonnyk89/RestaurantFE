using Microsoft.AspNetCore.Http;
using Restaurant.Domain.Entities.MessageResponses;
using Restaurant.Domain.Entities.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Business.Requests
{
    public class UserUpdateProfileRequest
    {
        [MaxLength(100, ErrorMessage = Messages.UserNameLength)]
        public string? firstName { get; set; }

        [MaxLength(100, ErrorMessage = Messages.UserNameLength)]
        public string? lastName { get; set; }

        [MaxLength(255, ErrorMessage = Messages.UserEmailLength)]
        [EmailAddress(ErrorMessage = Messages.UserEmailInvalid)]
        public string? email { get; set; }

        [MinLength(8, ErrorMessage = Messages.UserPasswordLength),
            MaxLength(100, ErrorMessage = Messages.UserPasswordLength)]
        public string? password { get; set; }

        public IFormFile? picture { get; set; }
    }
}
