using System.ComponentModel.DataAnnotations;

namespace ChemsonLab.API.Models.DTO.Machine
{
    public class UpdateMachineRequestDTO
    {
        [Required]
        public string Name { get; set; }
        public bool Status { get; set; }
    }
}
