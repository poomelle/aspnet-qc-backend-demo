using ChemsonLab.API.Models.DTO.Customer;
using ChemsonLab.API.Models.DTO.Product;

namespace ChemsonLab.API.Models.DTO.CustomerOrder
{
    public class CustomerOrderDTO
    {
        public int Id { get; set; }
        public CustomerDTO Customer { get; set; }
        public ProductDTO Product { get; set; }
        //public int CustomerId { get; set; }
        //public int ProductId { get; set; }
        public bool Status { get; set; }
    }
}
