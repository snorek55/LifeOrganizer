cd D:\Programacion\VS\LifeOrganizer\Infrastructure
dotnet ef database -v update 0
dotnet ef migrations -v remove --no-build
dotnet ef migrations -v add InitialCreate
dotnet ef database -v update