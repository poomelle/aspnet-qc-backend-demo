namespace ChemsonLab.API.Models.Domain
{
    public class CustomerOrder
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public bool Status { get; set; }

        //Navigation properties
        public Customer Customer { get; set; }
        public Product Product { get; set; }
    }
}
