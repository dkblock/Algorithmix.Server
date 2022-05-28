```json
{
  "ConnectionStrings": {
    "DefaultConnection": "__DB_CONNECTION__"
  },

  "ClientUrl": "http://localhost:8080",

  "Mail": {
    "Address": "kb.algorithmix@yandex.ru",
    "Password": "__MAIL_PASSWORD__",
    "Host": "smtp.yandex.ru",
    "Port": 587
  },

  "Identity": {
    "Secret": "__SECRET_KEY__",
    "AccessTokenLifetimeInMinutes": 20,
    "RefreshTokenLifetimeInDays":  30
  },

  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  }
}
```