using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VNPAY_CS_ASPX;

namespace PROJECT_PRN221.Pages.customersite.checkout
{
    public class ReturnModel : PageModel
    {   
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
    }
}
