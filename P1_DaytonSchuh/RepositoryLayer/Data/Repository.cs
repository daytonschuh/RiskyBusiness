using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ModelLayer.Models;
using ModelLayer.ViewModels;
using P1_DaytonSchuh;
using P1_DaytonSchuh.Data;
using P1_DaytonSchuh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace RepositoryLayer
{
    public class Repository
    {
        // This is dependency injection --
        private ApplicationDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;

        // Logger
        private readonly ILogger<Repository> _logger;

        DbSet<AppUser> users;
        DbSet<Product> products;
        DbSet<Order> orders;
        DbSet<OrderLine> orderLines;
        DbSet<Location> locations;
        DbSet<LocationLine> locationLines;
        DbSet<ShoppingCart> shoppingCarts;

        public List<Location> GetAllLocations()
        {
            return locations.ToList();
        }

        DbSet<CartItem> shoppingCartItems;

        public Repository(UserManager<AppUser> userManager, ApplicationDbContext dbContextClass, ILogger<Repository> logger)
        {
            _userManager = userManager;
            _dbContext = dbContextClass;
            this.users = _dbContext.ExtendedUsers;
            this.products = _dbContext.Products;
            this.orders = _dbContext.Orders;
            this.orderLines = _dbContext.OrderLines;
            this.locations = _dbContext.Locations;
            this.locationLines = _dbContext.LocationLines;
            this.shoppingCarts = _dbContext.ShoppingCarts;
            this.shoppingCartItems = _dbContext.ShoppingCartItems;
            _logger = logger;
        }

        public List<Order> GetOrderHistoryByLocation(int locationId)
        {
            return orders.Where(x => x.LocationId == locationId).ToList();
        }

        public OrderViewModel GetOrderViewModel(int? defLoc)
        {
            OrderViewModel ovm = new OrderViewModel();
            ovm.Location = locations.SingleOrDefault(x=>x.Id == defLoc);
            ovm.OrderDate = DateTime.Now;
            return ovm;
        }

        public List<CartItem> GetCartViewModel(int? id)
        {
            throw new NotImplementedException();
        }

        public List<CartItem> GetCartItems(int? id)
        {
            return shoppingCartItems.Where(x=>x.CartId == id).ToList();
        }

        public List<ProductViewModel> GetSpecificOrderHistory(string s, int id)
        {
            List<ProductViewModel> pvm = new List<ProductViewModel>();
            List<OrderLine> ol = orderLines.Where(x=>x.CustomerId == s).Where(x=>x.OrderId==0).ToList();


            foreach(var o in ol)
            {
                //map ol to pvm
                pvm.Add(new ProductViewModel()
                {
                    Id = o.ProductId,
                    Quantity = o.Quantity,
                    Name = products.FirstOrDefault(x => x.Id == o.ProductId).Name,
                    ProductPicture = products.FirstOrDefault(x => x.Id == o.ProductId).ProductPicture,
                    Price = products.FirstOrDefault(x => x.Id == o.ProductId).Price
                }) ;
            }

            return pvm;
        }

        public List<Order> GetOrderListByUser(string s)
        {
            return orders.Where(x=>x.CustomerId==s).ToList();
        }

        public List<OrderLine> GetOrderLines(string id)
        {
            return orderLines.Where(x => x.CustomerId == id).ToList();
        }

        public Location GetLocationById(int? defLoc)
        {
            return locations.SingleOrDefault(x=>x.Id == defLoc);
        }

        public List<OrderLine> GetOrderViewModel(string id)
        {
            return orderLines.Where(x=>x.CustomerId == id).ToList();
        }

        /// <summary>
        /// dont touch this
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="defLoc"></param>
        /// <returns></returns>
        public List<Product> GetProductsBySearchString(string searchString, int defLoc)
        {
            List<LocationLine> ll = locationLines.Where(x => x.LocationId == defLoc).ToList();
            List<Product> lp = new List<Product>();
            foreach(var l in ll)
            {
                //if(!lp.Contains(products.Where(x=>x.Id == l.ProductId).SingleOrDefault()))
                    lp.Add(products.Where(p => p.Id == l.ProductId).SingleOrDefault());
            }

            if (searchString != null)
            {
                List<Product> lp2 = new List<Product>();
                foreach (var p in lp)
                {
                    
                    if(p.Name.ToUpper().Contains(searchString.Trim().ToUpper()))
                        lp2.Add(p);
                }
                return lp2;
            }
            return lp;
        }

        public ShoppingCart GetCartNA(string userId)
        {
            var cart = shoppingCarts.FirstOrDefault(x => x.CustomerId == userId);
            return cart;
        }

        public List<AppUser> GetAllUsers()
        {
            return users.ToList();
        }

        public List<OrderLine> GetOrderLinesByOrder(Order o)
        {
            return orderLines.Where(x => x.OrderId == o.Id).ToList();
        }

        public Product GetProductById(int id)
        {
            return products.FirstOrDefault(x => x.Id == id);
        }

        public List<AppUser> GetUsersBySearchString(string searchString)
        {
            return users.Where(p => p.UserName.Contains(searchString)).ToList();
        }

        public List<Product> GetAllProducts()
        {
            return products.ToList();
        }

        public ShoppingCart GetCart(string userId)
        {
            return shoppingCarts.SingleOrDefault(x=>x.CustomerId == userId);
        }

        public void AddToCart(CartItem ci)
        {
            shoppingCartItems.Add(ci);
            _dbContext.SaveChanges();
        }

        public void AssignUserToCart(string id, int cartId)
        {
            ShoppingCart cart = shoppingCarts.Where(x=>x.CustomerId==id).SingleOrDefault();
            if (cart == null)
            {
                ShoppingCart scart = new ShoppingCart(id);
                shoppingCarts.Add(scart);
                _dbContext.SaveChanges();
            }
            else
            {
                cart.CustomerId = id;
            }
        }

        public Product AddProduct(Product p)
        {
            // Names are unique to the product
            if (products.SingleOrDefault(x => x.Name == p.Name) != null)
            {
                _logger.LogInformation("Product with that name already exists.");
                // reject request
                return null;
            }
            else
            {
                // Create new product
                Product product = new Product()
                {
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description
                };

                // add the product
                _dbContext.Add(product);
                _dbContext.SaveChanges();
                return product;
            }
        }

        public string HashPassword(string password)
        {
            int SaltByteSize = 24;
            int HashByteSize = 24;
            int HasingIterationsCount = 10101;
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, SaltByteSize, HasingIterationsCount))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(HashByteSize);
            }
            byte[] dst = new byte[(SaltByteSize + HashByteSize) + 1];
            Buffer.BlockCopy(salt, 0, dst, 1, SaltByteSize);
            Buffer.BlockCopy(buffer2, 0, dst, SaltByteSize + 1, HashByteSize);
            return Convert.ToBase64String(dst);
        }

        public AppUser GetUserByUserName(string userName)
        {
            return users.FirstOrDefault(x => x.UserName == userName);
        }

        public AppUser GetUserById(string id)
        {
            return users.Where(x => x.Id == id).FirstOrDefault();
        }

        public async Task<AppUser> EditUser(AppUser u)
        {
            AppUser p = GetUserById(u.Id);

            // Replace with new values
            if (p != null)
            {
                p.ProfilePicture = u.ProfilePicture;
                p.UserName = u.UserName;
                p.FirstName = u.FirstName;
                p.LastName = u.LastName;
                p.Email = u.Email;
                p.Address = u.Address;
                p.City = u.City;
                p.Country = u.Country;
                p.State = u.State;
            }
            else
            {
                // Something went wrong. Write to the logger
            }


            await _userManager.UpdateAsync(p);
            // Save changes to database
            _dbContext.SaveChanges();

            // Search for customer again to verify that changes were saved
            AppUser tempUser = GetUserById(p.Id);

            // Return edited player
            return tempUser;
        }

        public void DeleteUser(string id)
        {
            AppUser u = users.SingleOrDefault(x => x.Id == id);

            if (u != null)
            {
                _dbContext.Remove(u);
                _dbContext.SaveChanges();
            }
            else
            {
                _logger.LogInformation("Bruh whut. This shouldn't even be possible. Somehow didn't, but did delete a product.");
            }
        }

        public AppUser AddUser(AppUser user)
        {
            // usernames are unique
            if (users.SingleOrDefault(x => x.UserName == user.UserName) != null)
            {
                _logger.LogInformation("That username is taken!");
                // reject request
                return null;
            }
            else
            {
                // Create new user
                AppUser user1 = new AppUser()
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PasswordHash = user.PasswordHash,
                };

                // add the user
                _dbContext.Add(user1);
                _dbContext.SaveChanges();
                return user1;
            }
        }

        public Product EditProduct(Product product)
        {
            // Search the database for a customer
            Product p = GetProductById(product.Id);

            // Replace with new values
            if (p != null)
            {
                p.ProductPicture = product.ProductPicture;
                p.Name = product.Name;
                p.Price = product.Price;
                p.Description = product.Description;
            }
            else
            {
                // Something went wrong. Write to the logger
            }

            // Save changes to database
            _dbContext.SaveChanges();

            // Search for customer again to verify that changes were saved
            Product tempProduct = GetProductById(product.Id);

            // Return edited player
            return tempProduct;
        }

        public void DeleteProduct(int id)
        {
            Product p = products.SingleOrDefault(x => x.Id == id);

            if (p != null)
            {
                _dbContext.Remove(p);
                _dbContext.SaveChanges();
            }
            else
            {
                _logger.LogInformation("Bruh whut. This shouldn't even be possible. Somehow didn't, but did delete a product.");
            }
        }

        /// <summary>
        /// Alter quantity of a given location's associated product by given amount.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="product"></param>
        /// <param name="amount"></param>
        public void SubtractProductFromLocation(Location location, Product product, int amount)
        {
            LocationLine locationLine = locationLines.Where(x => x.LocationId == location.Id && x.ProductId == product.Id).FirstOrDefault();
            locationLine.Quantity -= amount;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Alter quantity of a given location's associated product by given amount.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="product"></param>
        /// <param name="amount"></param>
        public void AddProductToLocation(Location location, Product product, int amount)
        {
            LocationLine locationLine = locationLines.Where(x => x.LocationId == location.Id && x.ProductId == product.Id).FirstOrDefault();
            locationLine.Quantity += amount;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Returns the quantity of a product on a specified location line.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="l"></param>
        /// <returns>int</returns>
        public int QuantityOfProduct(Product p, LocationLine l)
        {
            return locationLines.Where(x => x.ProductId == p.Id && x.Id == l.Id).FirstOrDefault().Quantity;
        }

        /// <summary>
        /// Returns the location the user wishes to access.
        /// </summary>
        /// <param name="i"></param>
        /// <returns>Location</returns>
        public Location SetUserLocation(int i)
        {
            return locations.Where(x => x.Id == i).FirstOrDefault();
        }

        /// <summary>
        /// Returns an AppUser where specified names are similar in our database.
        /// </summary>
        /// <param name="names"></param>
        /// <returns>AppUser</returns>
        public AppUser FindUsers(string name)
        {
            //return users.Where();
            throw new NotImplementedException();
        }

        public List<LocationLine> GetHotDeals()
        {
            if (locationLines.Count() > 0)
            {
                // from products
                // pick 3 random ids and store them to the list
                List<Product> pl = new List<Product>();
                Random rnd = new Random();
                for (int i = 0; i < 3; i++)
                {
                    int fml = rnd.Next(1, products.Count());
                    // product doesn't exist in any of our location lines
                    if (locationLines.Where(x => x.ProductId == fml).FirstOrDefault() == null) { i--; }
                    // add the product to our hot deal list
                    else
                    {
                        pl.Add(products.SingleOrDefault(x => x.Id == fml));
                    }
                }
                // for each product in the list, find the cheapest price in location inventories
                // and store that in our list of location lines
                List<LocationLine> lll = new List<LocationLine>();

                foreach (Product p in pl)
                {
                    List<LocationLine> ll = locationLines.Where(x => x.ProductId == p.Id).ToList();
                    decimal pricetoBeat = ll[0].Price;
                    int index = 0;
                    for (int i = 1; i < ll.Count(); i++)
                    {
                        if (pricetoBeat > ll[i].Price)
                        {
                            pricetoBeat = ll[i].Price;
                            index = i;
                        }
                    }
                    lll.Add(ll.ElementAt(index));
                }
                // return the list of location lines
                return lll;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a list of all location lines of a specified location.
        /// </summary>
        /// <param name="location"></param>
        /// <returns>List(LocationLine)</returns>
        public List<LocationLine> GetLocationLines(Location location)
        {
            return locationLines.Where(x => x.LocationId == location.Id).ToList();
        }

        /// <summary>
        /// Returns the product of a specified location line.
        /// </summary>
        /// <param name="locationLine"></param>
        /// <returns>Product</returns>
        public Product GetSpecificProduct(LocationLine locationLine)
        {
            return products.Where(x => x.Id == locationLine.ProductId).FirstOrDefault();
        }

        /// <summary>
        ///  Create a new order, process and package it for delivery to the database.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="location"></param>
        /// <param name="product"></param>
        /// <param name="amount"></param>
        public void HandleOrder(AppUser customer, Location location)
        {
            // create order instance
            Order order = new Order(customer.Id, location.Id, DateTime.Now);
            customer.ShoppingCart = GetCart(customer.Id);
            List<CartItem> cartList = shoppingCartItems.Where(x=>x.CartId == customer.ShoppingCart.ShoppingCartId).ToList();
            foreach(var c in cartList)
            {
                if (c.Quantity < (locationLines.SingleOrDefault(x => x.LocationId == location.Id && x.ProductId == c.ProductId).Quantity))
                {
                    orderLines.Add(new OrderLine(order.Id, order.CustomerId, c.ProductId, products.SingleOrDefault(x => x.Id == c.ProductId).Price, c.Quantity));
                    SubtractProductFromLocation(location, products.SingleOrDefault(x => x.Id == c.ProductId), c.Quantity);
                    shoppingCartItems.Remove(c);
                }
            }

            orders.Add(order);

            // commit to db
            _dbContext.SaveChanges();
        }
    }
}
