# Personal Finance Management API

A personal finance management REST API built with **ASP.NET Core** for managing personal income and expenses.

The project was developed as a practical software engineering project to apply concepts such as **Clean Architecture, Domain Modeling, Entity Framework Core, Authentication, Multi-Currency Support, and Automated Testing**.

## Features

* User registration and authentication
* Create, update, delete, and view financial funds
* Support for both income and expenses
* User-owned categories
* Multi-currency support
* Historical exchange-rate support
* Calculate the user's balance
* Filter funds by date and category
* Users can only access their own financial data
* Validation of important business rules
* Entity Framework Core migrations
* Automated tests

## Business Rules

Some of the main rules implemented by the system:

* Fund amount must be greater than zero.
* A fund cannot belong to another user.
* A category belongs to a specific user.
* Category names must be unique for each user.
* A category cannot be deleted while it is used by a fund.
* A fund date cannot be in the future.
* A fund cannot be dated before the user's date of birth.
* The user cannot change the owner of an existing fund.
* Income increases the balance, while expenses decrease it.
* Exchange rates are stored with funds so historical transactions can use the appropriate rate.

## Technologies

* **C#**
* **ASP.NET Core Web API**
* **Entity Framework Core**
* **SQLite**
* **ASP.NET Core Identity**
* **JWT Authentication**
* **REST API**
* **Clean Architecture principles**
* **Unit Testing using xUnit and Moq**

## Architecture

The project is organized into several layers:

```text
Finance.Domain <---------+
        ↓                |
Finance.Application <----|
        ↓                |
Finance.Infrastructure <-|
        ↓                |
Finance.Api <------------|
                         |
                   Finance.Test
```

### Domain

Contains the core business model and rules, including:

* Entities
* Value Objects
* Enums
* Domain invariants

### Application

Contains the application's use cases and business orchestration.

Examples:

* Fund management
* Category management
* Total calculation
* User ownership checks

### Infrastructure

Contains technical implementations such as:

* Entity Framework Core
* SQLite
* Repositories
* ASP.NET Identity
* JWT
* Currency/exchange-rate services
* Database configurations
* EF Core migrations

### API

The presentation layer responsible for:

* HTTP endpoints
* Authentication
* Request/response handling
* Dependency Injection configuration

### Test

Contains tests for Domain and Application layers to check:

* Domain invariants
* Business orchestration
* Authentication and Authentication
* Returning the appropriate responses

## Database

The project currently uses **SQLite** for development.

The SQLite database file is generated locally and is not included in the repository.

Database migrations are stored in:

```text
Finance.Infrastructure/
└── Persistence/
    └── Migrations/
```

## Getting Started

### Prerequisites

Make sure you have:

* [.NET SDK](https://dotnet.microsoft.com/)
* Git

### Clone the repository

```bash
git clone <repository-url>
cd <repository-folder>
```

### Apply database migrations

using .NET CLI:
```bash
dotnet ef database update \
    --project Finance.Infrastructure \
    --startup-project Finance.Api
```

or Visual Studio:
```bash
Update-Database -Project Finance.Infrastructure -Startup-Project Finance.Api
```

### Run the application

```bash
dotnet run --project Finance.Api
```

The API can then be accessed through the URL shown by ASP.NET Core when the application starts.

## Project Structure

```text
Finance/
│
├── Finance.Domain/
│   ├── Entities/
│   ├── Enums/
│   └── ValueObjects/
│
├── Finance.Application/
│   ├── Abstractions/
│   ├── Requests/
│   ├── Responses/
│   └── Services/
│
├── Finance.Infrastructure/
│   ├── Identity/
│   ├── Persistence/
│   │   ├── Configurations/
│   │   ├── Migrations/
│   │   └── Repositories/
│   └── Services/
│
└── Finance.Api/
    └── ...
```

## Testing

The project includes automated tests for important business behavior and application use cases.

The tests focus on validating behavior and business rules rather than simply maximizing code coverage.

## Exchange Rates

The application can use Frankfurter API exchange-rate provider to obtain historical exchange rates.

The exchange rate used for a fund is stored with the transaction so that historical funds are not affected by future changes in exchange rates.

## What I Learned

This project was mainly built to practice the process of developing a system from requirements rather than following a tutorial.

Through the project I practiced:

* Translating requirements into business rules
* Designing domain entities and value objects
* Defining application use cases
* Separating business logic from infrastructure concerns
* Designing repository abstractions
* Working with EF Core and migrations
* Implementing authentication and authorization
* Handling ownership and access rules
* Working with multi-currency transactions
* Writing automated tests
* Structuring an ASP.NET Core Web API

## Project Status

This project is a learning and portfolio project.

The architecture and implementation are intentionally kept relatively simple rather than introducing patterns and infrastructure that are unnecessary for the project's scope.

## License

This project is available for educational and portfolio purposes.
