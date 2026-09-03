<a id="readme-top"></a>

<!-- PROJECT HEADER -->
<div align="center">

# Jasper Green Operations

### Full-stack business operations platform built with ASP.NET Core MVC, C#, SQL Server, and Entity Framework Core

[![C#][csharp-shield]][csharp-url]
[![.NET][dotnet-shield]][dotnet-url]
[![ASP.NET Core][aspnet-shield]][aspnet-url]
[![SQL Server][sqlserver-shield]][sqlserver-url]
[![Bootstrap][bootstrap-shield]][bootstrap-url]
[![LinkedIn][linkedin-shield]][linkedin-url]

<br />

Jasper Green Operations models the day-to-day workflows of a fictional landscaping company through a public-facing website and authenticated internal operations portal.

[View Repository](https://github.com/colebh03/jasper-green-operations)
·
[LinkedIn](https://www.linkedin.com/in/cole-howell/)

</div>

![Jasper Green Operations Dashboard](docs/images/operations-dashboard.png)


---

<!-- TABLE OF CONTENTS -->
<details>
  <summary><strong>Table of Contents</strong></summary>
  <ol>
    <li>
      <a href="#about-the-project">About the Project</a>
      <ul>
        <li><a href="#business-workflow">Business Workflow</a></li>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#core-features">Core Features</a>
      <ul>
        <li><a href="#operations-portal">Operations Portal</a></li>
        <li><a href="#business-rules-and-validation">Business Rules and Validation</a></li>
        <li><a href="#filtering-and-data-access">Filtering and Data Access</a></li>
        <li><a href="#dashboard">Dashboard</a></li>
        <li><a href="#invoice-generation">Invoice Generation</a></li>
        <li><a href="#public-website">Public Website</a></li>
      </ul>
    </li>
    <li><a href="#architecture">Architecture</a></li>
    <li><a href="#getting-started">Getting Started</a></li>
    <li><a href="#project-background">Project Background</a></li>
    <li><a href="#what-i-took-from-the-project">What I Took From the Project</a></li>
    <li><a href="#contact">Contact</a></li>
  </ol>
</details>

---

<!-- ABOUT THE PROJECT -->
## About the Project

Jasper Green Operations is a full-stack information system I designed and developed to model the operational needs of a fictional landscaping company.

The application combines two connected experiences:

- A public-facing company website for customers
- An authenticated internal portal for managing day-to-day business operations

The goal was not just to build a CRUD application. I wanted to take a connected business process, model the relationships behind it, and build an application that could support the workflow from customer and property management through completed service, payment, and invoice generation.

### Business Workflow

The system is organized around a connected set of business relationships:

**Customer → Property → Service → Crew → Payment → Invoice**

A customer can own multiple properties. Each property stores its own service information and standard rate. Employees are assigned to crews, crews complete service work, and completed services connect operational activity with customer payments and invoice generation.

The application uses those relationships to support workflows across the system rather than treating each database table as an isolated record.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

### Built With

#### Application

- C#
- .NET 8
- ASP.NET Core MVC
- Razor
- ASP.NET Core Identity

#### Data

- SQL Server LocalDB
- Entity Framework Core
- Code First migrations
- LINQ

#### Front End

- HTML
- CSS
- JavaScript
- Bootstrap
- Bootstrap Icons

#### Integration and Development

- PdfMyHtml API
- `HttpClient`
- JSON request and response handling
- Visual Studio
- Git
- GitHub

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

<!-- CORE FEATURES -->
## Core Features

### Operations Portal

The authenticated operations portal provides centralized access to the company's primary workflows:

- Customer and property management
- Employee and crew management
- Service scheduling and tracking
- Payment history and customer balances
- System-user administration
- Password management
- Operational dashboard and recent activity

### Business Rules and Validation

The application includes validation and relational controls designed around the underlying business process.

Examples include:

- A property must belong to a valid customer
- A crew must contain three different employees
- A service fee cannot be entered below the property's standard service rate
- Finalized services with recorded payments cannot be edited
- Records referenced by dependent data are protected from invalid deletion
- Property selections dynamically update based on the selected customer
- Forms provide validation and user feedback for completed or blocked operations

### Filtering and Data Access

Operational records can be filtered and sorted without loading the entire dataset into application memory.

Examples include:

- Service filtering by customer, property, or crew
- Payment filtering by customer and date
- Multi-column sorting across operational lists
- Entity Framework Core relationship loading for connected business data

### Dashboard

The administrative dashboard provides a quick view of current operations, including:

- Total customers
- Total properties
- Total crews
- Total employees
- Service activity from the previous seven days
- Revenue from the previous 30 days
- Revenue comparison against the preceding 30-day period
- Recent completed service activity

### Invoice Generation

Completed service information can be converted into a customer-facing invoice.

The application:

1. Retrieves the related customer, property, crew, service, and payment data
2. Renders the Razor invoice view into HTML
3. Sends the rendered HTML to the PdfMyHtml API
4. Polls the external conversion job until processing completes
5. Returns the generated PDF to the user

The API credential is stored outside version control through local development configuration.

### Public Website

The project also includes a responsive public website for the fictional Jasper Green brand, including:

- Home, About, and Contact pages
- Residential and commercial service information
- Responsive layouts and navigation
- Custom CSS and Bootstrap styling
- Scroll-based interface effects
- Separate public and administrative layouts within the MVC application

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

<!-- ARCHITECTURE -->
## Architecture

The application follows the ASP.NET Core Model-View-Controller pattern.

### Models

Models define the application's business entities, database relationships, validation rules, and view-specific data structures.

Primary entities include:

- Customer
- Property
- Employee
- Crew
- Service
- Payment
- User

### Controllers

Controllers coordinate application workflows, database access, validation, filtering, sorting, authentication, and external integrations.

Examples include:

- Building filtered `IQueryable` service queries before database execution
- Enforcing service-pricing and record-state rules
- Repopulating form data after validation failures
- Returning customer-specific property data for dynamic form behavior
- Preparing connected service data for invoice generation

### Views

Razor views provide the public website and authenticated operations interface. Shared layouts separate the customer-facing site from the internal administrative experience.

### Database

Entity Framework Core maps the relational business model to SQL Server and manages schema changes through migrations.

The data model reflects the operational relationships between customers, properties, employees, crews, services, and payments.

### Authentication

ASP.NET Core Identity provides authentication for the internal operations portal.

An initial administrator account is created from locally configured credentials when the application starts. Additional authenticated system users can then be managed through the operations portal.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

<!-- GETTING STARTED -->
## Getting Started

### Prerequisites

To run the application locally, install:

- Visual Studio
- ASP.NET and web development workload
- SQL Server LocalDB
- .NET 8 SDK

### Local Configuration

Private development values are stored in `appsettings.Development.json`, which is excluded from version control.

Example:

```json
{
  "PdfMyHtml": {
    "ApiKey": "YOUR_PRIVATE_API_KEY"
  },
  "AdminUser": {
    "Username": "YOUR_LOCAL_ADMIN_USERNAME",
    "Password": "YOUR_LOCAL_ADMIN_PASSWORD"
  }
}
```

No API keys or administrator passwords are included in this repository.

### Installation

1. Clone or download this repository.
2. Open `JasperGreenProjectPhaseIII.sln` in Visual Studio.
3. Create `appsettings.Development.json`.
4. Add the required private configuration values.
5. Apply the included Entity Framework Core migrations.
6. Build and run the application through Visual Studio.

A valid PdfMyHtml API key is required for PDF invoice generation. The remaining application functionality does not depend on that integration.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

<!-- PROJECT BACKGROUND -->
## Project Background

I originally developed Jasper Green Operations for an Application Development course in the Management Information Systems program at Texas A&M University's Mays Business School.

The course requirements focused on building a functional ASP.NET Core MVC application using C#, SQL Server, Entity Framework Core, Razor, relational data, CRUD workflows, filtering, and validation.

I independently developed the application and expanded it beyond those requirements with:

- ASP.NET Core Identity authentication
- System-user and password management
- A custom operations dashboard
- Revenue and service-activity metrics
- Expanded sorting and filtering
- Additional relational business rules
- Dynamic customer and property form behavior
- PDF invoice generation through an external API
- A redesigned public-facing website
- Additional demonstration data and interface improvements

The project gave me the opportunity to work through the full process of translating an operational scenario into a relational data model, application workflows, business rules, and a usable interface.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

<!-- TAKEAWAYS -->
## What I Took From the Project

The most valuable part of this project was seeing how application development connects to the broader work of information systems.

Building the system required more than writing C#. I had to think through how the business operates, which data belongs together, what users need to accomplish, which rules the system should enforce, and how the application should respond when those rules are violated.

That intersection between business requirements and technical implementation is the area of technology I am most interested in continuing to work in.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

<!-- CONTACT -->
## Contact

**Cole Howell**  
Management Information Systems  
Texas A&M University, Mays Business School  
December 2026

LinkedIn: [linkedin.com/in/cole-howell](https://www.linkedin.com/in/cole-howell/)

Project: [github.com/colebh03/jasper-green-operations](https://github.com/colebh03/jasper-green-operations)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

<!-- MARKDOWN LINKS & BADGES -->
[csharp-shield]: https://img.shields.io/badge/C%23-512BD4?style=for-the-badge&logo=csharp&logoColor=white
[csharp-url]: https://learn.microsoft.com/en-us/dotnet/csharp/

[dotnet-shield]: https://img.shields.io/badge/.NET_8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white
[dotnet-url]: https://dotnet.microsoft.com/

[aspnet-shield]: https://img.shields.io/badge/ASP.NET_Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white
[aspnet-url]: https://learn.microsoft.com/en-us/aspnet/core/

[sqlserver-shield]: https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white
[sqlserver-url]: https://www.microsoft.com/en-us/sql-server/

[bootstrap-shield]: https://img.shields.io/badge/Bootstrap-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white
[bootstrap-url]: https://getbootstrap.com/

[linkedin-shield]: https://img.shields.io/badge/LinkedIn-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white
[linkedin-url]: https://www.linkedin.com/in/cole-howell/
