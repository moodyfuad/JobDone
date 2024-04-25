namespace JobDone.ViewModels
{
    public class CustomerOrdersViewModel
    {
        public int orderId { get; set; }
        public string SellerName { get; set; }
        
        public string Username { get; set; }

        public int Rate { get; set; }
        
        public string SellerPicture { get; set; }

        public string OrderDescription { get; set; }

        public string ProjectName { get; set; }
        
        public decimal Price { get; set; }

        public DateOnly OrderDate { get; set; }

        public DateOnly DeliverDate{ get; set; }

        public string CategotyName { get; set; }

        public string OrderStatus { get; set; }

        public short SellerId { get; set; }

        public short CustomerId { get; set; }

        /*public decimal TotalOrdersPrice { get; set; }

        public decimal TotalTaxes { get; set; }

        public decimal TotalPayment { get; set; }*/

    }
}
