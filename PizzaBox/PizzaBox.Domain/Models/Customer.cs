using System;
using System.Collections.Generic;

namespace PizzaBox.Domain.Models
{
    public partial class Customer
    {
        public Customer()
        {
            CustomerOrder = new HashSet<CustomerOrder>();
        }

        public string Username { get; set; }
        public string Passw { get; set; }
        public string Previousorder { get; set; }

        public virtual ICollection<CustomerOrder> CustomerOrder { get; set; }
    }
}
