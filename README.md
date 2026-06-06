# 🍔 FoodOrderingSystem

![Build with .NET](https://img.shields.io/badge/.NET-6%2B-blueviolet?logo=dotnet)
![C#](https://img.shields.io/badge/C%23-Backend-blue?logo=csharp)
![Docker](https://img.shields.io/badge/Docker-Ready-2496ed?logo=docker)
![PostgreSQL](https://img.shields.io/badge/Postgres-Database-336791?logo=postgresql)
![Redis](https://img.shields.io/badge/Redis-Cache-DC382D?logo=redis)
![Dapper](https://img.shields.io/badge/Dapper-ORM-6d429c?logo=.net)
![EF Core](https://img.shields.io/badge/EF%20Core-ORM-512BD4?logo=ef)
![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)

Welcome to **FoodOrderingSystem**!  
A modern backend for real-world online food ordering — manage restaurants, menu, cart, and orders with a robust and scalable architecture. 🚀

---

## ✨ Tech Stack & Features

- **C# / .NET 9** API backend, built for scalability
- **🍴 PostgreSQL** for primary data storage
- **⚡ Redis** for caching (used for restaurant listing, cart, and more)
- **🔑 JWT** authentication (access and refresh tokens via secure cookies)
- **Dapper** for high-performance, SQL-centric data access in addition to Entity Framework
- **Serilog + Seq** for centralized structured logging
- **Swagger** for easy, interactive API docs
- **Extensive Unit/Integration Tests**
- **Ready-to-launch Docker Compose orchestration**

---

## ⚡️ One-Second Quickstart

Spin up API, database, Redis & logging instantly with:

```bash
docker-compose up --build
```

- API: [http://localhost:18080](http://localhost:18080)
- Swagger UI: [http://localhost:18080/swagger](http://localhost:18080/swagger)
- Seq Logs: [http://localhost:15341](http://localhost:15341)

No manual installation required—everything is containerized!

---

## 🛣️ API Endpoints & How They Work

Endpoints prefixed with `/api/`, most require authentication (see Auth block).

### 🔑 Auth Endpoints

| Method | Endpoint            | Description                                    |
|--------|---------------------|------------------------------------------------|
| POST   | `/api/auth/login`   | Log in with `{ email, password }`. Issues JWT access & refresh tokens as **secure cookies**. |
| POST   | `/api/auth/logout`  | Logs you out by deleting your auth cookies.    |
| POST   | `/api/auth/register`| Register a new user. Validates unique email, stores hashed credentials. |
| POST   | `/api/auth/refresh` | Uses the refresh token cookie to obtain a new access token, for seamless user experience. |

**How it works:**  
User login/registration uses JWT and stores tokens both as cookies and (for refresh) in the DB/Redis for extra security. All requests requiring authentication check the JWT in cookies.

---

### 🗺 Restaurants

| Method | Endpoint                      | Description           |
|--------|-------------------------------|-----------------------|
| GET    | `/api/restaurants`            | **List all restaurants. First tries to load from Redis cache for ultra-fast reads; if not cached, loads from Postgres DB via Dapper and caches the result for next time.** |
| GET    | `/api/restaurants/{id}`       | Get a single restaurant by ID (cacheable). |
| POST   | `/api/restaurants`            | Create new restaurant (Admin only). Updates DB and invalidates Redis cache. |
| PUT    | `/api/restaurants/{id}`       | Update restaurant (Admin only). Keeps cache up-to-date. |
| DELETE | `/api/restaurants/{id}`       | Delete restaurant (Admin only). Also updates cache. |

**Tech Highlights:**  
- Uses **Dapper** for performant restaurant queries.
- Ensures high throughput and low DB load through **Redis-based caching**.
- Cache is automatically invalidated on write operations to keep reads always up-to-date!

---

### 🍽 Menu Items

| Method | Endpoint                                      | Description                                                    |
|--------|-----------------------------------------------|----------------------------------------------------------------|
| GET    | `/api/menu-items`                             | List all menu items (can be cached).                           |
| GET    | `/api/menu-items/by-restaurant/{restaurantId}`| List all menu items for a restaurant. Also uses Redis cache for speed! |
| POST   | `/api/menu-items`                             | Create a menu item (Admin only), auto-invalidate cache.        |
| PUT    | `/api/menu-items/{id}`                        | Update a menu item (Admin only). Re-caches accordingly.        |
| DELETE | `/api/menu-items/{id}`                        | Delete menu item (Admin only) and update cache.                |

---

### 🛒 Cart

| Method | Endpoint                          | Description                                         |
|--------|-----------------------------------|-----------------------------------------------------|
| GET    | `/api/cart`                       | Get your current cart from Redis (super rapid retrieval, persistent across sessions). |
| POST   | `/api/cart/add/{menuItemId}`      | Add menu item to your cart (`quantity` param); updates both Redis and DB if needed. |
| POST   | `/api/cart/remove/{menuItemId}`   | Remove a quantity of item(s) from cart.             |
| POST   | `/api/cart/clear`                 | Empty your cart instantly.                          |

**Behind the Scenes:**  
All cart operations leverage Redis for low-latency, real-time updates. Data is synced with persistent store as appropriate.

---

### 🧾 Orders

| Method | Endpoint        | Description                              |
|--------|-----------------|------------------------------------------|
| POST   | `/api/orders`   | Place current cart as a new order. Validates presence of items, checks menu state, stores in Postgres. |
| GET    | `/api/orders/mine` | Lists your own orders (cached where possible for speed). |

---

## 🏗 Project Structure

```plaintext
Backend/
  FoodOrderingSystem.Api/           # .NET API endpoints, Dependency Injection
  FoodOrderingSystem.Application/   # Business/application logic, SQRS
  FoodOrderingSystem.Domain/        # Business entities and value objects
  FoodOrderingSystem.Infrastructure/# Data access (Postgres, Dapper, EF), Redis, Auth
  FoodOrderingSystem.UnitTests/     # Unit tests
  FoodOrderingSystem.IntegrationTests/ # End-to-end (E2E) tests
docker-compose.yml                  # All-in-one orchestration 🚀
```

---

## 🔧 How Dapper, Redis, and Caching Work

- **Dapper**: Used for fast, SQL-optimized data queries especially for large or frequent list-type reads (e.g. all restaurants).
- **Redis**: Accelerates all read/lookup operations for restaurants, menu, and carts by acting as a caching layer and session store.
- **Cache Invalidation**: When a restaurant or menu item is created/updated/deleted, the cache for that entity or list is cleared to ensure next fetch gets fresh data.
- **Fallback**: On cache-miss, data is always retrieved from Postgres (using Dapper or EF as most appropriate), then cached for future requests.

---

## ⚡️ Contributing & License

PRs and suggestions are welcome!  
MIT licensed.  
Made by [Yunkeq](https://github.com/Yunkeq) with ❤️

---

## 🍦 Enjoy blazing-fast food ordering APIs with modern C#, Dapper, Redis, and Docker!
