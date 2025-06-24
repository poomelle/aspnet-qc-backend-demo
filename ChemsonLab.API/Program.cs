using ChemsonLab.API.Data;
using ChemsonLab.API.Mappings;
using ChemsonLab.API.Repositories.BatchRepository;
using ChemsonLab.API.Repositories.BatchTestResultRepository;
using ChemsonLab.API.Repositories.CoaRepository;
using ChemsonLab.API.Repositories.CustomerOrderRepository;
using ChemsonLab.API.Repositories.CustomerRepository;
using ChemsonLab.API.Repositories.DailyQcRepository;
using ChemsonLab.API.Repositories.EvaluationRepository;
using ChemsonLab.API.Repositories.MachineRepository;
using ChemsonLab.API.Repositories.MeasurementRepository;
using ChemsonLab.API.Repositories.ProductRepository;
using ChemsonLab.API.Repositories.ProductSpecificationRepository;
using ChemsonLab.API.Repositories.QcAveTestTimeKpiRepository;
using ChemsonLab.API.Repositories.QcLabelRepository;
using ChemsonLab.API.Repositories.QcPerformanceKpiRepository;
using ChemsonLab.API.Repositories.ReportRepository;
using ChemsonLab.API.Repositories.TestResultReportRepository;
using ChemsonLab.API.Repositories.TestResultRepository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("LocalhostConnectionString");
builder.Services.AddDbContext<ChemsonLabDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IProductRepository, MySQLProductRepository>();
builder.Services.AddScoped<IMachineRepository, MySQLMachineRepository>();
builder.Services.AddScoped<IProductSpecificationRepository, MySQLProductSpecificationRepository>();
builder.Services.AddScoped<IReportRepository, MySQLReportRepository>();
builder.Services.AddScoped<ICustomerRepository, MySQLCustomerRepository>();
builder.Services.AddScoped<IBatchRepository, MySQLBatchRepository>();
builder.Services.AddScoped<IMeasurementRepository, MySQLMeasurementRepository>();
builder.Services.AddScoped<IEvaluationRepository, MySQLEvaluationRepository>();
builder.Services.AddScoped<ITestResultRepository, MySQLTestResultRepository>();
builder.Services.AddScoped<IBatchTestResultRepository, MySQLBatchTestResultRepository>();
builder.Services.AddScoped<ICustomerOrderRepository, MySQLCustomerOrderRepository>();
builder.Services.AddScoped<ITestResultReportRepository, MySQLTestResultReportRepository>();
builder.Services.AddScoped<IQcLabelRepository, MySQLQcLabelRepository>();
builder.Services.AddScoped<IQcPerformanceKpiRepository, MySQLQcPerformanceKpiRepository>();
builder.Services.AddScoped<IQcAveTestTimeKpiRepository, MySQLQcAveTestTimeKpiRepository>();
builder.Services.AddScoped<IDailyQcRepository, MySQLDailyQcRepository>();
builder.Services.AddScoped<ICoaRepository, MySQLCoaRepository>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
