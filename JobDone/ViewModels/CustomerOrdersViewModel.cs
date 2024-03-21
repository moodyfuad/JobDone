namespace JobDone.ViewModels
{
    public class CustomerOrdersViewModel
    {
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

        /*public decimal TotalOrdersPrice { get; set; }

        public decimal TotalTaxes { get; set; }

        public decimal TotalPayment { get; set; }*/

    }
}
