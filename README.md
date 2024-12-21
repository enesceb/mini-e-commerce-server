# 1likte API Project

## Table of Contents
- [Introduction](#introduction)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Requirements](#requirements)
- [Setup and Installation](#setup-and-installation)
- [Running the Project](#running-the-project)
- [Docker Usage](#docker-usage)
- [Environment Variables](#environment-variables)
- [Endpoints](#endpoints)
- [License](#license)

---

## Introduction
The **1likte API Project** is a backend service designed to manage operations like user management, category management, and backups. The project is built using .NET 8 and PostgreSQL and supports Docker deployment for easy setup and scaling.

---

## Features
- **User Authentication & Authorization**
  - JWT-based authentication
  - Role-based authorization (e.g., Admin, User)
- **Category Management**
  - CRUD operations for categories
- **Swagger Documentation**

---

## Technologies Used
- **Frameworks:** .NET 8 (ASP.NET Core), EF CORE
- **Database:** PostgreSQL 15
- **Containerization:** Docker & Docker Compose
- **Libraries:**
  - `Swashbuckle` for Swagger API documentation
  - `Microsoft.AspNetCore.Authentication.JwtBearer` for authentication
  - `Microsoft.IdentityModel.Tokens` for token validation
  - `Microsoft.EntityFrameworkCore` for DB Operations

---

## Requirements
- **.NET SDK:** Version 8.0+
- **.EF CORE:** Version 9.0+
- **Docker:** Version 20.10+
- **PostgreSQL:** Version 15+

---

## Setup and Installation
### 1. Clone the Repository
```bash
git clone https://github.com/your-username/1likte-api.git
cd 1likte-api
```

### 2. Configure Environment Variables
Create an `appsettings.json` file in the `1likte.API` project directory or use Docker secrets for production. Example configuration:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=postgres_db;Port=5432;Database=1likte-postgres;Username=myuser;Password=mypass"
  },
  "TokenOption": {
    "Issuer": "1likte",
    "Audience": ["1likte-audience"],
    "SecurityKey": "your-secure-key",
    "AccessTokenExpiration": 60
  }
}
```

### 3. Install Dependencies
```bash
cd 1likte.API
dotnet restore
```

---

## Running the Project
### 1. Run Locally
```bash
dotnet run --project 1likte.API/1likte.API.csproj
```
Access the API at `http://localhost:5000` (default port).

### 2. Using Docker Compose
```bash
docker-compose up --build
```
Access the API at `http://localhost:8080` and the database at `localhost:5432`.

---

## Docker Usage
### Build and Run Containers
1. Build the image:
   ```bash
   docker-compose build
   ```
2. Start the services:
   ```bash
   docker-compose up
   ```

### Stopping Services
```bash
docker-compose down
```

---

## Environment Variables
Below are the required environment variables:

| Key                | Description                        |
|--------------------|------------------------------------|
| `DefaultConnection`| PostgreSQL connection string       |
| `Issuer`           | Token issuer                      |
| `Audience`         | Token audience                    |
| `SecurityKey`      | Token signing key                 |
| `AccessTokenExpiration` | Token expiration time in minutes |

---

## Endpoints
### Swagger Documentation
Visit `http://localhost:8080/swagger/index.html` for the full API documentation.

### Example Endpoints
1. **Authentication**
   - `POST /api/auth/login`: Login and get a JWT token.
2. **Categories**
   - `GET /api/categories`: List all categories.
   - `POST /api/categories`: Create a new category (Admin only).
3. **Products**
   - `POST /api/prodcuts`: Create a product (Admin and user only).

---

## License
This project is licensed under the MIT License. See the LICENSE file for more information.

---

