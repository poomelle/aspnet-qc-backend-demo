using AutoMapper;
using ChemsonLab.API.Models.Domain;
using ChemsonLab.API.Models.DTO.Batch;
using ChemsonLab.API.Models.DTO.BatchTestResult;
using ChemsonLab.API.Models.DTO.Coa;
using ChemsonLab.API.Models.DTO.Customer;
using ChemsonLab.API.Models.DTO.CustomerOrder;
using ChemsonLab.API.Models.DTO.DailyQc;
using ChemsonLab.API.Models.DTO.Evaluation;
using ChemsonLab.API.Models.DTO.Machine;
using ChemsonLab.API.Models.DTO.Measurement;
using ChemsonLab.API.Models.DTO.Product;
using ChemsonLab.API.Models.DTO.ProductSpecification;
using ChemsonLab.API.Models.DTO.QcAveTestTimeKpi;
using ChemsonLab.API.Models.DTO.QcLabel;
using ChemsonLab.API.Models.DTO.QcPerformanceKpi;
using ChemsonLab.API.Models.DTO.Report;
using ChemsonLab.API.Models.DTO.TestResult;
using ChemsonLab.API.Models.DTO.TestResultReport;
using System.Globalization;

namespace ChemsonLab.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Product's autoMapper
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, AddProductRequestDTO>().ReverseMap();
            CreateMap<Product, UpdateProductRequestDTO>().ReverseMap();

            // Machine's autoMapper
            CreateMap<Machine, MachineDTO>().ReverseMap();
            CreateMap<Machine, AddMachineReuqestDTO>().ReverseMap();
            CreateMap<Machine, UpdateMachineRequestDTO>().ReverseMap();

            // Product Specification's autoMapper
            CreateMap<ProductSpecification, ProductSpecificationDTO>().ReverseMap();
            CreateMap<ProductSpecification, AddProductSpecificationRequestDTO>().ReverseMap();
            CreateMap<ProductSpecification, UpdateProductSpecificationRequestDTO>().ReverseMap();

            // Report's autoMapper
            CreateMap<Report, ReportDTO>().ReverseMap();
            CreateMap<Report, AddReportRequestDTO>().ReverseMap();
            CreateMap<Report, UpdateReportRequestDTO>().ReverseMap();

            // Customer's autoMapper
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<Customer, AddCustomerRequestDTO>().ReverseMap();
            CreateMap<Customer, UpdateCustomerRequestDTO>().ReverseMap();

            // Batch's autoMapper
            CreateMap<Batch, BatchDTO>().ReverseMap();
            CreateMap<Batch, AddBatchRequestDTO>().ReverseMap();
            CreateMap<Batch, UpdateBatchRequestDTO>().ReverseMap();

            // Measurement's autoMapper
            CreateMap<Measurement, MeasurementDTO>()
                .ForMember(destination => destination.TimeAct, options => options.MapFrom(source => source.TimeAct.ToString(@"hh\:mm\:ss")))
                .ReverseMap()
                .ForMember(destination => destination.TimeAct, options => options.MapFrom(source => TimeSpan.Parse(source.TimeAct)));

            CreateMap<Measurement, AddMeasurementRequestDTO>()
                .ForMember(destination => destination.TimeAct, options => options.MapFrom(source => source.TimeAct.ToString(@"hh\:mm\:ss")))
                .ReverseMap()
                .ForMember(destination => destination.TimeAct, options => options.MapFrom(source => TimeSpan.Parse(source.TimeAct)));

            CreateMap<Measurement, UpdateMeasurementRequestDTO>()
                .ForMember(destination => destination.TimeAct, options => options.MapFrom(source => source.TimeAct.ToString(@"hh\:mm\:ss")))
                .ReverseMap()
                .ForMember(destination => destination.TimeAct, options => options.MapFrom(source => TimeSpan.Parse(source.TimeAct)));


            // Evaluation's autoMapper
            CreateMap<Evaluation, EvaluationDTO>()
                .ForMember(destination => destination.TimeEval, options => options.MapFrom(source => source.TimeEval.ToString(@"hh\:mm\:ss")))
                .ForMember(destination => destination.TimeRange, options => options.MapFrom(source => source.TimeRange.ToString(@"hh\:mm\:ss")))
                .ReverseMap()
                .ForMember(destination => destination.TimeEval, options => options.MapFrom(source => TimeSpan.Parse(source.TimeEval)))
                .ForMember(destination => destination.TimeRange, options => options.MapFrom(source => TimeSpan.Parse(source.TimeRange)));
            CreateMap<Evaluation, AddEvaluationRequestDTO>()
                .ForMember(destination => destination.TimeEval, options => options.MapFrom(source => source.TimeEval.ToString(@"hh\:mm\:ss")))
                .ForMember(destination => destination.TimeRange, options => options.MapFrom(source => source.TimeRange.ToString(@"hh\:mm\:ss")))
                .ReverseMap()
                .ForMember(destination => destination.TimeEval, options => options.MapFrom(source => TimeSpan.Parse(source.TimeEval)))
                .ForMember(destination => destination.TimeRange, options => options.MapFrom(source => TimeSpan.Parse(source.TimeRange)));
            CreateMap<Evaluation, UpdateEvaluationRequestDTO>()
                .ForMember(destination => destination.TimeEval, options => options.MapFrom(source => source.TimeEval.ToString(@"hh\:mm\:ss")))
                .ForMember(destination => destination.TimeRange, options => options.MapFrom(source => source.TimeRange.ToString(@"hh\:mm\:ss")))
                .ReverseMap()
                .ForMember(destination => destination.TimeEval, options => options.MapFrom(source => TimeSpan.Parse(source.TimeEval)))
                .ForMember(destination => destination.TimeRange, options => options.MapFrom(source => TimeSpan.Parse(source.TimeRange)));

            // TestResult's autoMapper
            CreateMap<TestResult, TestResultDTO>()
                .ForMember(destination => destination.TestDate, options => options.MapFrom(source => source.TestDate.ToString("dd/MM/yyyy HH:mm")))
                .ReverseMap()
                .ForMember(destination => destination.TestDate, options => options.MapFrom(source => DateTime.ParseExact(source.TestDate, "d/M/yyyy H:mm", CultureInfo.InvariantCulture)));
            CreateMap<TestResult, AddTestResultRequestDTO>()
                .ForMember(destination => destination.TestDate, options => options.MapFrom(source => source.TestDate.ToString("dd/MM/yyyy HH:mm")))
                .ReverseMap()
                .ForMember(destination => destination.TestDate, options => options.MapFrom(source => DateTime.ParseExact(source.TestDate, "d/M/yyyy H:mm", CultureInfo.InvariantCulture)));
            CreateMap<TestResult, UpdateTestResultRequestDTO>()
                .ForMember(destination => destination.TestDate, options => options.MapFrom(source => source.TestDate.ToString("dd/MM/yyyy HH:mm")))
                .ReverseMap()
                .ForMember(destination => destination.TestDate, options => options.MapFrom(source => DateTime.ParseExact(source.TestDate, "d/M/yyyy H:mm", CultureInfo.InvariantCulture)));

            // BatchTestResult's autoMapper
            CreateMap<BatchTestResult, BatchTestResultDTO>().ReverseMap();
            CreateMap<BatchTestResult, AddBatchTestResultRequestDTO>().ReverseMap();
            CreateMap<BatchTestResult, UpdateBatchTestResultRequestDTO>().ReverseMap();

            // CustomerOrder's autoMapper
            CreateMap<CustomerOrder, CustomerOrderDTO>().ReverseMap();
            CreateMap<CustomerOrder, AddCustomerOrderRequestDTO>().ReverseMap();
            CreateMap<CustomerOrder, UpdateCustomerOrderRequestDTO>().ReverseMap();

            // TestResultReport's autoMapper
            CreateMap<TestResultReport, TestResultReportDTO>().ReverseMap();
            CreateMap<TestResultReport, AddTestResultReportRequestDTO>().ReverseMap();
            CreateMap<TestResultReport, UpdateTestResultReportRquestDTO>().ReverseMap();

            // QcLabel autoMapper
            CreateMap<QcLabel, QcLabelDTO>().ReverseMap();
            CreateMap<QcLabel, AddQcLabelRequestDTO>().ReverseMap();
            CreateMap<QcLabel, UpdateQcLabelRequestDTO>().ReverseMap();

            // QcPerformanceKpi autoMapper
            CreateMap<QcPerformanceKpi, QcPerformanceKpiDTO>().ReverseMap();
            CreateMap<QcPerformanceKpi, AddQcPerformanceKpiRequestDTO>().ReverseMap();
            CreateMap<QcPerformanceKpi, UpdateQcPerformanceKpiRequestDTO>().ReverseMap();

            // QcAveTestTimeKpi autoMapper
            CreateMap<QcAveTestTimeKpi, QcAveTestTimeKpiDTO>().ReverseMap();
            CreateMap<QcAveTestTimeKpi, AddQcAveTestTimeKpiRequestDTO>().ReverseMap();
            CreateMap<QcAveTestTimeKpi, UpdateQcAveTestTimeKpiRequestDTO>().ReverseMap();

            // DailyQc autoMapper
            CreateMap<DailyQc, DailyQcDTO>().ReverseMap();
            CreateMap<DailyQc, AddDailyQcRequestDTO>().ReverseMap();
            CreateMap<DailyQc, UpdateDailyQcRequestDTO>().ReverseMap();

            // Coa autoMapper
            CreateMap<Coa, CoaDTO>().ReverseMap();
            CreateMap<Coa, AddCoaRequestDTO>().ReverseMap();
            CreateMap<Coa, UpdateCoaRequestDTO>().ReverseMap();
        }
    }
}
