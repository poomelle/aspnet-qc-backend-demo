namespace ChemsonLab.API.Models.Domain
{
    public class QcLabel
    {
        public int Id { get; set; }
        public string? BatchName { get; set; }
        public bool? Printed { get; set; }
        public int? ProductId { get; set; }
        public string? Year { get; set; }
        public string? Month { get; set; }

        // Navigation properties
        public Product Product { get; set; }
    }
}
