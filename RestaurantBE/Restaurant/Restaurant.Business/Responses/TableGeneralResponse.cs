﻿
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Business.Responses
{
    public class TableGeneralResponse
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public int Capacity { get; set; }
        public string Waiter { get; set; }
        public string Bill { get; set; }
    }
}
