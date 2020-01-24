using System;
using System.Collections.Generic;


namespace PizzaBox.Domain.Models
{
    public class User
    {
        public Dictionary<string, DateTime> previousOrder = new Dictionary<string, DateTime>();
        public List<Order> orders;

        public string UserName { get; set; }
        public string Password { get; set; }
        public User()
        {

        }
        public User(string _usern, string _password)
        {
            UserName = _usern;
            Password = _password;
            previousOrder = new Dictionary<string, DateTime>();
            orders = new List<Order>();
            
        }
        public void DisplayOrders()
        {

            foreach (var po in orders)
            {
                Console.WriteLine(po.DisplayOrder());

            }
        }
        public void NewPassword(string currentpass, string np)
        {
            if (Password == currentpass)
            {
                Password = np;
                Console.WriteLine("New Password is Set. ");
            }
            else
            {
                Console.WriteLine("Incorrect password...");
            }
        }
      
    }
}