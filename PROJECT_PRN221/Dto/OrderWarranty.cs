namespace PROJECT_PRN221.Dto
{
    public class OrderWarranty
    {
        public int Orderdetail_id { get; set; }
        public int Customer_id { get; set; }
        public int Product_id { get; set; } 
        public string Product_name { get; set; }
        public string Image { get; set; }
        public int Quantity_order { get; set; }
        public DateTime Order_date { get; set; }
    }
}
