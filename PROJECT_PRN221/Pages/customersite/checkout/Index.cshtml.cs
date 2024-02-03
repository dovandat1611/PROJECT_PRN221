using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PROJECT_PRN221.Dto;
using PROJECT_PRN221.Models;
using System.Runtime.CompilerServices;

namespace PROJECT_PRN221.Pages.customersite.checkout
{
    public class IndexModel : PageModel
    {

        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;

        public IndexModel(PROJECT_PRN221.Models.ProjectPrn221Context context)
        {
            _context = context;
        }
        public Customer Customer { get; set; }
        public double totalprice { get; set; }
        public async Task OnGetAsync()
        {
            string customerJson = HttpContext.Session.GetString("customer");
            string cartJson = HttpContext.Session.GetString("cart");


            if (!string.IsNullOrEmpty(customerJson))
            {
                Customer = JsonConvert.DeserializeObject<Customer>(customerJson);
            }

            if (!string.IsNullOrEmpty(cartJson))
            {
                List<Cart> cartList = JsonConvert.DeserializeObject<List<Cart>>(cartJson);
                totalprice = cartList.Sum(x => x.subtotal);
            }

        }

        public async Task<IActionResult> OnPostAsync(int id, double total_price, string status, string name_receiver, 
            string phone_receiver, string address_receiver, string payment)
        {
            if(payment.Equals("Ship COD"))
            {
                int orderId = 0;
                // Add ORDER
                Order order = new Order
                {
                    CustomerId = id,
                    NameReceiver = name_receiver,
                    PhoneReceiver = phone_receiver,
                    AddressReceiver = address_receiver,
                    TotalPrice = total_price,
                    OderDate = DateTime.Now,
                    Payment = payment,
                    Status = status,
                };

                if (_context != null)
                {
                    _context.Orders.Add(order);
                    _context.SaveChanges();
                    orderId = order.OrderId;
                }

                // Add ORDER DETAIL
                string cartJson = HttpContext.Session.GetString("cart");
                if (!string.IsNullOrEmpty(cartJson))
                {
                    var cartList = JsonConvert.DeserializeObject<List<Cart>>(cartJson);
                    foreach (var item in cartList)
                    {
                        OrderDetail orderDetail = new OrderDetail
                        {
                            OrderId = orderId,
                            ProductId = item.id,
                            ListPrice = item.subtotal,
                            QuantityOrder = item.quantity,
                        };

                        if (_context != null)
                        {
                            _context.OrderDetails.Add(orderDetail);
                            _context.SaveChanges();
                        }
                    }
                }
                HttpContext.Session.Remove("cart");
            }
            if (payment.Equals("Payment by Card"))
            {
                //PaymentService _paymentService = new PaymentService();
                //double amount = total_price;
                //string paymentUrl = _paymentService.ProcessPayment(amount);
                //Redirect(paymentUrl);
            }
            return RedirectToPage("/customersite/cart/Index");
        }
    }
}
