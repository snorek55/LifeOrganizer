cd D:\Programacion\VS\LifeOrganizer\Infrastructure
dotnet ef database update 0
dotnet ef migrations remove --no-build
dotnet ef migrations add InitialCreate
dotnet ef database update --no-build