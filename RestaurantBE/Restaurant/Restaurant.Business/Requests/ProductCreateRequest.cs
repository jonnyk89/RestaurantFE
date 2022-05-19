using Restaurant.Domain.Entities;
using Restaurant.Domain.Entities.MessageResponses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Business.Requests
{
    public class ProductCreateRequest
    {
        [Required(ErrorMessage = Messages.ProductNameRequired)]
        [MaxLength(100, ErrorMessage = Messages.ProductNameLength)]
        public string Name { get; set; }
        public string?  Description { get; set; }
        [Required(ErrorMessage = Messages.ProductPriceRequired)]
        [Range(0.01, Double.PositiveInfinity, ErrorMessage = Messages.ProductPriceRange)]
        public decimal Price { get; set; }


        [Required(ErrorMessage = Messages.ProductCategoryRequired)]
        public string CategoryId { get; set; }
    }
}
