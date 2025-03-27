using System.ComponentModel.DataAnnotations;

namespace ChemsonLab.API.Models.DTO.Product
{
    public class AddProductRequestDTO
    {
        [Required]
        public string Name { get; set; }
        public bool Status { get; set; }
        public double? SampleAmount { get; set; }
        public DateTime? DBDate { get; set; }
        public string? Comment { get; set; }
        public bool? COA { get; set; }
        public double? TorqueWarning { get; set; }
        public double? TorqueFail { get; set; }
        public double? FusionWarning { get; set; }
        public double? FusionFail { get; set; }
        public DateTime? UpdateDate { get; set; }
        public double? BulkWeight { get; set; }
        public double? PaperBagWeight { get; set; }
        public int? PaperBagNo { get; set; }
        public double? BatchWeight { get; set; }
    }
}
