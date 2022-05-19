```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "Database": {
    "Server": "SERVER",
    "Port": 1433,
    "DatabaseName": "DATABASE_NAME",
    "Username": "SA",
    "Password": "PASSWORD"
  },
  "Identity": {
    "Secret": "secret-key",
    "AccessTokenLifetimeInMinutes": 20,
    "RefreshTokenLifetimeInDays":  30
  },
  "ClientUrl": "CLIENT_URL",
  "Email": {
    "Address": "MAIL_ADDRESS",
    "Password": "MAIL_PASSWORD",
    "Host": "MAIL_HOST",
    "Port": 587
  }
}
```