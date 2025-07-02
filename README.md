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
dotnet run --project ReadyTech
```

---

## ☕ Features

### GET /brew-coffee

- Returns a `200 OK` with:
  - A status message based on current temperature.
  - The current UTC time.

- Every **5th call** returns:
  - `503 Service Unavailable` — simulating the machine running out of coffee.

- On **April 1st**, always returns:
  - `418 I'm a teapot` — no coffee on April Fool’s Day.

- Includes temperature-based messages using [OpenWeatherMap](https://openweathermap.org/current):
  - Above 30°C → "Your refreshing iced coffee is ready"
  - 30°C or below → "Your piping hot coffee is ready"

---

## 🌦️ Weather Integration (OpenWeatherMap)

The coffee message depends on real-time temperature in Melbourne, fetched from the OpenWeatherMap API.

### 🔑 Setting up your API Key

To use the weather feature, you need an OpenWeatherMap API key. You can set it in either of the following ways:

#### Option 1: `appsettings.json`

```json
{
  "WeatherApi": {
    "ApiKey": "your-api-key-here"
  }
}
```

#### Option 2: Secret Manager (recommended for local development)

```bash
dotnet user-secrets init
dotnet user-secrets set "WeatherApi:ApiKey" "your-api-key-here"
```

> 💡 Your key will be automatically injected via `IConfiguration`.

---

## 📚 Swagger UI

Once the app is running, access interactive API docs at:

```
http://localhost:5153/swagger/index.html
```

