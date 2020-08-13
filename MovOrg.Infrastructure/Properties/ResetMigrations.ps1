function ThrowIfError
{
if(!$?)
{
throw "Failed"
}
}

cd $PSScriptRoot
cd ..
dotnet ef database -v update 0
ThrowIfError
dotnet ef migrations -v remove --no-build
dotnet ef migrations -v add InitialCreate
dotnet ef database -v update

