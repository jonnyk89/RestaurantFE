using Restaurant.Domain.Entities;
using Restaurant.Domain.Entities.MessageResponses;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Business.Requests
{
    public class CategoryCreateRequest
    {
        [Required(ErrorMessage = Messages.CategoryNameRequired)]
        [MaxLength(100, ErrorMessage = Messages.CategoryNameLength)]
        public string Name { get; set; }

        public string? ParentId { get; set; }
    }
}
