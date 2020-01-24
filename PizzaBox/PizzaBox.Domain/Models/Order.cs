using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaBox.Domain.Models
{
    public class Order
    {
        public List<Pizza> pizzas;
        public decimal Price { get; set; }
        public string Username { get; set; }

        public string Storename { get; set; }
        public Order()
        {
            pizzas = new List<Pizza>();
            Price = 0;
        }
        public Order(string usern, string storen)
        {
            Username = usern;
            Storename = storen;
            pizzas = new List<Pizza>();
            Price = 0;
        }
        public void PizzaCount(Pizza p)
        {
            if (pizzas.Count < 100)
            {
                pizzas.Add(p);
                CalcPrice();
                if (Price > 250m)
                {
                    decimal rem = Price;
                    DeletePizza(p);
                    throw new InvalidOperationException($"Pizza price is over $250 (${rem})");
                }
            }
            else
            {
                throw new InvalidOperationException("Number of pizza is over 100.");
            }
        }
        public void DeletePizza(Pizza n)
        {
            if (pizzas.Contains(n))
            {
                pizzas.Remove(n);
                CalcPrice();
            }
            else
            {
                throw new ArgumentException("Pizza is not part of the order. Can't remove.");
            }
        }
        public void DeletePizza(int num)
        {
            if (pizzas.Count > num)
            {
                pizzas.RemoveAt(num);
                CalcPrice();
            }
            else
            {
                throw new ArgumentOutOfRangeException("Pizza not present in list. Cant remove");
            }
        }
        public string DisplayOrder()
        {
            CalcPrice();
            StringBuilder sb = new StringBuilder();
            int num = 0;
            sb.Append($"{ Username } : {Storename}, Price: ${Price}");
            foreach (Pizza p in pizzas)
            {
                sb.Append($"{num}: Pizza size: {p.Size}\t Pizza Crust:{p.Crust}\t Pizza Toppings: {p.Toppings()}");
                num++;
            }
            return sb.ToString();
        }
        public void CalcPrice()
        {
            decimal calculate = 0m;
            foreach (Pizza p in pizzas)
            {
                calculate += p.Price;
            }
            Price = calculate;
        }
    }
}
