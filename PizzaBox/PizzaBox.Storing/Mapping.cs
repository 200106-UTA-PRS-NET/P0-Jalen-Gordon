using System;
using PizzaBox.Domain.Models;
using PizzaBox.Storing.Repositories;
using System.Collections.Generic;
using PizzaBox.Storing;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using System.Linq;

namespace PizzaBox.Storing
{
   public class Mapping
    {
        public static Order Map(CustomerOrder o)
        {
            return new Order
            {
                Price = (decimal)o.Price,
                Storename = o.Storename,
                Username = o.Username,
                pizzas = JsonConvert.DeserializeObject<List<Pizza>>(o.Pizza)
            };
        }
        public static CustomerOrder Map(Order o)
        {
            return new CustomerOrder
            {
                Price = o.Price,
                Storename = o.Storename,
                Username = o.Username,
                Pizza = JsonConvert.SerializeObject(o.pizzas)
            };
        }
        public static User Map(Customer c)
        {
            List<CustomerOrder> s = c.CustomerOrder.ToList();
            List<Order> p = new List<Order>();
            foreach (var o in s)
            {
                p.Add(Map(o));
            }
            return new User
            {
                UserName = c.Username,
                Password = c.Passw,
                previousOrder = JsonConvert.DeserializeObject<Dictionary<string, DateTime>>(c.Previousorder),
                orders = p
            };
        }
        public static Customer Map(User u)
        {
            List<CustomerOrder> p = new List<CustomerOrder>();
            if (u.orders != null)
            {
                List<Order> s = u.orders.ToList();
                foreach (var o in s)
                {
                    p.Add(Map(o));

                }
            }
            return new Customer
            {
                Passw = u.Password,
                Username = u.UserName,
                CustomerOrder = p,
                Previousorder = JsonConvert.SerializeObject(u.previousOrder)
            };
        }
        public static Restaurant Map(Store s)
        {
            List<CustomerOrder> z = s.CustomerOrder.ToList();
            List<Order> p = new List<Order>();
            foreach (var o in z)
            {
                p.Add(Map(o));
            }
            return new Restaurant
            {
                StoreName = s.Storename,
                Password = s.Storepassword,
                orders = p
            };
        }
        public static Store Map(Restaurant s)
        {
            List<CustomerOrder> p = new List<CustomerOrder>();
            if (s.orders != null)
            {
                List<Order> v = s.orders.ToList();
                foreach (var o in v)
                {
                    p.Add(Map(o));

                }
            }
            return new Store
            {
                Storename = s.StoreName,
                Storepassword = s.Password,
                CustomerOrder = p
            };
        }
    }
}
