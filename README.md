ECommerce APIs
Robust ASP.NET Core Web APIs for e-commerce operations with product and category management, JWT authentication, and Swagger documentation.

Features
✅ Product Management (CRUD operations with image upload)

✅ Category Management (CRUD operations with validation)

✅ JWT Authentication & Authorization

✅ Swagger/OpenAPI Documentation

✅ Entity Framework Core with Code First Migrations

✅ Repository Pattern & Unit of Work

✅ AutoMapper for DTO transformations

✅ Global Exception Handling

✅ File Upload Support

✅ Pagination and Filtering

Prerequisites
.NET 8.0 SDK 

SQL Server (LocalDB, Express, or full version)

Visual Studio 2022 or VS Code

Setup Instructions
1. Clone the Repository
2. Database Setup
    "ConnectionStrings": {
        "DefaultConnection": "Server=.;Database=ECommerceDb;Trusted_Connection=true;TrustServerCertificate=true;"
    },

3.Apply Database Migrations 
Run this commands in the Package Manager Console:
update-database
