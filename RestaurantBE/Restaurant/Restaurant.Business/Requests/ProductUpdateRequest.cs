using Restaurant.Domain.Entities.MessageResponses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Business.Requests
{
    public class ProductUpdateRequest
    {
        [MaxLength(100, ErrorMessage = Messages.ProductNameLength)]
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Range(0.01, Double.PositiveInfinity, ErrorMessage = Messages.ProductPriceRange)]
        public decimal? Price { get; set; }
        public string? CategoryId { get; set; }
    }
}
