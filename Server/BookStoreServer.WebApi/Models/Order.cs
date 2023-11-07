using BookStoreServer.WebApi.ValueObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreServer.WebApi.Models
{
   
    public sealed class Order 
    {
       
        public int Id { get; set; }
        public string OrderNumber { get; set; }//16 Hane Unique olacak
        [ForeignKey("Book")]
        public int BookId { get; set; }
        public Book Book { get; set; }

        public Money Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentType { get; set; }
        public string PaymentNumber { get; set; }
        public static string GetNewOrderNumber()
        {

            return Guid.NewGuid().ToString();
        }

    }
}
