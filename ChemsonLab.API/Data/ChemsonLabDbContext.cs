using ChemsonLab.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChemsonLab.API.Data
{
    public class ChemsonLabDbContext : DbContext
    {
        public ChemsonLabDbContext(DbContextOptions dbContextOptions):base(dbContextOptions)
        {
            
        }

        // Define the tables
        public DbSet<Product> Product { get; set; }
        public DbSet<Machine> Machine { get; set; }
        public DbSet<ProductSpecification> ProductSpecification { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<CustomerOrder> CustomerOrder { get; set; }
        public DbSet<TestResult> TestResult { get; set; }
        public DbSet<Measurement> Measurement { get; set; }
        public DbSet<Evaluation> Evaluation { get; set; }
        public DbSet<Batch> Batch { get; set; }
        public DbSet<BatchTestResult> BatchTestResult { get; set; }
        public DbSet<Report> Report { get; set; }
        public DbSet<TestResultReport> TestResultReport { get; set; }
        public DbSet<QcLabel> QcLabel { get; set; }
        public DbSet<QcPerformanceKpi> QcPerformanceKpi { get; set; }
        public DbSet<QcAveTestTimeKpi> QcAveTestTimeKpi { get; set; }
        public DbSet<DailyQc> DailyQc { get; set; }
        public DbSet<Coa> Coa { get; set; }

        // Define view table


    }
}
