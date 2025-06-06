﻿using System.ComponentModel.DataAnnotations;

namespace ChemsonLab.API.Models.DTO.Report
{
    public class UpdateReportRequestDTO
    {
        [Required]
        public string CreateBy { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
        [Required]
        public bool Status { get; set; }
    }
}
