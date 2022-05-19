using Restaurant.Domain.Entities.MessageResponses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Business.Requests
{
    public class CategoryUpdateRequest
    {
        [MaxLength(100, ErrorMessage = Messages.CategoryNameLength)]
        public string? Name { get; set; }

        public string? ParentId { get; set; }
    }
}
