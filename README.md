# Readytech - Coffee Machine API

An HTTP API that simulates an internet-connected coffee machine.

---

## 🧱 Technologies

- [.NET 9](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-9/overview)
- ASP.NET Core
- xUnit (for testing)
- Moq (for mocking)
- Swagger (API documentation)

---

## 🚀 How to Run the Project

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- (Optional) Visual Studio or Visual Studio Code

### Running the Application

```bash
# Restore dependencies
dotnet restore

# Test the application
dotnet test

# Run the application
dotnet run --ReadyTech
```

---

## ☕ Features

- `GET /brew-coffee`
  - Returns a 200 OK with a message and current time.
  - Every **5th call** returns `503 Service Unavailable` (coffee machine out of coffee).
  - On **April 1st**, always returns `418 I'm a teapot` (no coffee on April Fool's Day).

