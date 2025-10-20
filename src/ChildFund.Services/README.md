# ChildFund.Services

A .NET 8 library for interacting with the ChildFund API, supporting both synchronous and asynchronous endpoints.

## Features

- 🔄 **Dual Mode Support**: Runtime switching between sync and async API endpoints
- 🔐 **Automatic Authentication**: Token-based authentication with intelligent caching
- 🔁 **Retry Policies**: Built-in exponential backoff for transient failures
- 🚀 **Optimized HTTP**: Connection pooling, HTTP/2, and proper resource management
- ⚙️ **Configurable**: All settings via appsettings.json

## Configuration

### appsettings.json

```json
{
  "ChildFund": {
    "BaseUrl": "https://pubwebapi.childfund.org/api/v1",
    "AsyncBaseUrl": "https://pubwebapi-async.childfund.org/api/v1",
    "UseAsyncEndpoints": false,
    "ApiKey": "User=your-api-key",
    "AuthenticatePath": "Account/Authenticate/",
    "RequestTimeoutSeconds": 30,
    "ConnectionLifetimeMinutes": 5,
    "MaxConnectionsPerServer": 10,
    "ConnectionIdleTimeoutMinutes": 2
  }
}
```

### Configuration Options

| Option | Type | Default | Description |
|--------|------|---------|-------------|
| `BaseUrl` | string | *(required)* | Synchronous API base URL |
| `AsyncBaseUrl` | string | *(required)* | Asynchronous API base URL |
| `UseAsyncEndpoints` | bool | `false` | Enable async endpoints (appends "Async" to method names) |
| `ApiKey` | string | *(required)* | API authentication key |
| `AuthenticatePath` | string | `"Account/Authenticate/"` | Authentication endpoint path |
| `RequestTimeoutSeconds` | int | `30` | HTTP request timeout |
| `ConnectionLifetimeMinutes` | int | `5` | Connection pool lifetime |
| `MaxConnectionsPerServer` | int | `10` | Maximum concurrent connections per server |
| `ConnectionIdleTimeoutMinutes` | int | `2` | Idle connection timeout |

## Usage

### Registration (Startup.cs / Program.cs)

```csharp
services.AddChildFundServices(configuration);
```

### Dependency Injection

```csharp
public class MyService
{
    private readonly IChildInventoryClient _childInventoryClient;
    private readonly IDonorPortalClient _donorPortalClient;
    
    public MyService(
        IChildInventoryClient childInventoryClient,
        IDonorPortalClient donorPortalClient)
    {
        _childInventoryClient = childInventoryClient;
        _donorPortalClient = donorPortalClient;
    }
    
    public async Task GetDataAsync()
    {
        // Client automatically uses the correct endpoint based on configuration
        var children = await _childInventoryClient.GetRandomKidsForWebAsync();
        var countries = await _childInventoryClient.GetAllCountriesAsync();
    }
}
```

## Sync vs Async Endpoints

### How It Works

When `UseAsyncEndpoints` is set to `true`:

1. The library uses `AsyncBaseUrl` instead of `BaseUrl`
2. Method names automatically get "Async" appended

**Example:**
```
Method call: GetRandomKidsForWeb
UseAsyncEndpoints = false → ChildInventory/GetRandomKidsForWeb
UseAsyncEndpoints = true  → ChildInventory/GetRandomKidsForWebAsync
```

### Switching at Runtime

The endpoint mode is determined at application startup based on the configuration. To switch modes:

1. Update `UseAsyncEndpoints` in appsettings.json
2. Restart the application

### Migration Path

```json
// Phase 1: Start with sync endpoints until the Async servers are stable
{
  "ChildFund": {
    "UseAsyncEndpoints": false,
    "BaseUrl": "https://pubwebapi.childfund.org/api/v1",
    "AsyncBaseUrl": "https://pubwebapi-async.childfund.org/api/v1"
  }
}

// Phase 2: Test async endpoints
{
  "ChildFund": {
    "UseAsyncEndpoints": true,
    "BaseUrl": "https://pubwebapi.childfund.org/api/v1",
    "AsyncBaseUrl": "https://pubwebapi-async.childfund.org/api/v1"
  }
}

// Phase 3: Roll out to production (just flip the flag)
```

## HTTP Configuration Details

### Connection Pooling
- Connections are reused for up to 5 minutes (configurable)
- Idle connections are disposed after 2 minutes (configurable)
- Maximum 10 connections per server (configurable)

### Retry Policy
- 3 retry attempts with exponential backoff (200ms, 400ms, 800ms)
- Retries on: Network failures, 5xx errors, 429 (rate limiting)

### Security
- TLS 1.2 and TLS 1.3 support
- Bearer token authentication with automatic caching
- Token refresh with clock skew handling (2-minute safety margin)

### Performance
- HTTP/2 support with multiple connections
- Automatic GZip/Deflate decompression
- Disabled Nagle algorithm for reduced latency
- Disabled Expect100Continue for faster requests