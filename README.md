# Universal Payment Terminal

A demo payment terminal built with **Full Stack .NET + React Application** to demonstrate software design patterns and layered architecture.

## Technologies

- C# / ASP.NET Core Web API
- React, TypeScript, Vite (frontend)
  
## Design Patterns

- **Adapter:** Integrates different service providers through `IServicePaymentProvider`.
- **Strategy:** Supports interchangeable payment methods through `IPaymentMethod`.
- **Decorator:** Adds commission behavior using `CommissionPaymentDecorator`.
- **Factory:** Creates decorated payment methods through `PaymentMethodDecoratorFactory`.

## Getting Started

### Prerequisites
- .NET 10 SDK
- Node.js 24+ and npm (for the frontend)

### Run the Backend

```bash
git clone <YOUR_REPOSITORY_URL>
cd UniversalPaymentTerminal
dotnet restore
dotnet build
cd UniversalPaymentTerminal.API
dotnet run
```

Open `/swagger` using the API URL printed in the terminal.

### Run the Frontend

From the frontend directory:

```bash
npm install
npm run dev
```

Configure `VITE_API_BASE_URL` to match the backend URL.

## Demo Data

| Service | Provider ID | Customer ID |
|---|---|---|
| Education | `university-1` | `STU123` |
| Bank | `bank-1` | `LOAN123` |
| Utility | `utility-1` | `UTIL123` |
| Fine | `fine-1` | `FINE123` |

## API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/payment-categories` | List service categories |
| GET | `/api/payment-methods` | List payment methods |
| POST | `/api/service-payments/validate` | Validate a customer |
| POST | `/api/payments` | Simulate a payment |

## Architecture

The solution consists of four clean architecture layers:

- **API:** HTTP endpoints
- **Application:** Business orchestration and contracts
- **Domain:** Entities and enums
- **Infrastructure:** Adapters, payment methods, decorators, and mock services

> **Note:** This is a demonstration project. Payments are simulated; no real transactions are processed.
