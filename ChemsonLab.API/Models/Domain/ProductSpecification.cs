namespace ChemsonLab.API.Models.Domain
{
    public class ProductSpecification
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int MachineId { get; set; }
        public bool? InUse { get; set; }
        public int? Temp { get; set; }
        public int? Load { get; set; }
        public int? RPM { get; set; }

        //Navigation properties
        public Product Product { get; set; }
        public Machine Machine { get; set; }
    }
}
