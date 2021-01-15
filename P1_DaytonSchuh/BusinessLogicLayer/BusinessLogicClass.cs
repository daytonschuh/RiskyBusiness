using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ModelLayer.Models;
using ModelLayer.ViewModels;
using P1_DaytonSchuh;
using P1_DaytonSchuh.Data;
using P1_DaytonSchuh.Models;
using RepositoryLayer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class BusinessLogicClass
    {
        private readonly Repository _repository;
        private readonly MapperClass _mapperClass;
        private readonly UserManager<AppUser> _userManager;
        public BusinessLogicClass(Repository repository, MapperClass mapperClass, UserManager<AppUser> userManager)
        {
            _repository = repository;
            _mapperClass = mapperClass;
            _userManager = userManager;
        }

        public List<Location> GetAllLocations()
        {
            return _repository.GetAllLocations();
        }

        public List<CartItem> GetCartViewModel(int? id)
        {
            List<CartItem> list = _repository.GetCartItems(id);
            return list;
        }

        public List<Order> GetOrderHistoryByLocation(int locationId)
        {
            return _repository.GetOrderHistoryByLocation(locationId);
        }

        public void AddToCart(int id)
        {
            throw new NotImplementedException();
        }

        public OrderViewModel GetOrderViewModel(int? defLoc)
        {
            return _repository.GetOrderViewModel(defLoc);
        }

        /// <summary>
        /// dont touch this
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="defLoc"></param>
        /// <returns></returns>
        public List<ProductViewModel> GetProducts(string searchString, int defLoc)
        {
            // gets a list of products filtered by search string
            var list = _repository.GetProductsBySearchString(searchString, defLoc);

            List<ProductViewModel> pvmList = new List<ProductViewModel>();

            // convert List<Product> to List<ProductViewModel>
            foreach (var l in list)
            {
                pvmList.Add(_mapperClass.ConvertProductToProductViewModel(l, 0));
            }

            // return List<ProductViewModel>
            return pvmList;
        }

        public List<ProductViewModel> GetSpecificOrderHistory(string s, int id)
        {
            return _repository.GetSpecificOrderHistory(s, id);
        }

        public List<OrderViewModel> GetUserOrderHistory(string s)
        {
            List<OrderViewModel> ovm = new List<OrderViewModel>();
            List<Order> ol = _repository.GetOrderListByUser(s);
            foreach (var o in ol)
            {
                ovm.Add(_mapperClass.ConvertOrderToOrderViewModel(o));
            }
            return ovm;
        }

        public void HandleOrder(int? defLoc, string userId, int? cartId)
        {
            AppUser user = _repository.GetUserById(userId);
            user.ShoppingCart = GetCart(userId);
            Location loc = _repository.GetLocationById(defLoc);
            _repository.HandleOrder(user, loc);
        }

        public List<UserViewModel> GetUsers()
        {
            var list = _repository.GetAllUsers();
            List<UserViewModel> uvm = new List<UserViewModel>();

            foreach(var l in list)
            {
                uvm.Add(_mapperClass.ConvertUserToUserViewModel(l));
            }
            return uvm;
        }

        public List<UserViewModel> GetUsers(string searchString)
        {
            var list = _repository.GetUsersBySearchString(searchString);
            List<UserViewModel> uvm = new List<UserViewModel>();

            foreach (var l in list)
            {
                uvm.Add(_mapperClass.ConvertUserToUserViewModel(l));
            }
            return uvm;
        }

        public void AddedProduct(ProductViewModel productViewModel)
        {
            throw new NotImplementedException();
        }

        public ShoppingCart GetCartNA(string userId)
        {
            return _repository.GetCartNA(userId);
        }

        public UserViewModel AddUser(AddUserViewModel addUserViewModel)
        {
            AppUser user = new AppUser()
            {
                FirstName = addUserViewModel.FirstName,
                LastName = addUserViewModel.LastName,
                Email = addUserViewModel.Email,
                PasswordHash = _repository.HashPassword(addUserViewModel.Password),
            };

            AppUser u = _repository.AddUser(user);

            UserViewModel uvm = _mapperClass.ConvertUserToUserViewModel(u);
            return uvm;
        }

        public ShoppingCart GetCart(string userId)
        {
            return _repository.GetCart(userId);
        }

        public void AddedToCart(ShoppingCart cart, ProductViewModel productViewModel)
        {
            CartItem ci = _mapperClass.ConvertProductViewModelToCartItem(cart, productViewModel);
            _repository.AddToCart(ci);
        }

        public void AssignUserToCart(string id, int cartId)
        {
            _repository.AssignUserToCart(id, cartId);
        }

        public List<ProductViewModel> GetProducts()
        {
            var list = _repository.GetAllProducts();

            List<ProductViewModel> pvm = new List<ProductViewModel>();

            foreach (var l in list)
            {
                pvm.Add(_mapperClass.ConvertProductToProductViewModel(l, 0));
            }
            return pvm;
        }

        public ProductViewModel AddProduct(AddProductViewModel addProductViewModel)
        {
            Product product = new Product()
            {
                ProductPicture = addProductViewModel.ProductPicture,
                Name = addProductViewModel.Name,
                Price = addProductViewModel.Price,
                Description = addProductViewModel.Description
            };

            Product p = _repository.AddProduct(product);

            ProductViewModel pvm = _mapperClass.ConvertProductToProductViewModel(p, 0);
            return pvm;
        }

        public async Task<UserViewModel> EditedUser(UserViewModel userViewModel)
        {            
            // get an instance of the customer being edited
            AppUser appUser = _repository.GetUserById(userViewModel.Id);

            // create a player and insert the changed details
            // make sure to convert the byte array to a jpeg string image

            appUser.ProfilePicture = userViewModel.ProfilePicture;
            appUser.UserName = userViewModel.UserName;
            appUser.FirstName = userViewModel.FirstName;
            appUser.LastName = userViewModel.LastName;

            AppUser u = await _repository.EditUser(appUser);

            await _userManager.UpdateAsync(u);

            UserViewModel uvm = _mapperClass.ConvertUserToUserViewModel(u);

            return uvm;
        }

        public UserViewModel EditUser(string id)
        {
            // call a method in repository that will return a product based on the id
            AppUser user = _repository.GetUserById(id);

            // map the product to a productviewmodel
            UserViewModel userViewModel = _mapperClass.ConvertUserToUserViewModel(user);

            return userViewModel;
        }

        public void DeleteUser(string id)
        {
            _repository.DeleteUser(id);
        }

        public ProductViewModel EditProduct(int id)
        {
            // call a method in repository that will return a product based on the id
            Product product = _repository.GetProductById(id);

            // map the product to a productviewmodel
            ProductViewModel productViewModel = _mapperClass.ConvertProductToProductViewModel(product, 0);

            return productViewModel;
        }

        public ProductViewModel EditedProduct(ProductViewModel productViewModel)
        {
            // get an instance of the customer being edited
            Product product = _repository.GetProductById(productViewModel.Id);

            // create a player and insert the changed details
            // make sure to convert the byte array to a jpeg string image

            product.ProductPicture = productViewModel.ProductPicture;
            product.Name = productViewModel.Name;
            product.Price = productViewModel.Price;
            product.Description = productViewModel.Description;

            Product p = _repository.EditProduct(product);

            ProductViewModel pvm = _mapperClass.ConvertProductToProductViewModel(p, 0);

            return pvm;
        }

        public void DeleteProduct(int id)
        {
            _repository.DeleteProduct(id);
        }

        public List<LocationLineViewModel> GetHotDeals()
        {
            List<LocationLine> lll = _repository.GetHotDeals();
            List<LocationLineViewModel> ll = _mapperClass.ConvertLocationLineListToViewModel(lll);
            return ll;
        }
    }
}
