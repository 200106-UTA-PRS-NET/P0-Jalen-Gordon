using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PizzaBox.Domain.Models;
using PizzaBox.Storing.Repositories;
using System.Linq;
using PizzaBox.Storing;
using Newtonsoft.Json;

namespace PizzaBox.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            #region configuration
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = configBuilder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<PizzaDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("PizzaDb"));
            var options = optionsBuilder.Options;
            PizzaDbContext db = new PizzaDbContext(options);
            #endregion configuration

            // set bool "execute" to True while running login system
            bool execute = true;

            List<Restaurant> restaurant = new List<Restaurant>();
            List<Order> order = new List<Order>();

            User user = null;
            Restaurant currRestaurant = null;
            Restaurant chosenRestaurant = null;

            string userinput;
            string action = "home";

            Console.WriteLine("Welcome to Jay's Pizza!");

            while (execute)
            {
                // List of options for user at home screen
                Console.WriteLine("Please choose an option below: ");
                Console.WriteLine("1. Login as an User");
                Console.WriteLine("2. Login as an Admin");
                Console.WriteLine("3. Exit the menu");

                userinput = Console.ReadLine();

                // Checking if option 1 is selected
                if (userinput.Equals("1"))
                {
                    action = "Login User";
                    while (action.Equals("Login User"))
                    {
                        if (user == null)
                        {
                            Console.WriteLine("Choose an option: ");
                            Console.WriteLine("4: Login");
                            Console.WriteLine("5: Signup");
                            Console.WriteLine("6: Navigate back");
                            userinput = Console.ReadLine();

                            if (userinput.Equals("4"))
                            {
                                action = "login";
                                while (action.Equals("login"))
                                {
                                    Console.WriteLine("Please enter your username. Enter 'q' to navigate backward.");
                                    string uinput = Console.ReadLine();

                                    if (uinput == "q")
                                    {
                                        action = "Login User";
                                    }
                                    // Username captures input from user
                                    if (db.Customer.Any(b => b.Username == uinput) && uinput != "q")
                                    {
                                        Console.WriteLine("Please enter your password: ");
                                        string password = Console.ReadLine();
                                        if (db.Customer.Any(b => b.Username == uinput && b.Passw == password))
                                        {
                                            var query = from b in db.Customer
                                                        where b.Username == uinput && b.Passw == password
                                                        select b;
                                            try
                                            {
                                                Customer customer = query.Single();
                                                user = Mapping.Map(customer);
                                                Console.WriteLine($"Welcome, {user.UserName}.");
                                                action = "Login User";
                                            }
                                            catch
                                            {
                                                Console.WriteLine("Username already exists!");
                                                throw;
                                            }
                                        }
                                    }

                                    else
                                    {
                                        Console.WriteLine("Username doesn't exist");
                                    }
                                }
                            }

                            else if (userinput.Equals("5"))
                            {
                                action = "New User";
                                while (action.Equals("New User"))
                                {
                                    string uinput = "JayTest";
                                    string passw;
                                    bool different = false;
                                    while (!different)
                                    {
                                        different = true;
                                        Console.WriteLine("Enter new username: ");
                                        uinput = Console.ReadLine();
                                        if (db.Customer.Any(b => b.Username == uinput))
                                        {
                                            different = false;
                                            Console.WriteLine("Username already exists.");
                                        }
                                    }
                                    Console.WriteLine("Please enter a password");
                                    passw = Console.ReadLine();
                                    User newUser = new User
                                    {
                                        UserName = uinput,
                                        Password = passw

                                    };
                                    NewUser(db, newUser);
                                    action = "Login User";
                                    Console.WriteLine($"Account Info: Username: {uinput}, Password: {passw}");
                                }
                            }
                            else if (userinput.Equals("6"))
                            {
                                action = "home";
                            }
                            else
                            {
                                Console.WriteLine("Error. Please enter options 4, 5, or 6");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Please select an option: ");
                            Console.WriteLine("7: Logout");
                            Console.WriteLine("8: New Order");
                            Console.WriteLine("9: Order History");
                            Console.WriteLine("0: Navigate Back");
                            userinput = Console.ReadLine();

                            if (userinput.Equals("7"))
                            {
                                user = null;
                            }
                            else if (userinput.Equals("8"))
                            {
                                Console.WriteLine("Please choose a restaurant: ");
                                action = "cr";
                                while (action.Equals("cr"))
                                {
                                    DisplayRestaraunts(db);
                                    userinput = Console.ReadLine();
                                    if (userinput.Equals("0"))
                                    {
                                        action = "Login User";
                                        break;
                                    }
                                    var query = from u in db.Store
                                                where u.Storename == userinput
                                                select u;
                                    try
                                    {
                                        Store store4 = query.Single();
                                        chosenRestaurant = Mapping.Map(store4);
                                        Console.WriteLine($"Restarant, {chosenRestaurant.StoreName}, is chosen.");
                                        action = "";
                                    }
                                    catch
                                    {
                                        Console.WriteLine("Restaurant Doesn't Exist.");
                                    }
                                }
                                if (action.Equals("Login User"))
                                {
                                    break;
                                }
                                Console.WriteLine("Please make an order");
                                action = "order";
                                Order o = new Order(user.UserName, chosenRestaurant.StoreName);
                                while (action.Equals("order"))
                                {
                                    Console.WriteLine("p: Preset Pizza");
                                    Console.WriteLine("c: Custom Pizza");
                                    Console.WriteLine("d: Delete Pizza");
                                    Console.WriteLine("r: Cancel Pizza");
                                    Console.WriteLine("s: Show Pizza");
                                    Console.WriteLine("o: Order Pizza");

                                    userinput = Console.ReadLine();
                                    if (userinput.Equals("p"))
                                    {
                                        Pizza p = new Pizza();

                                    Preset:
                                        try
                                        {
                                            Console.WriteLine("Choose a preset pizza.");
                                            Console.WriteLine("krispy, supreme, meat lover, greek.");
                                            userinput = Console.ReadLine();
                                            p.PresetPizzas(userinput);
                                        }
                                        catch
                                        {
                                            goto Preset;
                                        }
                                    PresetSize:
                                        try
                                        {
                                            Console.WriteLine("Choose a size");
                                            Console.WriteLine("small, medium, large, extra large.");
                                            userinput = Console.ReadLine();
                                            p.Size = userinput;
                                        }
                                        catch
                                        {
                                            goto PresetSize;
                                        }
                                    PresetCrust:
                                        try
                                        {
                                            Console.WriteLine("Choose a crust");
                                            Console.WriteLine("thin, regular, or deep dish.");
                                            userinput = Console.ReadLine();
                                            p.Crust = userinput;
                                        }
                                        catch
                                        {
                                            goto PresetCrust;
                                        }
                                        o.PizzaCount(p);
                                    }
                                    else if (userinput.Equals("c"))
                                    {
                                        Pizza p = new Pizza();

                                    CustomSize:
                                        try
                                        {
                                            Console.WriteLine("Choose a side");
                                            Console.WriteLine("small, medium, large, extralarge");
                                            userinput = Console.ReadLine();
                                            p.Size = userinput;
                                        }
                                        catch
                                        {
                                            goto CustomSize;
                                        }

                                    CustomCrust:
                                        try
                                        {
                                            Console.WriteLine("Choose a crust.");
                                            Console.WriteLine("thin, regular, or deep dish.");
                                            userinput = Console.ReadLine();
                                            p.Crust = userinput;
                                        }
                                        catch
                                        {
                                            goto CustomCrust;
                                        }
                                        bool check = false;
                                        while (!check)
                                        {
                                            Console.WriteLine("Toppings Options: ");
                                            Console.WriteLine("a: Add topping");
                                            Console.WriteLine("d Delete topping");
                                            Console.WriteLine("s Show toppings");
                                            Console.WriteLine("o Finish toppings selection");
                                            userinput = Console.ReadLine();
                                            if (userinput.Equals("a"))
                                            {
                                                Console.WriteLine("Available toppings");
                                                foreach (string pt in p.toppingChoice)
                                                {
                                                    Console.Write($"{pt}, ");
                                                }

                                             
                                                userinput = Console.ReadLine();
                                                p.AddTopping(userinput);
                                            }
                                            else if (userinput.Equals("d"))
                                            {
                                                Console.WriteLine("Deleting toppings");
                                                Console.WriteLine(p.Toppings());
                                                userinput = Console.ReadLine();
                                                p.DeleteTopping(userinput);
                                            }
                                            else if (userinput.Equals("s"))
                                            {
                                                Console.WriteLine(p.Toppings());
                                            }
                                            else if (userinput.Equals("o"))
                                            {
                                                o.PizzaCount(p);
                                                check = true;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Please choose options a, d, s, o: ");
                                            }
                                        }
                                    }
                                    else if (userinput.Equals("d"))
                                    {
                                        Console.WriteLine(o.DisplayOrder());
                                        Console.WriteLine("How many pizzas to delete.");
                                        int num = Convert.ToInt32(Console.ReadLine());
                                        o.DeletePizza(num);
                                    }
                                    else if (userinput.Equals("r"))
                                    {
                                        action = "Login User";
                                    }
                                    else if (userinput.Equals("s"))
                                    {
                                        Console.WriteLine(o.DisplayOrder());
                                    }
                                    else if (userinput.Equals("o"))
                                    {
                                        Console.WriteLine(o.DisplayOrder());
                                        Console.WriteLine("Is your order complete. Please select (y/n): ");
                                        userinput = Console.ReadLine();
                                        if (userinput.Equals("y"))
                                        {
                                            action = "Login User";
                                            AddOrder(db, o);
                                            if(user.previousOrder is null)
                                            {
                                                user.previousOrder = new Dictionary<string, DateTime>();
                                                user.previousOrder[chosenRestaurant.StoreName] = DateTime.Now;
                                            }
                                            else
                                            {
                                                if(user.previousOrder.ContainsKey(chosenRestaurant.StoreName))
                                                {
                                                    user.previousOrder[chosenRestaurant.StoreName] = DateTime.Now;
                                                }
                                                else
                                                {
                                                    user.previousOrder.Add(chosenRestaurant.StoreName, DateTime.Now);
                                                }
                                            }
                                            Dictionaryupdate(db, user);

                                        }
                                        else { }
                                        
                                    }
                                    else
                                    {
                                        Console.WriteLine("Choose between options: p,c,r, and s");
                                    }
                                }
                            }

                            else if (userinput.Equals("9"))
                            {
                                int num = 1;
                                foreach (CustomerOrder orders in db.CustomerOrder)
                                {
                                    Order order2 = Mapping.Map(orders);
                                    if (order2.Username == user.UserName)
                                    {
                                        order2.CalcPrice();
                                        Console.WriteLine($"Order {num}, {order2.DisplayOrder()},\n Price: {order2.Price}");
                                        num++;
                                    }
                                }

                            }

                            else if (userinput.Equals("6"))
                            {
                                action = "home";
                            }
                            else
                            {
                                Console.WriteLine("Error! Please enter options 6, 7, 8, or 9.");
                            }
                        }
                    }
                }

                else if (userinput.Equals("2"))
                {
                    action = "admin restaurant";
                    while (action.Equals("admin restaurant"))
                    {
                        if (currRestaurant == null)
                        {
                            Console.WriteLine("Please choose an option: ");
                            Console.WriteLine("x: Existing Store.");
                            Console.WriteLine("y: New Store.");
                            Console.WriteLine("z: Naviagate Back.");
                            userinput = Console.ReadLine();
                            if (userinput.Equals("x"))
                            {
                                action = "Existing Store Login";
                                while (action.Equals("Existing Store Login"))
                                {
                                    Console.WriteLine("Please enter a store. Enter 'z' to navigate back.");
                                    string ar = Console.ReadLine();
                                    if (ar == "z")
                                    {
                                        action = "admin restaurant";
                                    }
                                    if (db.Store.Any(s => s.Storename == ar) && ar != "z")
                                    {
                                        Console.WriteLine("Enter store password.");
                                        string pw = Console.ReadLine();
                                        if (db.Store.Any(s => s.Storename == ar && s.Storepassword == pw))
                                        {
                                            var query = from s in db.Store
                                                        where s.Storename == ar && s.Storepassword == pw
                                                        select s;

                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Store doesn't exist.");
                                    }
                                }
                            }

                            else if (userinput.Equals("y"))
                            {
                                action = "new store";
                                while (action.Equals("new store"))
                                {
                                    string ar = "JayTest";
                                    string passw;
                                    bool different = false;
                                    while (!different)
                                    {
                                        different = true;
                                        Console.WriteLine("Please enter a store.");
                                        ar = Console.ReadLine();
                                        if (db.Store.Any(s => s.Storename == ar))
                                        {
                                            different = false;
                                            Console.WriteLine("Choose another store to add.");
                                        }
                                    }
                                    Console.WriteLine("Please Enter a password");
                                    passw = Console.ReadLine();
                                    Restaurant newstore = new Restaurant
                                    {
                                        StoreName = ar,
                                        Password = passw
                                    };
                                    AddStore(db, newstore);
                                    action = "admin restaurant";
                                    Console.WriteLine($"New store - Username: {ar}, Password: {passw}");
                                }
                            }

                            else if (userinput.Equals("z"))
                            {
                                action = "home";
                            }
                            else
                            {
                                Console.WriteLine("Error! Please enter x, y, z.");
                            }
                        }


                        else
                        {
                            Console.WriteLine("Please choose an option: ");
                            Console.WriteLine("j: Store Logout");
                            Console.WriteLine("k: Order History");
                            Console.WriteLine("b: Navigate Back");
                            userinput = Console.ReadLine();
                            if (userinput.Equals("j"))
                            {
                                currRestaurant = null;
                            }
                            else if (userinput.Equals("k"))
                            {
                                int oh = 1;
                                foreach (CustomerOrder orders in db.CustomerOrder)
                                {
                                    Order order1 = Mapping.Map(orders);
                                    if (order1.Storename == currRestaurant.StoreName)
                                    {
                                        order1.CalcPrice();
                                        Console.WriteLine($"Order {oh}, {order1.DisplayOrder()},\n Price: {order1.Price}");
                                        oh++;
                                    }
                                }
                            }

                            else if (userinput.Equals("b"))
                            {
                                action = "home";
                            }
                        }
                    }
                }
                else if (userinput.Equals("3"))
                {
                    execute = false;
                }
                else
                {
                    Console.WriteLine("Error! Please enter 1, 2, or 3.");
                }
            }
        }
        static IEnumerable<CustomerOrder> GetUserOrders(PizzaDbContext db, User u)
        {
            var query = from o in db.CustomerOrder
                        where o.Username == u.UserName
                        select o;
            return query;
        }
        static IEnumerable<CustomerOrder> GetStoreOrders(PizzaDbContext db, Restaurant s)
        {
            var query = from o in db.CustomerOrder
                        where o.Storename == s.StoreName
                        select o;
            return query;
        }
        static void NewUser(PizzaDbContext db, User u)
        {
            if (db.Customer.Any(c => c.Username == u.UserName))
            {
                Console.WriteLine($"{u.UserName} is taken.");
                return; //return no value if username exists in DB
            }
            else
            {
                Customer cust = Mapping.Map(u);
                db.Customer.Add(cust);
            }
            db.SaveChanges();
        }
        static void AddStore(PizzaDbContext db, Restaurant s)
        {
            if (db.Store.Any(sn => sn.Storename == s.StoreName) || s.StoreName == null)
            {
                Console.WriteLine($"{s.StoreName} exists.");
                return;   //return no value if store name exists
            }
            else
            {
                Store sn = Mapping.Map(s);
                db.Store.Add(sn);
            }
            db.SaveChanges();
        }
        static void AddOrder(PizzaDbContext db, Order o)
        {
            o.CalcPrice();
            CustomerOrder no = Mapping.Map(o);
            db.CustomerOrder.Add(no);
            db.SaveChanges();
        }

        
        static void DisplayRestaraunts(PizzaDbContext db)
        {
            foreach(var d in db.Store)
            {
                Console.WriteLine($"{d.Storename}");
            }
        }
        static void Dictionaryupdate(PizzaDbContext db, User us)
        {
            var query = from d in db.Customer
                      where d.Username == us.UserName
                      select d;
            foreach (Customer cus in query)
            {
                cus.Previousorder = JsonConvert.SerializeObject(us.previousOrder);
            }
            db.SaveChanges();
        }
    }
}

    

    


