using System.ComponentModel.DataAnnotations;

namespace ChemsonLab.API.Models.DTO.Machine
{
    public class AddMachineReuqestDTO
    {
        [Required]
        public string Name { get; set; }
        public bool Status { get; set; }
    }
}
