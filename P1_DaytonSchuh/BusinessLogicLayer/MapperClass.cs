using System;
using Microsoft.AspNetCore.Http;
using System.IO;
using P1_DaytonSchuh.Models;
using System.Collections.Generic;
using RepositoryLayer;
using ModelLayer.ViewModels;
using P1_DaytonSchuh;
using ModelLayer.Models;
using System.Linq;

namespace BusinessLogicLayer
{
    public class MapperClass
    {
        private readonly Repository _repository;

        public MapperClass(Repository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Takes a byte array and returns a string representative of the jpeg image.
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        private string ConvertByteArrayToJpegString(byte[] byteArray)
        {
            if (byteArray != null)
            {
                string imageBase64Data = Convert.ToBase64String(byteArray, 0, byteArray.Length);
                string imageDataURL = string.Format($"data:image/jpg;base64, {imageBase64Data}");
                return imageDataURL;
            }

            else return null;
        }

        public List<ProductViewModel> ConvertCartItemsToProductViewModel(List<CartItem> cartViewModel)
        {
            List<ProductViewModel> pvm = new List<ProductViewModel>();

            foreach(var l in cartViewModel)
            {
                Product p = _repository.GetProductById(l.ProductId);
                ProductViewModel pp = pvm.SingleOrDefault(x=>x.Id == ConvertProductToProductViewModel(p, l.Quantity).Id);
                if (pp == null)
                    pvm.Add(ConvertProductToProductViewModel(p,l.Quantity));
                else pvm.Find(x => x.Id == ConvertProductToProductViewModel(p,l.Quantity).Id).Quantity += l.Quantity;
            }

            return pvm;
        }

        public byte[] ConvertIformFileToByteArray(IFormFile iformFile)
        {
            using (var ms = new MemoryStream())
            {
                // convert the IFormFile into a byte[]
                iformFile.CopyTo(ms);

                if (ms.Length > 2097152)// if it's bigger that 2 MB
                {
                    return null;
                }
                else
                {
                    byte[] a = ms.ToArray(); // put the string into the Image property
                    return a;
                }
            }
        }

        public OrderViewModel ConvertOrderToOrderViewModel(Order o) {
            try
            {
                List<OrderLine> ol = _repository.GetOrderLinesByOrder(o);
                OrderViewModel ovm = new OrderViewModel()
                {
                    OrderId = o.Id,
                    OrderLines = ol,
                    Total = o.Total,
                    OrderDate = o.OrderDate
                };
                
                return ovm;
            }
            catch (Exception e)
            {

            }
            return null;
        }

        internal UserViewModel ConvertUserToUserViewModel(AppUser u)
        {
            try
            {
                UserViewModel uvm = new UserViewModel()
                {
                    Id = u.Id,
                    Email = u.Email,
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Address = u.Address,
                    City = u.City,
                    State = u.State,
                    DefaultLocation = u.DefaultLocation,
                    Country = u.Country,
                    ProfilePicture = u.ProfilePicture,
                    UsernameChangeLimit = u.UsernameChangeLimit,

                };
                return uvm;
            }
            catch (Exception e)
            {

            }
            return null;
        }

        public ProductViewModel ConvertProductToProductViewModel(Product p, int quantity)
        {
            try
            {
                ProductViewModel pvm = new ProductViewModel()
                {
                    ProductPicture = p.ProductPicture,
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    Quantity = quantity
                };
                return pvm;
            }
            catch (Exception e)
            {

            }
            return null;
        }

        internal CartItem ConvertProductViewModelToCartItem(ShoppingCart cart, ProductViewModel productViewModel)
        {
            CartItem ci = new CartItem();

            ci.CartId = cart.ShoppingCartId;
            ci.ProductId = productViewModel.Id;
            ci.Quantity = productViewModel.Quantity;

            return ci;
        }

        public List<LocationLineViewModel> ConvertLocationLineListToViewModel(List<LocationLine> lll)
        {
            if (lll != null)
            {
                List<LocationLineViewModel> lvm = new List<LocationLineViewModel>();
                foreach (var l in lll)
                {
                    lvm.Add(new LocationLineViewModel()
                    {
                        Id = l.Id,
                        LocationId = l.LocationId,
                        ProductId = l.ProductId,
                        Price = l.Price,
                        Quantity = l.Quantity,
                        product = _repository.GetProductById(l.Id)
                    });
                }
                return lvm;
            }
            else return null;
        }
    }
}
