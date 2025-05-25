# 📝 SimpleBlog

SimpleBlog is a monolithic blog platform built with ASP.NET Core, using Domain-Driven Design (DDD) principles. It allows users to register, authenticate, create, edit, and delete blog posts. Real-time notifications are sent to connected clients when new posts are published, using WebSockets.

---

## ✅ Features

### 🔐 Authentication

- Register new users via `POST /users/register`
- User login with JWT token via `POST /users/authenticate`
- Secure routes using JWT Bearer authentication
- Password hashing and strong password validation

### ✏️ Post Management

- Create new posts (`POST /posts`) by authenticated users
- Edit existing posts (`PUT /posts`) by the post's author
- Delete posts (`DELETE /posts/{id}`), with ownership validation
- All operations are protected by JWT authentication

### 🌐 Post Viewing

- Public paginated listing of posts via `GET /posts`
- Returns post title, content, author's name, and last updated date

### 🔔 Real-Time Notifications

- WebSocket endpoint protected by JWT
- Automatically notifies connected clients when a new post is published
- Message validation to prevent malformed or harmful payloads

---

## 🧱 Project Structure

Organized using DDD layers:

```
/SimpleBlog
├── SimpleBlog.Domain          # Core business logic (Entities, ValueObjects, Interfaces)
├── SimpleBlog.Application     # Application services and DTOs
├── SimpleBlog.Infrastructure  # Data access, JWT, Repositories, WebSockets
├── SimpleBlog.API             # Controllers, Middleware, Swagger, DI setup
```

---

## 🛠️ Technologies Used

- **.NET 9**
- **Entity Framework Core**
- **JWT Authentication**
- **WebSockets**
- **Swagger for API documentation**
- **Azure Data Studio / SQL Server**

---

## ▶️ Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/)
- [Azure Data Studio](https://learn.microsoft.com/en-us/sql/azure-data-studio/)

### Setup

1. **Clone the repository**

   ```bash
   git clone https://github.com/yourusername/SimpleBlog.git
   cd SimpleBlog
   ```

2. **Configure `appsettings.json`**

   In `SimpleBlog.API/appsettings.json`, set your JWT secret and SQL connection string:

   ```json
   "JwtSettings": {
     "Secret": "masterblastersecret1234567890supersecret",
     "Issuer": "SimpleBlog",
     "Audience": "SimpleBlogUsers",
     "ExpiresInMinutes": 60
   },
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=SimpleBlogDb;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

3. **Apply EF Core Migrations**

   ```bash
   dotnet ef database update --project SimpleBlog.Infrastructure
   ```

4. **Run the project**

   ```bash
   dotnet run --project SimpleBlog.API
   ```

5. **Open Swagger UI**

   Navigate to [https://localhost:5001/swagger](https://localhost:5001/swagger)

---

## 📡 WebSocket Endpoint

Connect to the WebSocket at:

```
ws://localhost:5001/ws/notifications
```

Include the JWT token in the query string:

```
ws://localhost:5001/ws/notifications?access_token={your_token}
```

---

## 🧰 Additional Tools

- **Middleware for global exception handling**
- **Custom password validation**
- **DTOs for data encapsulation**
- **Service interfaces for testability and separation of concerns**

---

## 🧑‍💻 Author

Developed by [Hiago](https://github.com/hiagoindalecio)

---

## 📄 License

This project is licensed under the MIT License.
