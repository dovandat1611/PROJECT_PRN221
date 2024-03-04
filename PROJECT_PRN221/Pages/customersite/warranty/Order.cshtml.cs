using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PROJECT_PRN221.Dto;
using PROJECT_PRN221.Models;

namespace PROJECT_PRN221.Pages.customersite.warranty
{
    public class OrderModel : PageModel
    {
        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;

        public OrderModel(PROJECT_PRN221.Models.ProjectPrn221Context context)
        {
            _context = context;
        }
        public Customer Customer { get; set; }
        public List<OrderWarranty> Warranties { get; set; }

        public bool checkSession()
        {
            bool checkS = true;
            var httpContext = HttpContext;
            if (httpContext != null && httpContext.Session != null)
            {
                string isCustomerAuthenticated = httpContext.Session.GetString("customer");
                if (string.IsNullOrEmpty(isCustomerAuthenticated))
                {
                    checkS = false;
                }
            }

            return checkS;
        }

        public IActionResult OnGet()
        {   

            if (checkSession() == false)
            {
                return RedirectToPage("/customersite/authenticate/login/Index");
            }

            string customerJson = HttpContext.Session.GetString("customer");
            if (!string.IsNullOrEmpty(customerJson))
            {
                Customer = JsonConvert.DeserializeObject<Customer>(customerJson);
            }

            List<Warranty> warranties = OrderWarrantyCheck(Customer.CustomerId);
            if (warranties.Count > 0)
            {
                Warranties = OrderWarrantyIFNOTNULL(Customer.CustomerId);
            }
            else
            {
                Warranties = OrderWarrantyIFNULL(Customer.CustomerId);
            }

            return Page();
        }

        public List<Warranty> OrderWarrantyCheck(int id)
        {
            var list = _context.Warranties
                .Where(warranty => warranty.CustomerId == id)
                .ToList();
            return list;
        }

        public List<OrderWarranty> OrderWarrantyIFNULL(int id)
        {
            var list = (from od in _context.OrderDetails
                        join o in _context.Orders on od.OrderId equals o.OrderId
                        join p in _context.Products on od.ProductId equals p.ProductId // Include Product
                        where o.CustomerId == id && o.Status == "Done"
                        group new { od, o, p } by new
                        {
                            od.OrderdetailId,
                            od.Order.OrderId,
                            od.ProductId,
                            p.ProductName,
                            p.Image,
                            od.QuantityOrder,
                            o.OderDate
                        } into g
                        where !_context.Warranties.Any(w => w.OrderdetailId == g.Key.OrderdetailId)
                        select new OrderWarranty
                        {
                            Orderdetail_id = g.Key.OrderdetailId,
                            Customer_id = id,
                            Product_id = g.Key.ProductId.Value,
                            Product_name = g.Key.ProductName,
                            Image = g.Key.Image,
                            Quantity_order = g.Key.QuantityOrder.Value,
                            Order_date = g.Key.OderDate.Value
                        }).ToList();

            return list;
        }

        public List<OrderWarranty> OrderWarrantyIFNOTNULL(int id)
        {
            var list = (from a in
                            (from od in _context.OrderDetails
                             join o in _context.Orders on od.OrderId equals o.OrderId
                             join p in _context.Products on od.ProductId equals p.ProductId // Include Product
                             where o.CustomerId == id && o.Status == "Done"
                             group new { od, o, p } by new
                             {
                                 od.OrderdetailId,
                                 od.Order.OrderId,
                                 od.ProductId,
                                 p.ProductName,
                                 p.Image,
                                 od.QuantityOrder,
                                 o.OderDate
                             } into g
                             select new OrderWarranty
                             {
                                 Orderdetail_id = g.Key.OrderdetailId,
                                 Customer_id = id,
                                 Product_id = g.Key.ProductId.Value,
                                 Product_name = g.Key.ProductName,
                                 Image = g.Key.Image,
                                 Quantity_order = g.Key.QuantityOrder.Value,
                                 Order_date = g.Key.OderDate.Value
                             })
                        where !_context.Warranties.Any(w => w.OrderdetailId == a.Orderdetail_id)
                        select a)
                        .ToList();
            return list;
        }

    }
}
