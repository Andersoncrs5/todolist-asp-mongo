# ToDoList API Project 

Status: finish

This repository contains a robust and scalable ToDoList API built with ASP.NET Core, designed with maintainability, testability, and performance in mind. It leverages best practices in software development, including SOLID principles, various design patterns, and a comprehensive testing strategy.

## Features 
    Task Management: Full CRUD (Create, Read, Update, Delete) operations for tasks.

    MongoDB Integration: Persistent storage for tasks using MongoDB as the NoSQL database.

    Pagination: Efficiently retrieve tasks with built-in pagination support.

    Status Toggling: Easily change the completion status of tasks.

    Bulk Deletion: Delete multiple tasks at once.

    Rate Limiting: Protects the API from abuse with SlidingWindowLimiterPolicy.

## Architecture & Design Patterns 
    The project is structured to ensure high cohesion and low coupling, adhering to the following principles and patterns:

    SOLID Principles: Guiding the design for maintainable and scalable code.

    Repository Pattern: Abstracts the data access layer, decoupling the application from the database technology (MongoDB).

    Unit of Work Pattern: Manages transactions and ensures data consistency across multiple repository operations.

    Dependency Injection (DI): Facilitates loose coupling and testability by injecting dependencies (like repositories and database contexts).

    Minimal APIs (ASP.NET Core .NET 9): Utilizes the streamlined and high-performance API development model.

    DTOs (Data Transfer Objects): Ensures clear separation between domain models and API contract models, improving security and flexibility.


## A multi-layered testing approach ensures the reliability and correctness of the API:

    Unit Tests: Focus on individual components (e.g., business logic in services, filter application logic) in isolation, using mocking frameworks like Moq.

    Integration Tests: Verify the interaction between different layers of the application (controllers, services, repositories) with an in-memory test host (Microsoft.AspNetCore.Mvc.Testing). These tests mock the MongoDB interactions to ensure speed and isolation from a live database.

    End-to-End (E2E) Tests: Validate the entire system flow from an external perspective, interacting with the API running on a real (or test-managed) MongoDB instance. These tests simulate real user scenarios and ensure all components work together seamlessly.

## Technologies Used 🛠️
    ASP.NET Core (.NET 9): The primary framework for building the API.

    MongoDB: NoSQL database for data persistence.

    C#: The programming language used.

    xUnit: Testing framework for unit, integration, and E2E tests.

    Moq: Mocking library for isolating dependencies in unit and integration tests.

    System.Text.Json / Newtonsoft.Json: For JSON serialization and deserialization.

## Project Structure

    ToDoList/
    ├── ToDoList.API/                      # Main ASP.NET Core Web API project
    │   ├── Controllers/
    │   ├── Models/
    │   ├── Repositories/
    │   ├── DTOs/
    │   ├── Settings/                      # MongoDB settings
    │   ├── Program.cs                     # API entry point
    │   └── appsettings.json               # Application configuration
    ├── ToDoList.Tests/                    # Base folder for all test projects
    │   ├── Integration/
    │   │   ├── ToDoList.Tests.Integration.csproj # Integration Tests project
    │   │   ├── CustomWebApplicationFactory.cs    # Custom factory for test environment
    │   │   └── ... (Integration test files)
    │   └── E2E/
    │       ├── ToDoList.Tests.E2E.csproj         # E2E Tests project
    │       ├── E2EBaseTest.cs                    # Base class for E2E tests (manages API host)
    │       └── ... (E2E test files)
    └── README.md
