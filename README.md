# EcommerceAPI

> ASP.NET Core Web API for managing an ecommerce backend: categories, products and sales.

# Requirements

- [x] The project is an ASP.NET Core Web API using Entity Framework Core with SQL Server.
- [x] The API uses Dependency Injection (constructor injection on controllers/services, services registered through a custom `IServiceCollection` extension).
- [x] There are at least three tables: `Categories`, `Products` and `Sales`.
- [x] `Products` and `Sales` have a many-to-many relationship through a join entity (`SaleDetail`) that also stores `Quantity`, `UnitPrice` and `Subtotal` for each product sold.
- [x] Products have a `Price`, and a single sale can include multiple products.
- [x] `GetProducts` and `GetSales` endpoints support pagination through a generic `PagedResultDTO<T>`.
- [x] Controllers are kept lean: all business logic lives in services (`CategoryService`, `ProductService`, `SaleService`).
- [x] Postman Collection with all requests (`EcommerceAPI.postman_collection.json`, included in the repo).
- [x] Deletion is prevented by design: no `DELETE` endpoints were added for `Category`, `Product` or `Sale` (see *Areas to improve* for the reasoning), so soft-deletes were not needed.
- [x] Product price cannot be updated: `Price` was removed from `UpdateProductDTO`, so it can only be set once, on creation.

# API EcommerceAPI

The EcommerceAPI is a RESTful ASP.NET Core Web API designed to manage a small retail catalog: categories, products and the sales made from them.

It provides endpoints to create and retrieve categories and products, update some of their fields, register sales (deducting stock automatically) and update a sale's status.

The API uses Entity Framework Core with SQL Server for persistence, follows a layered architecture (Controllers, Services, DTOs and Models), centralizes error handling through a custom exception/result pattern (`Error`, `EcommerceException`, `ErrorMapper`) combined with ASP.NET Core's `IExceptionHandler` and `ProblemDetails`, and includes logging with `ILogger` across all services.

## Data Model

| Entity | Description |
|--------|--------------|
| **Category** | Has a name and description. A category can have many products. |
| **Product** | Belongs to one category. Has name, description, price and stock. |
| **Sale** | Represents a purchase transaction. Has a date, total and status (`Pending`, `Completed`, `Cancelled`, `Refunded`). |
| **SaleDetail** | Join entity between `Sale` and `Product` (many-to-many). Stores quantity, unit price and subtotal for each product in a sale. |

When a sale is created, the API validates that every product exists and has enough stock, then discounts the sold quantity from stock and calculates the subtotal/total automatically.

## API Endpoints

### Categories

| Method | Endpoint | Description | Success Response |
|:------:|----------|-------------|:-----------------:|
| **GET** | `/api/categories` | Returns all categories, including their products. | `200 OK` |
| **GET** | `/api/categories/{id}` | Returns a category by its ID. | `200 OK` |
| **POST** | `/api/categories` | Creates a new category. | `201 Created` |
| **PATCH** | `/api/categories/{id}` | Partially updates a category (name and/or description). | `204 No Content` |

### Products

| Method | Endpoint | Description | Success Response |
|:------:|----------|-------------|:-----------------:|
| **GET** | `/api/products` | Returns a paginated, filterable and sortable list of products. | `200 OK` |
| **GET** | `/api/products/{id}` | Returns a product by its ID. | `200 OK` |
| **POST** | `/api/products` | Creates a new product (validates that the category exists). | `201 Created` |
| **PATCH** | `/api/products/{id}` | Partially updates a product (name, description, stock and/or category — price is not editable). | `204 No Content` |

**Query parameters for `GET /api/products`:**

- `pageNumber`, `pageSize` — pagination (defaults: page 1, size 10, max 100).
- `name` — filters by partial name match.
- `categoryId` — filters by category.
- `minPrice`, `maxPrice` — filters by price range.
- `inStock` — filters products with stock greater than 0.
- `sortBy` — `name`, `price`, `stock` or `id` (default).
- `descending` — sort direction.

### Sales

| Method | Endpoint | Description | Success Response |
|:------:|----------|-------------|:-----------------:|
| **GET** | `/api/sales` | Returns a paginated list of sales. | `200 OK` |
| **GET** | `/api/sales/{id}` | Returns a sale by its ID, including its details. | `200 OK` |
| **POST** | `/api/sales` | Creates a new sale from a list of products/quantities, validating stock and discounting it. | `201 Created` |
| **PATCH** | `/api/sales/{id}` | Updates the status of a sale (`Pending`, `Completed`, `Cancelled`, `Refunded`). | `204 No Content` |

## Validation

- Category name is required, between 5 and 100 characters.
- Product name is required, between 3 and 100 characters; description limited to 500 characters.
- Product price must be greater than 0; stock cannot be negative.
- Every product added to a sale requires a valid `ProductId` and a quantity greater than 0.
- A sale must include at least one product.
- Validation is enforced with Data Annotations on the `Create*` DTOs and automatically returned as `400 Bad Request` through ASP.NET Core's model validation.

## Error Handling

Business and domain errors are represented with an `Error` record (`Code`, `Description`, `ErrorType`) and thrown as an `EcommerceException`. An `ErrorMapper` translates each `ErrorType` (`Validation`, `NotFound`, `Conflict`, `BusinessRule`, `Failure`) into the matching HTTP status code.

A global exception handler (`GlobalExceptionHandler`, implementing `IExceptionHandler`) catches these exceptions — as well as EF Core's `DbUpdateException`/`DbUpdateConcurrencyException` — logs them with `ILogger`, and returns a consistent `ProblemDetails` response.

- Not found resources → `404 Not Found`
- Invalid input / business rule violations (e.g. insufficient stock) → `400 Bad Request`
- Concurrency conflicts → `409 Conflict`
- Unhandled errors → `500 Internal Server Error`

## Tech Stack

- ASP.NET Core Web API
- Entity Framework Core (Code First, SQL Server)
- Dependency Injection
- Swagger / OpenAPI
- `ILogger` for logging

# Challenges

- Modeling a many-to-many relationship between `Products` and `Sales` through a join entity with its own data (quantity, unit price, subtotal).
- Adding filtering, sorting and pagination to the `GetProducts` endpoint.
- Designing a consistent error-handling strategy (`Error` + `EcommerceException` + `ErrorMapper`) decoupled from ASP.NET Core, then wiring it into the framework's exception handling pipeline.
- Keeping stock consistent when creating a sale: validating availability for every product before committing any change.

# What I have learned

- Modeling many-to-many relationships with a rich join entity using EF Core.
- Building reusable pagination, filtering and sorting logic for list endpoints.
- Designing a custom domain error model and mapping it cleanly to HTTP responses with `ProblemDetails`.
- Keeping controllers thin and pushing all business logic into services.
- Using `ILogger` to trace both successful operations and warnings/errors across the service layer.

# Areas to improve

- **Design note:** No `DELETE` endpoints or soft-deletes were implemented for `Category`, `Product` or `Sale`. In a retail context, deleting a product or category that has already been sold would break the integrity of historical sale records (`SaleDetail`), since they depend on the original product data. Updates (`PATCH`) are favored over deletion instead, following the project's own suggestion that not every table needs every CRUD operation.
- Remove the unused `Result`/legacy `GlobalExceptionHandlerMiddleware` in favor of the single `GlobalExceptionHandler` approach, or finish integrating `Result` consistently across services.
- Integration tests.
- CI/CD with GitHub Actions.
- Docker support.

# Resources Used

- Microsoft Learn - ASP.NET Core Web API documentation: https://learn.microsoft.com/aspnet/core/web-api/
- Microsoft Learn - Entity Framework Core documentation: https://learn.microsoft.com/ef/core/
- Microsoft Learn - Many-to-many relationships: https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many
- Microsoft Learn - Dependency Injection: https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-usage
- The C# Academy - Dependency Injection Tutorial: https://thecsharpacademy.com/course/6/article/1/500150/False
- C# Corner - Pagination and Filtering in ASP.NET Core 8.0: https://www.c-sharpcorner.com/article/implementing-pagination-and-filtering-in-asp-net-core-8-0-api/
- Medium - Understanding REST and RESTful APIs: https://medium.com/@bpst.blog/understanding-rest-and-restful-apis-constraints-methods-and-examples-76b2d1b63003
- Swagger / OpenAPI Documentation: https://swagger.io/docs/
- The C# Academy - Ecommerce API Project: https://www.thecsharpacademy.com/project/18/ecommerce-api
