# Jasper Green Operations
Full-stack landscaping operations management application built with ASP.NET Core MVC and SQL Server.

I independently designed and developed Jasper Green Operations as an ISTM 415 semester project. The application combines a public-facing company website with an authenticated operations portal for managing customers, properties, employees, crews, services, payments, invoices, and system users.

## Application Overview
Jasper Green is a fictional landscaping company that needs an information system to manage its expanding operations. The application models the relationships between customers, their properties, landscaping crews, completed services, and customer payments.

The system includes two primary components:
- A public-facing company website presenting Jasper Green’s services and contact information
- An authenticated administrative portal supporting the company’s operational workflows

## Key Features
### Public Website
- Custom Jasper Green branding and visual design
- Home, About, and Contact pages
- Residential and commercial service information
- Service cards, property imagery, customer testimonials, and consultation calls to action
- Separate public navigation and layout

### Administrative Portal
- ASP.NET Core Identity authentication
- Administrator role protection
- Operational dashboard
- Customer and property management
- Employee and crew management
- Service scheduling and tracking
- Payment and customer-balance management
- System-user administration
- Password-management functionality

### Dashboard and Reporting
- Customer, property, employee, and crew totals
- Revenue calculated from recorded payments
- Current and previous 30-day revenue comparison
- Weekly service activity
- Recent service history
- Crew activity data
- PDF invoice generation through an external API

### Data Management
- Create, view, edit, and delete workflows
- Model and form validation
- Customer sorting by name and city
- Service filtering by customer, property, and crew
- Payment filtering
- State-selection dropdowns
- Referential business rules that prevent deletion of records still used by other entities
- Status messages confirming completed operations

## Technology Stack
- C#
- ASP.NET Core MVC
- .NET 8
- Entity Framework Core
- SQL Server LocalDB
- ASP.NET Core Identity
- Razor
- HTML
- CSS
- JavaScript
- Bootstrap

## Architecture
The application follows the ASP.NET Core Model-View-Controller pattern:
- Models: Business entities, validation rules, database relationships, and view models
- Views: Razor pages for public content and administrative workflows
- Controllers: Request handling, business logic, filtering, validation, and database operations
- Database: Entity Framework Core Code First development with SQL Server migrations and demonstration data
- Authentication: ASP.NET Core Identity with role-protected administrative controllers

## Database Model
The relational database connects the following entities:
- Customers can own one or more properties
- Properties belong to customers and store contracted service fees
- Employees are assigned to landscaping crews
- Crews contain a foreman and crew members
- Services connect customers, properties, crews, dates, and service fees
- Payments are recorded against completed services
- Users provide authenticated access to the operations portal

## Project Background
The original project specifications required a functional ASP.NET Core MVC application using C#, Entity Framework Core, SQL Server, Razor, Bootstrap, CRUD operations, relational data, filtering, and validation.

I independently implemented the required application and expanded it with:
- Authentication and role-based administration
- System-user and password management
- A custom administrative dashboard
- Revenue calculations and operational metrics
- Recent-activity and crew-performance data
- PDF invoice generation
- Additional sorting and filtering
- Expanded relational business rules
- A redesigned public-facing company website
- Additional demonstration data and interface improvements

## Local Configuration
Private development settings are stored in appsettings.Development.json, which is excluded from version control.

A local development configuration can include:
'''
{ "PdfMyHtml": { "ApiKey": "YOUR_PRIVATE_API_KEY" }, "AdminUser": { "Username": "YOUR_LOCAL_ADMIN_USERNAME", "Password": "YOUR_LOCAL_ADMIN_PASSWORD" } }
'''

No private API keys or administrator passwords are included in this repository.

## Running the Application
1. Install Visual Studio with the ASP.NET and web development workload.
2. Install SQL Server LocalDB.
3. Clone or download this repository.
4. Open JasperGreenProjectPhaseIII.sln.
5. Add the required private values to appsettings.Development.json.
6. Apply the included Entity Framework Core migrations.
7. Build and run the application through Visual Studio.

The PDF invoice integration requires a valid PdfMyHtml API key. The remaining application features can run without that integration.

## Skills Demonstrated
- Full-stack application development
- C# and ASP.NET Core MVC
- Relational database design
- SQL Server and Entity Framework Core
- Authentication and role-based access
- Business-process modeling
- Data validation and business rules
- CRUD application development
- Dashboard and reporting logic
- User-interface design
- External API integration

## Author
Cole Howell