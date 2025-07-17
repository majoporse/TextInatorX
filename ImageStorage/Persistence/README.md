# Entity Framework Core Commands

## Add a New Migration

To create a new migration, use the following command:

```bash
$ dotnet ef migrations add addFilenames --project .\ImageStorage\Persistence\Persistence.csproj --startup-project .\ImageStorage\ImageStorage\ImageStorage.csproj
```
to apply the migration, use:

```bash
dotnet ef database update --project .\ImageStorage\Persistence\Persistence.csproj --startup-project .\ImageStorage\ImageStorage\ImageStorage.csproj
```
