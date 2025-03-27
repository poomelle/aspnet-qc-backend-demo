namespace ChemsonLab.API.Models.DTO.QcLabel
{
    public class AddQcLabelRequestDTO
    {
        public string? BatchName { get; set; }
        public int? ProductId { get; set; }
        public bool? Printed { get; set; }
    }
}
