using System;
using System.Collections.Generic;

namespace PizzaBox.Storing.Repositories
{
    public partial class Store
    {
        public Store()
        {
            CustomerOrder = new HashSet<CustomerOrder>();
        }

        public string Storename { get; set; }
        public string Storepassword { get; set; }

        public virtual ICollection<CustomerOrder> CustomerOrder { get; set; }
    }
}
