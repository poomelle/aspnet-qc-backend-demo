namespace ChemsonLab.API.Models.DTO.Coa
{
    public class CoaDTO
    {
        public int Id { get; set; }
        public Domain.Product Product { get; set; }
        public string BatchName { get; set; }
    }
}
