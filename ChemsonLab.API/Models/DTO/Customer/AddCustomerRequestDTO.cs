﻿using System.ComponentModel.DataAnnotations;

namespace ChemsonLab.API.Models.DTO.Customer
{
    public class AddCustomerRequestDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public bool Status { get; set; }
    }
}
