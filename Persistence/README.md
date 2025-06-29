# Entity Framework Core Commands

## Add a New Migration

To create a new migration, use the following command:

```bash
dotnet ef migrations add MigrationName --project Persistence --startup-project Presentation.MVC
```