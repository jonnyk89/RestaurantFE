using Restaurant.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.Business.Responses
{
    public class GeneralCategoryResponse
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public List<GeneralCategoryResponse> Subcategories { get; set; }
    }
}
