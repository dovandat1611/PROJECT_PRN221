using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PROJECT_PRN221.Pages.adminsite.dashboard
{
    public class ChartsModel : PageModel
    {
        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;
        public ChartsModel(PROJECT_PRN221.Models.ProjectPrn221Context context)
        {
            _context = context;
        }

        public int currentDay { get; set; }
        public int currentMonth { get; set; } 
        public int currentYear { get; set; }

        public int warrantyWait { get; set; }
        public int warrantyProcess { get; set; }
        public int warrantyDone { get; set; }

        public int orderWait { get; set; }
        public int orderProcess { get; set; }
        public int orderDone { get; set; }
        public int orderCancel { get; set; }

        public decimal ti1 { get; set; }
        public decimal ti2 { get; set; }
        public decimal ti3 { get; set; }
        public decimal ti4 { get; set; }
        public decimal ti5 { get; set; }
        public decimal ti6 { get; set; }
        public decimal ti7 { get; set; }


        public int o1 { get; set; }
        public int o2 { get; set; }
        public int o3 { get; set; }
        public int o4 { get; set; }
        public int o5 { get; set; }
        public int o6 { get; set; }
        public int o7 { get; set; }


        public bool checkSession()
        {
            bool checkS = true;
            var httpContext = HttpContext;
            if (httpContext != null && httpContext.Session != null)
            {
                string isCustomerAuthenticated = httpContext.Session.GetString("admin");
                if (string.IsNullOrEmpty(isCustomerAuthenticated))
                {
                    checkS = false;
                }
            }
            return checkS;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (checkSession() == false)
            {
                return RedirectToPage("/adminsite/authenticate/login/Index");
            }
            DateTime currentDate = DateTime.Now;
            currentDay = currentDate.Day;
            currentMonth = currentDate.Month;
            currentYear = currentDate.Year;

            warrantyWait = getChartStatusWarranty("Wait");
            warrantyProcess = getChartStatusWarranty("Process");
            warrantyDone = getChartStatusWarranty("Done");

            orderWait = getChartStatusOrder("Wait");
            orderProcess = getChartStatusOrder("Process");
            orderDone = getChartStatusOrder("Done");
            orderCancel = getChartStatusOrder("Cancel");

            ti1 = getChartTotalIncomeWeek(currentDay, currentMonth, currentYear);
            ti2 = getChartTotalIncomeWeek(currentDay - 1, currentMonth, currentYear);
            ti3 = getChartTotalIncomeWeek(currentDay - 2, currentMonth, currentYear);
            ti4 = getChartTotalIncomeWeek(currentDay - 3, currentMonth, currentYear);
            ti5 = getChartTotalIncomeWeek(currentDay - 4, currentMonth, currentYear);
            ti6 = getChartTotalIncomeWeek(currentDay - 5, currentMonth, currentYear);
            ti7 = getChartTotalIncomeWeek(currentDay - 6, currentMonth, currentYear);

            o1 = GetChartTotalOrderWeek(currentDay, currentMonth, currentYear);
            o2 = GetChartTotalOrderWeek(currentDay - 1, currentMonth, currentYear);
            o3 = GetChartTotalOrderWeek(currentDay - 2, currentMonth, currentYear);
            o4 = GetChartTotalOrderWeek(currentDay - 3, currentMonth, currentYear);
            o5 = GetChartTotalOrderWeek(currentDay - 4, currentMonth, currentYear);
            o6 = GetChartTotalOrderWeek(currentDay - 5, currentMonth, currentYear);
            o7 = GetChartTotalOrderWeek(currentDay - 6, currentMonth, currentYear);

            return Page();
        }

        public int getChartStatusWarranty(string status)
        {   
            int total = _context.Warranties.Count(x => x.Status == status);
            if(total != null)
            {
                return total;
            }
            return 0;
        }

        public int getChartStatusOrder(string status)
        {
            int total = _context.Orders.Count(x => x.Status == status);
            if (total != null)
            {
                return total;
            }
            return 0;
        }

        public decimal getChartTotalIncomeWeek(int day, int month, int year)
        {
            decimal total = (decimal)_context.Orders.Where(x => x.OderDate.Value.Day == day 
            && x.OderDate.Value.Month == month && x.OderDate.Value.Year == year).Sum(x => x.TotalPrice);
            if (total != null)
            {
                return total;
            }
            return 0;
        }

        public int GetChartTotalOrderWeek(int day, int month, int year)
        {
            int total = 0;
            total = _context.Orders
                .Where(x => x.OderDate.HasValue &&
                            x.OderDate.Value.Day == day &&
                            x.OderDate.Value.Month == month &&
                            x.OderDate.Value.Year == year)
                .Count();

            return total;
        }
    }
}
