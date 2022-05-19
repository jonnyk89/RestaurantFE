using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Business.Responses
{
    public class TotalResponse
    {
        public int total { get; set; }

        public TotalResponse(int totalValue)
        {
            this.total = totalValue;
        }
    }
}
