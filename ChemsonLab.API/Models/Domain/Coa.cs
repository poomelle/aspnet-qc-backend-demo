namespace ChemsonLab.API.Models.Domain
{
    public class Coa
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string BatchName { get; set; }

        // Navigation properties
        public Product Product { get; set; }
    }
}
