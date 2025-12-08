using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing.Models
{
    /// <summary>
    /// Возможные состояния заказа.
    /// </summary>
    public enum OrderStatus
    {
        Pending,
        Paid,
        Shipped,
        Cancelled
    }
}
