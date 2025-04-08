# TerraGroupLabs
Site for random stuff

---

## 🔧 Configuration Instructions

### 1. Create the API Config File

Please create the following file:

```
WebAPI/Configs/appsettings.json
```

Add a `"DefaultConnection"` key with a SQL database connection string as the value.

#### Example (for MySQL):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=www.yourdomain.com;Database=DBNAME;Uid=Username;Pwd=Password"
  }
}
```

---

### 2. Edit Razor Server Configuration

Please edit the following file:

```
WebServerRazor/appsettings.json
```

And also:

```
WebServerRazor/appsettings.Development.json
```

*(if you plan on using that)*

Add an `"ApiSettings"` section with a `"BaseUrl"` key pointing to the API.

#### Example:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ApiSettings": {
    "BaseUrl": "http://webapi:8080/api"
  }
}
```

---
