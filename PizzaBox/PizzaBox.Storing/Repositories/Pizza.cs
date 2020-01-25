using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaBox.Storing.Repositories
{
    public class Pizza
    {
        private string _crust = "regular";
        private string _size = "medium";
        private decimal _price;
        private List<string> toppings;
        public List<string> toppingChoice= new List<string> { "sausage", "pepperoni", "cheese", "bacon", "pineapple",
            "ham", "mushrooms", "onions", "green peppers", "olives", "chicken", "spinach", "sauce"};
      

        public Pizza()
        {
            toppings = new List<string> { "sauce", "cheese" };
        }
        public decimal Price
        {
            get => _price;
        }
        public string Crust
        {
            get => _crust;
            set
            {
                if (value == "thin" || value == "regular" || value == "deep dish")
                {
                    _crust = value;
                    setPrice();
                }
                else
                {
                    throw new ArgumentException("Thin, regular, and deep dish are the available crust.", nameof(value));
                }
            }
        }
        public string Size
        {
            get => _size;
            set
            {
                if (value == "small" || value == "medium" || value == "large" || value == "extralarge")
                {
                    _size = value;
                    setPrice();
                }
                else
                {
                    throw new ArgumentException("Small, medium, large, and extralarge are the size options.", nameof(value));
                }
            }
        }
        
        public void AddTopping(string top)
        {
            if (toppingChoice.Contains(top))
            {
                if (toppings.Count < 5)
                {
                    toppings.Add(top);
                    setPrice();
                }
                else
                {
                    Console.WriteLine("Pizza should have no more than 5 toppings"); ;
                }
            }
            else
            {
                Console.WriteLine("Invalid topping selection");
            }
        }
        public void DeleteTopping(string del)
        {
            if (toppings.Contains(del))
            {
                toppings.Remove(del);
                setPrice();
            }
            else
            {
                Console.WriteLine("Cant remove topping. Topping is not on pizza.");
            }
        }
        public string Toppings()
        {
            StringBuilder br = new StringBuilder();
            foreach (string tp in toppings)
            {
                br.Append(tp);
                br.Append(", ");
            }
            string pizzatp = Convert.ToString(br);
            return pizzatp;
        }
        private void setPrice()
        {
            decimal price = 0m;
            if (Crust.Equals("thin"))
            {
                price += 1.25m;
            }
            else if (Crust.Equals("regular"))
            {
                price += 1.50m;
            }
            else
            {
                price += 1.5m;
            }
            price += (decimal)(.07 * Convert.ToDouble(price) + 2.5);
            foreach (var topping in toppings)
            {
                price += .25m;
            }
            _price = price;
        }
        public void PresetPizzas(string p)
        {
            if (p.Equals("krispy"))
            {
                toppings = new List<string> { "sauce", "cheese", "green peppers", "olives", "pepperoni" };
                setPrice();
            }
            
            else if (p.Equals("supreme"))
            {
                toppings = new List<string> { "sauce", "cheese", "sausage", "mushroom", "green pepper", "pepperoni" };
                setPrice();
            }
            else if (p.Equals("meat lover"))
            {
                toppings = new List<string> { "sauce", "cheese", "sausage", "ham", "pepperoni", "bacon" };
                setPrice();
            }
            else if (p.Equals("greek"))
            {
                toppings = new List<string> { "sauce", "cheese", "onion", "pickles", "mushrooms", "chicken" };
                setPrice();
            }
            else
            {
                throw new ArgumentException("The preset pizzas consists of krispy, supreme, meat lover, and greek.");
            }
        }
    }
}