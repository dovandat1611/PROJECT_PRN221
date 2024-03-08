using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using PROJECT_PRN221.Dto;
using PROJECT_PRN221.Models;
using VNPAY_CS_ASPX;

namespace PROJECT_PRN221.Pages.customersite.checkout
{
    public class ReturnModel : PageModel
    {
        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;
        private readonly IHubContext<HubServer> _hubContext;
        public ReturnModel(PROJECT_PRN221.Models.ProjectPrn221Context context, IHubContext<HubServer> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        public string displayMsg { get; set; }
        public string paymentStatus { get; set; }
        public string displayTmnCode { get; set; }
        public string displayTxnRef { get; set; }
        public string displayVnpayTranNo { get; set; }
        public string displayAmount { get; set; }
        public string displayBankCode { get; set; }

        public void OnGet(string vnp_TxnRef, string vnp_TransactionNo, string vnp_ResponseCode, 
            string vnp_TransactionStatus, string vnp_SecureHash, 
            string vnp_TmnCode, string vnp_Amount, string vnp_BankCode)
        {
            string vnp_HashSecret = "FAZFUPBGAQNACZYMXPPTWPPMGBJGTRZD";

            long orderId = Convert.ToInt64(vnp_TxnRef);
            long vnpayTranId = Convert.ToInt64(vnp_TransactionNo);
            string ResponseCode = vnp_ResponseCode;
            string TransactionStatus = vnp_TransactionStatus;
            String SecureHash = vnp_SecureHash;
            String TerminalID = vnp_TmnCode;
            long Amount = Convert.ToInt64(vnp_Amount) / 100;
            String bankCode = vnp_BankCode;

            if (!string.IsNullOrEmpty(vnp_TxnRef) &&
                !string.IsNullOrEmpty(vnp_TransactionNo) &&
                !string.IsNullOrEmpty(vnp_ResponseCode) &&
                !string.IsNullOrEmpty(vnp_TransactionStatus) &&
                !string.IsNullOrEmpty(vnp_SecureHash) &&
                !string.IsNullOrEmpty(vnp_TmnCode) &&
                !string.IsNullOrEmpty(vnp_Amount) &&
                !string.IsNullOrEmpty(vnp_BankCode))
            {
                if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                {   
                    //Thanh toan thanh cong
                    displayMsg = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                    paymentStatus = "Success";

                    // Xử Lý Thanh Toán: 
                    string orderByVNPAY = HttpContext.Session.GetString("orderByVNPAY");
                    if (!string.IsNullOrEmpty(orderByVNPAY))
                    {
                        var order = JsonConvert.DeserializeObject<Order>(orderByVNPAY);
                        int orderDefault = 0;
                        if (_context != null)
                        {
                            _context.Orders.Add(order);
                            _context.SaveChanges();
                            orderDefault = order.OrderId;
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
                                    OrderId = orderDefault,
                                    ProductId = item.id,
                                    ListPrice = item.subtotal,
                                    QuantityOrder = item.quantity,
                                };

                                if (_context != null)
                                {
                                    _context.OrderDetails.Add(orderDetail);
                                    _context.SaveChanges();
                                    _hubContext.Clients.All.SendAsync("ReloadData");
                                }
                            }
                        }
                        HttpContext.Session.Remove("orderByVNPAY");
                        HttpContext.Session.Remove("cart");
                    }
                }
                else
                {
                    //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                    displayMsg = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                    paymentStatus = "Fail";
                }
                displayTmnCode = TerminalID;
                displayTxnRef =  orderId.ToString();
                displayVnpayTranNo = vnpayTranId.ToString();
                displayAmount =  vnp_Amount.ToString();
                displayBankCode = bankCode;
            }
            else
            {
                displayMsg = "Có lỗi xảy ra trong quá trình xử lý";
            }
        }

        public async Task<IActionResult> OnPostAsync(string paymentStatus, string service)
        {
            if (service.Equals("returnVNPAY"))
            {
                if (paymentStatus.Equals("Success"))
                {
                    return RedirectToPage("/customersite/shop/Index");
                }
                if (paymentStatus.Equals("Fail"))
                {
                    return RedirectToPage("/customersite/cart/Index");
                }
            }
            return RedirectToPage("/customersite/cart/Index");
        }
    }
}
