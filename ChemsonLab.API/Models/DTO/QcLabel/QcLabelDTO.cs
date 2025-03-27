using ChemsonLab.API.Models.DTO.Batch;
using ChemsonLab.API.Models.Domain;

namespace ChemsonLab.API.Models.DTO.QcLabel
{
    public class QcLabelDTO
    {
        public int Id { get; set; }
        public string? BatchName { get; set; }
        public bool? Printed { get; set; }
        public string? Year { get; set; }
        public string? Month { get; set; }

        // Navigation properties
        public Domain.Product Product { get; set; }
    }
}
