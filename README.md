# Loan Service API

## Overview

The Loan Service API is a microservice-based application built using C# and .NET Core. It facilitates the management of loan requests, approvals, repayments, and provides a secure solution for handling customer financial transactions.

## Features

1. **Loan Creation:**
   - Customers can submit loan requests specifying the amount and term.
   - Scheduled repayments are generated automatically based on the loan details.

2. **Admin Approval:**
   - Admins have the authority to approve pending loan requests, changing their state to APPROVED.

3. **Customer View:**
   - Customers can view their own loans, with policy checks to ensure privacy.

4. **Repayment Handling:**
   - Customers can fulfill the repayment installments.
   - When all scheduled repayments are PAID, the entire loan is marked as PAID.

## Technologies Used

- **Programming Language:** C#
- **Framework:** .NET Core
- **Testing:** xUnit, Fluent Assertions, Auto Fixture, Moq
- **Authentication:** JWT
- **Documentation:** Swagger
- **Logging:** Serilog/Console/...
- **Database:** SQL

## Getting Started

### Clone the Repository:

```bash
git clone https://github.com/abhishek054/AspireLoanManagement.git
```

### Install Dependencies:

```bash
cd .\AspireLoanManagement
dotnet restore
```

### Database Setup in Local Environment:

1. Install SQL Server.
2. Update the connection string in the configuration:

   ```json
   "ConnectionStrings": {
     "AspireDBConnectionString": "Server=(localdb)\\MSSQLLocalDB;Database=AspireDB;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   ```

3. Run the following command for database schema migration:


   ```bash
   dotnet ef database update
   ```
## Aspire Loan Management mini-API Features 

### Caching

- **Choice of Cache Service**: The application allows users to choose the cache service based on configuration settings.
  
- **Cache Manager**: A Cache Manager handles interactions with the selected cache service. It abstracts the caching logic and allows for easy extension to support other caching solutions.

- **Extensibility**: The caching implementation is open for extension to accommodate future caching solutions. The current implementation supports both In-Memory caching and Redis caching.

### Logging

- **Choice of Log Service**: The application provides flexibility in selecting the log service based on configuration settings.

- **Log Manager**: A Log Manager orchestrates log services and supports various log levels. It allows for easy extension to incorporate other logging solutions.

- **Extensibility**: Similar to caching, the logging implementation is extensible, welcoming additional logging solutions. The current implementation supports Console logs.

### Attributes

- **User-Context-Attribute**: The application utilizes the LoanToUserMapping attribute to manage user details, providing a means for the application to establish relationships with loans.

### Config

- **AspireConfigService**: Configurations are easily accessible throughout the application using the AspireConfigService. It retrieves configurations in a structured class format (AspireConfigModel).

### Exception Handling

- **ExceptionHandlingMiddleware**: Implemented middleware that catches any unhandled exceptions in the application. It allows for actions like setting error codes, creating response objects, and providing generic error messages.

### Mapper

- **Auto-Mapper**: Utilizes Auto-Mapper to map Data Transfer Objects (DTOs) to ViewModels and vice-versa. This simplifies modifications to models across different layers of the application.

### Validator

- **Fluent Assertion**: Uses Fluent Assertion for input validation. For example, in the Loan Controller, this validator is employed in a structured manner to retrieve all errors in the payload while creating a loan.

### Dependency Injection

- **Service Registration**: Services are registered (Scoped, Transient, Singleton) in Program.cs. Interfaces of services are provided wherever needed, facilitating dependency injection without explicitly constructing the respective class.

### Swagger

- **Interactive API Documentation**: The application utilizes Swagger to provide an interactive API documentation interface. Users can test APIs and view exposed models directly from the Swagger window.

### Unit Testing

- **AspireLoanManagementTest Project**: Unit testing is implemented in a separate project named "AspireLoanManagementTest." This ensures the reliability and correctness of individual components.

### Versioning

- **URL-Based Versioning**: Implements URL-based versioning to manage different versions of the API.

## Improvement Opportunities / TODO

### Transaction

**Opportunity:**
Implementing transactions can ensure atomicity, consistency, isolation, and durability (ACID properties) for operations that involve multiple steps. This is crucial for maintaining data integrity, especially in scenarios where multiple database updates must be treated as a single unit of work.

**Recommendation:**
Explore the use of transactions for critical operations, ensuring that either all steps of the operation succeed or none at all. Ex. The creation of loans and repayments should be in a single transaction. Also, the Settlement of multiple repayments (If needed) should happen in a single transaction

---

### Versioning

**Opportunity:**
While URL-based versioning is implemented, there are alternative versioning strategies, such as header-based or query parameter-based versioning.

**Recommendation:**
Explore and choose the versioning strategy that best aligns with your application's requirements and API design principles.

---

### Swagger Authentication

**Opportunity:**
Securing the Swagger documentation with authentication can restrict unauthorized access and ensure that only authenticated users can interact with the API documentation.

**Recommendation:**
Implement authentication for the Swagger UI, potentially integrating it with the same authentication mechanism used for the API itself.

---

### Rate Limiting

**Opportunity:**
Implementing rate limiting helps protect your API against abuse by limiting the number of requests a client can make within a specified time frame.

**Recommendation:**
Integrate rate-limiting mechanisms, such as token buckets or sliding window counters, to prevent abuse and ensure fair usage of your API resources.

---

### CI/CD

**Opportunity:**
Continuous Integration (CI) and Continuous Deployment (CD) practices streamline the development and deployment processes, improving collaboration and reducing manual errors.

**Recommendation:**
Set up a CI/CD pipeline to automate build, test, and deployment processes. Tools like Jenkins, GitLab CI, or GitHub Actions can assist in achieving this.

---

### Containerizing

**Opportunity:**
Containerization with tools like Docker enables consistent deployment across different environments and simplifies dependency management.

**Recommendation:**
Explore containerization for your application. Dockerizing your API allows you to package it with its dependencies, providing a consistent and reproducible deployment environment.

---
