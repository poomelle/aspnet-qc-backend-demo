namespace ChemsonLab.API.Models.Domain
{
    public class Report
    {
        public int Id { get; set; }
        public string? CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Status { get; set; }


    }
}
