# Entity Framework Core Commands

## Add a New Migration

To create a new migration, use the following command:

```bash
$ dotnet ef migrations add addFilenames --project Image/Persistence/Persistence.csproj --startup-project Image/Presentation.Mvc/Presentation.Mvc.csproj
```