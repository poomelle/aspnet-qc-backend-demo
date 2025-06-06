﻿namespace ChemsonLab.API.Models.DTO.QcPerformanceKpi
{
    public class AddQcPerformanceKpiRequestDTO
    {
        public int? ProductId { get; set; }
        public int? MachineId { get; set; }
        public string? Year { get; set; }
        public string? Month { get; set; }
        public int? TotalTest { get; set; }
        public int? FirstPass { get; set; }
        public int? SecondPass { get; set; }
        public int? ThirdPass { get; set; }
    }
}
