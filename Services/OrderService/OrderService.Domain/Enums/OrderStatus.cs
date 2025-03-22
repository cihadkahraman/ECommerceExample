﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Enums
{
    public enum OrderStatus
    {
        Pending = 0,
        Completed = 1,
        Shipped = 2,
        Cancelled = 3
    }
}
