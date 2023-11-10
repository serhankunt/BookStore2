using BookStoreServer.WebApi.Context;
using BookStoreServer.WebApi.Dtos;
using BookStoreServer.WebApi.Enums;
using BookStoreServer.WebApi.Models;
using BookStoreServer.WebApi.Services;
using BookStoreServer.WebApi.ValueObject;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookStoreServer.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public sealed class ShoppingCartsController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Payment(PaymentDto requestDto)
        {
            decimal total = 0;
            decimal comission = 0;
            foreach (var book in requestDto.Books)
            {
                total += book.Price.Value;
            }
            comission = total * 1.2m;

            Currency currency = Currency.TRY;
            string requestCurrency = requestDto.Books[0]?.Price.Currency;
            if (!string.IsNullOrEmpty(requestCurrency))
            {
                switch (requestCurrency)
                {
                    case "₺":
                        currency = Currency.TRY;
                        break;
                    case "$":
                        currency = Currency.USD;
                        break;
                    case "£":
                        currency = Currency.GBP;
                        break;
                    default:
                        throw new Exception("Para birimi bulunamadı");
                        
                }
            }
            else
            {
                throw new Exception("Sepette ürününüz yok!");
            }

            //Bağlantı bilgilerini istiyor
            Iyzipay.Options options = new Iyzipay.Options();
            options.ApiKey = "sandbox-MBmzWVOeil9arc1EVT1PLGRh07ARuxGr";
            options.SecretKey = "DW8050suGcchWnAoveQoglj4YfUq7NHi";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";

            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = Guid.NewGuid().ToString();
            request.Price = total.ToString();//Ödeme kısmı
            request.PaidPrice = comission.ToString();//komisyon+ödeme tutarı
            request.Currency = currency.ToString();
            request.Installment = 1;
            request.BasketId = Order.GetNewOrderNumber();
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            PaymentCard paymentCard = requestDto.PaymentCard;
            request.PaymentCard = paymentCard;



            Buyer buyer = requestDto.Buyer;
            buyer.Id = Guid.NewGuid().ToString();
            request.Buyer = buyer;


            request.ShippingAddress = requestDto.ShippingAddress;

            request.BillingAddress = requestDto.BillingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();
            foreach (var book in requestDto.Books)
            {
                BasketItem item = new BasketItem();
                item.Category1 = "Book";
                item.Category2 = "Book";
                item.Id = book.Id.ToString();
                item.Name = book.Title;
                item.ItemType = BasketItemType.PHYSICAL.ToString();
                item.Price = book.Price.Value.ToString();
                basketItems.Add(item);
            }

            request.BasketItems = basketItems;

            Payment payment = Iyzipay.Model.Payment.Create(request, options);
            if (payment.Status == "success")
            {
                string orderNumber = Order.GetNewOrderNumber();

                List<Order> orders = new();
                
                foreach (var book in requestDto.Books)
                {
                    Order order = new()
                    {
                        OrderNumber = orderNumber,
                        BookId = book.Id,
                        Price = new Money(book.Price.Value, book.Price.Currency),
                        PaymentDate = DateTime.Now,
                        PaymentType = request.PaymentGroup,
                        PaymentNumber = payment.PaymentId,
                        CreatedAt = DateTime.Now
                       
                    };
                    orders.Add(order);
                }

                AppDbContext context = new();
                OrderStatus orderStatus = new()
                {
                    OrderNumber = orderNumber,
                    Status = OrderStatusEnum.AwaitingApproval,
                    StatusDate = DateTime.Now
                };


                context.OrderStatuses.Add(orderStatus);
                context.Orders.AddRange(orders);
                context.SaveChanges();

                string response = await MailService.SendEmailAsync(requestDto.Buyer.Email, "Siparişiniz alındı",$@"
                <h1>Siparişiniz alındı<h1>
                <p>Sipariş numaranız:{orderNumber}<p> 
                <p>Ödeme numaranız:{payment.PaymentId}<p>
                <p>Ödeme Tutarınız:{payment.PaidPrice}<p>
                <p>Ödeme tarihiniz:{DateTime.Now}<p>
                   <p>Ödeme Tipiniz: Kredi Kartı<p>
                <p>Ödeme durumunuz:Onay Bekliyor<p>");

                //smtp=>mail sistemin

                return NoContent();

            }
            else
            {
                return BadRequest(payment.ErrorMessage);
    }
            //payment status :success or failure
            //ErrorMessage: Hata mesajı var.
           
        }
    }
}
