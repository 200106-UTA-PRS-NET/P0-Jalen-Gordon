using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaBox.Domain.Models
{
    public class Restaurant
    {
        public string StoreName { get; set; }
        public string Password { get; set; }
        public List<Order> orders;
        public Restaurant()
        {

        }
        public Restaurant(string storen, string password)
        {
            Password = password;
            StoreName = storen;
        }
        public void DisplayOrders()
        {
            
            foreach (var o in orders)
            {
                Console.WriteLine(o.DisplayOrder());
                
            }
        }

    }
}