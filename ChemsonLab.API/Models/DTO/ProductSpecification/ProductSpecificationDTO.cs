using ChemsonLab.API.Models.DTO.Machine;
using ChemsonLab.API.Models.DTO.Product;

namespace ChemsonLab.API.Models.DTO.ProductSpecification
{
    public class ProductSpecificationDTO
    {
        public int Id { get; set; }
        public ProductDTO Product { get; set; }
        public MachineDTO Machine { get; set; }
        //public int ProductId { get; set; }
        //public int MachineId { get; set; }
        public bool? InUse { get; set; }
        public int? Temp { get; set; }
        public int? Load { get; set; }
        public int? RPM { get; set; }
    }
}
