using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PROJECT_PRN221.Dto;
using PROJECT_PRN221.Models;
using RazorPagesLab.utils;
using System.Collections.Generic;
using System.Configuration;
using System.Numerics;

namespace PROJECT_PRN221.Pages.adminsite.dashboard
{
    public class IndexModel : PageModel
    {
        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;
        public IndexModel(PROJECT_PRN221.Models.ProjectPrn221Context context)
        {
            _context = context;
        }

        public int TotalOrder { get; set; }
        public int TotalProductSold { get; set; }
        public double TotalIncome { get; set; }
        public int TotalProduct { get; set; }
        public int TotalCustomer { get; set; }
        public double TotalIncomeYear { get; set; }
        public int Year { get; set; }

        public int Wait { get; set; }
        public int Process { get; set; }
        public int Done { get; set; }
        public int Cancel { get; set; }

        public List<LoyalCustomer> LoyalCustomers { get; set; }

        public List<TopCategories> TopCategories { get; set; }

        public decimal t1 { get; set; }
        public decimal t2 { get; set; }
        public decimal t3 { get; set; }
        public decimal t4 { get; set; }
        public decimal t5 { get; set; }
        public decimal t6 { get; set; }
        public decimal t7 { get; set; }
        public decimal t8 { get; set; }
        public decimal t9 { get; set; }
        public decimal t10 { get; set; }
        public decimal t11 { get; set; }
        public decimal t12 { get; set; }


        public async Task OnGetAsync(string service, int year)
        {
            // HEADER
            TotalOrder = _context.Orders.Count();
            TotalCustomer = _context.Customers.Count();
            TotalProduct = _context.Products.Count();
            TotalProductSold = (int)_context.OrderDetails.Sum(x => x.QuantityOrder);
            TotalIncome = (double)_context.Orders.Sum(x => x.TotalPrice);

            // ORDER STATUS
            Wait = _context.Orders.Where(x => x.Status.Equals("Wait")).Count();
            Process = _context.Orders.Where(x => x.Status.Equals("Process")).Count();
            Done = _context.Orders.Where(x => x.Status.Equals("Done")).Count();
            Cancel = _context.Orders.Where(x => x.Status.Equals("Cancel")).Count();

            // Top Loyal Customers
            LoyalCustomers = _context.Orders
                .Join(_context.Customers, o => o.CustomerId, c => c.CustomerId, (o, c) => new { Order = o, Customer = c })
                .GroupBy(x => new { x.Customer.CustomerId, x.Customer.Name, x.Customer.Image })
                .Select(group => new LoyalCustomer
                {
                    CustomerId = group.Key.CustomerId,
                    TotalPrice = (double)group.Sum(x => x.Order.TotalPrice),
                    Name = group.Key.Name,
                    Image = group.Key.Image
                })
                .OrderByDescending(x => x.TotalPrice)
                .Take(5)
                .ToList();

            // SEARCH TOTAL INCOME YEAR
            if (service == null)
            {
                service = "TotalInComeCurrentYear";
            }
            if (service.Equals("TotalInComeCurrentYear"))
            {
                Year = DateTime.Now.Year;
                TotalIncomeYear = (double)_context.Orders
                .Where(x => x.OderDate.Value.Year == DateTime.Now.Year)
                .Sum(x => x.TotalPrice);
            }
            if (service.Equals("searchTotalIncomeYear"))
            {
                Year = year;
                TotalIncomeYear = (double)_context.Orders
                .Where(x => x.OderDate.Value.Year == year)
                .Sum(x => x.TotalPrice);
            }

            // CHART TOTAL INCOME YEAR
            t1 = getChartTotalMoneyForYear(1, Year);
            t2 = getChartTotalMoneyForYear(2, Year);
            t3 = getChartTotalMoneyForYear(3, Year);
            t4 = getChartTotalMoneyForYear(4, Year);
            t5 = getChartTotalMoneyForYear(5, Year);
            t6 = getChartTotalMoneyForYear(6, Year);
            t7 = getChartTotalMoneyForYear(7, Year);
            t8 = getChartTotalMoneyForYear(8, Year);
            t9 = getChartTotalMoneyForYear(9, Year);
            t10 = getChartTotalMoneyForYear(10, Year);
            t11 = getChartTotalMoneyForYear(11, Year);
            t12 = getChartTotalMoneyForYear(12, Year);


            // CHART CATEGORY

            TopCategories = (from od in _context.OrderDetails
                             join p in _context.Products on od.ProductId equals p.ProductId
                             join c in _context.Categories on p.CategoryId equals c.CategoryId
                             group new { c.CategoryId, c.CategoryName } by new { c.CategoryId, c.CategoryName } into g
                             orderby g.Count() descending
                             select new TopCategories
                             {
                                 CategoryID = g.Key.CategoryId,
                                 CategoryName = g.Key.CategoryName,
                                 TotalSold = g.Count()
                             }).ToList();

        }


        public decimal getChartTotalMoneyForYear(int mounth, int year)
        {
            decimal totalMoney = (decimal)_context.Orders
            .Where(o => o.OderDate.Value.Month == mounth && o.OderDate.Value.Year == year)
            .Sum(o => o.TotalPrice);

            if (totalMoney == null)
            {
                return 0;
            }
            else
            {
                return totalMoney;
            }
        }

    }
}
