using System;
using System.ComponentModel.DataAnnotations;

namespace P1_DaytonSchuh.Models
{
    public class Order
    {
        // default constructor
        public Order() { }

        // overloaded constructor with customerid, locationid, orderdate and total
        public Order(string customerId, int locId, DateTime dateTime, Decimal total)
        {
            this.CustomerId = customerId;
            this.LocationId = locId;
            this.OrderDate = dateTime;
            this.Total = total;
        }

        public Order(string customerId, int locationId, DateTime dateTime)
        {
            this.CustomerId = customerId;
            this.LocationId = locationId;
            this.OrderDate = dateTime;
        }

        // overloaded constructor with customerid, locationid, orderdate, address, city, state, country, and total
        public Order(string customerId, int locId, DateTime dateTime, string add, string city, string state, string country, Decimal total)
        {
            this.CustomerId = customerId;
            this.LocationId = locId;
            this.OrderDate = dateTime;
            this.BillingAddress = add;
            this.BillingCity = city;
            this.BillingState = state;
            this.BillingCountry = country;
            this.Total = total;
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Customer")]
        public string CustomerId { get; set; }
        [Required]
        [Display(Name = "Location")]
        public int LocationId { get; set; }
        [Required]
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }
        [Display(Name = "Billing Address")]
        public string BillingAddress { get; set; }
        [Display(Name = "City")]
        public string BillingCity { get; set; }
        [Display(Name = "State")]
        public string BillingState { get; set; }
        [Display(Name = "Country")]
        public string BillingCountry { get; set; }
        [Required]
        public decimal Total { get; set; }
    }
}
