# ChemsonLab API
A comprehensive laboratory management system API for chemical testing and quality control operations. This ASP.NET Core Web API provides endpoints for managing test results, batch operations, product specifications, and quality control processes.

## 📋 Overview
ChemsonLab API is designed to support laboratory operations including:
- Test result management and analysis
- Batch tracking and quality control
- Product specification management
- Machine and equipment monitoring
- Report generation and COA (Certificate of Analysis)
- Customer order management
- Daily QC operations and KPI tracking

## 🏗️ Architecture
The project follows a clean architecture pattern with:
- **Controllers**: Handle HTTP requests and responses
- **Models**: Domain entities and DTOs for data transfer
- **Repositories**: Data access layer with interface-based design
- **Services**: Business logic implementation
- **Database**: MySQL database for data persistence

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- MySQL Server
- Visual Studio 2022 or VS Code

### Installation
1. **Clone the repository**
2. **Update Connection String**
   Edit `appsettings.json` and update the connection string
3. **Restore Dependencies**
4. **Run Database Migrations**
5. **Run the Application**


The API will be available at `https://localhost:7000` and `http://localhost:5000`.

## 📊 Core Features

### Test Management
- **Test Results**: Comprehensive test result tracking with detailed parameters
- **Measurements**: Time-based measurement data with torque, temperature, and speed monitoring
- **Test Methods**: Configurable test procedures and protocols

### Quality Control
- **Batch Management**: Track production batches with quality metrics
- **Daily QC**: Daily quality control operations and monitoring
- **KPI Tracking**: Performance indicators for quality metrics
- **Evaluations**: Quality evaluation and assessment tools

### Product Management
- **Product Specifications**: Define and manage product quality specifications
- **COA Generation**: Certificate of Analysis creation and management
- **Customer Orders**: Link products to customer requirements

### Reporting
- **Test Result Reports**: Detailed analysis and comparison reports
- **QC Performance**: Quality control performance metrics
- **Custom Reports**: Flexible reporting system for various needs

## 🔧 API Endpoints

### Test Results
- `GET /api/testresults` - Get all test results
- `GET /api/testresults/{id}` - Get specific test result
- `POST /api/testresults` - Create new test result
- `PUT /api/testresults/{id}` - Update test result
- `DELETE /api/testresults/{id}` - Delete test result

### Batches
- `GET /api/batch` - Get all batches
- `GET /api/batch/{id}` - Get specific batch
- `POST /api/batch` - Create new batch
- `PUT /api/batch/{id}` - Update batch
- `DELETE /api/batch/{id}` - Delete batch

### Products
- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get specific product
- `POST /api/products` - Create new product
- `PUT /api/products/{id}` - Update product
- `DELETE /api/products/{id}` - Delete product

### Machines
- `GET /api/machines` - Get all machines
- `GET /api/machines/{id}` - Get specific machine
- `POST /api/machines` - Create new machine
- `PUT /api/machines/{id}` - Update machine
- `DELETE /api/machines/{id}` - Delete machine

*[Additional endpoints for other entities...]*

## 🧪 Testing

The project includes comprehensive unit tests for both controllers and repositories:

### Running Tests

### Test Coverage
- **Controller Tests**: API endpoint testing with mocked dependencies
- **Repository Tests**: Data access layer testing with mock repositories
- **Integration Tests**: End-to-end testing scenarios

### Test Structure
- `ChemsonLab.API.Tests/ControllerTests/` - Controller unit tests
- `ChemsonLab.API.Tests/RepositoryTests/` - Repository unit tests

## 💾 Database Schema

Key entities include:
- **TestResult**: Core test data with measurements and parameters
- **Product**: Product definitions and specifications
- **Machine**: Testing equipment and machinery
- **Batch**: Production batch tracking
- **Customer**: Customer information and orders
- **Reports**: Generated reports and analysis

## 🔍 Key Models

### TestResult
Comprehensive test result entity with properties including:
- Test parameters (Speed, Temperature, Time)
- Sample information (Weight, Type, Batch)
- Quality metrics (Torque, Fusion values)
- Operator and machine information

### Batch
Production batch tracking with:
- Batch identification and timing
- Quality control status
- Associated test results
- Customer order linkage

## 🛠️ Technologies Used

- **Framework**: ASP.NET Core 8
- **Database**: MySQL with Entity Framework Core
- **Testing**: xUnit, Moq
- **Mapping**: AutoMapper
- **Logging**: Microsoft.Extensions.Logging
- **Documentation**: Swagger/OpenAPI

## 📝 Development Guidelines

### Code Standards
- Follow C# naming conventions
- Use async/await for database operations
- Implement proper error handling and logging
- Include XML documentation for public APIs

### Testing Requirements
- Maintain comprehensive unit test coverage
- Mock external dependencies
- Test both success and error scenarios
- Include edge case testing

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Implement changes with tests
4. Ensure all tests pass
5. Submit a pull request

## 📄 License

This project is proprietary software for laboratory management operations.

## 📞 Support

For technical support or questions about the API, please contact the development team.

---

**Version**: 1.0.0  
**Last Updated**: June 2025

   
