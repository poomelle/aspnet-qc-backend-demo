using System.ComponentModel.DataAnnotations;

namespace ChemsonLab.API.Models.DTO.CustomerOrder
{
    public class UpdateCustomerOrderRequestDTO
    {
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public bool Status { get; set; }
    }
}
