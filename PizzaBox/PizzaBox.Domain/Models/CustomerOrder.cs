using System;
using System.Collections.Generic;

namespace PizzaBox.Domain.Models
{
    public partial class CustomerOrder
    {
        public int OrderId { get; set; }
        public string Storename { get; set; }
        public string Username { get; set; }
        public decimal? Price { get; set; }
        public string Pizza { get; set; }

        public virtual Store StorenameNavigation { get; set; }
        public virtual Customer UsernameNavigation { get; set; }
    }
}
