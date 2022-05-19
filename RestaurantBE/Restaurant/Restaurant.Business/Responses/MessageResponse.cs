using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Business.Responses
{
    public class MessageResponse
    {
        public MessageResponse()
        {
        }

        public MessageResponse(string message)
        {
            Message = message;
        }

        public MessageResponse(string message, string Id)
        {
            Message = message;
            Id = Id;
        }

        public string? Message { get; set; }
        public virtual string? Id { get; set; }
    }
}
