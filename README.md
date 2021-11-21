# Catalog.API
.NET 5 REST API end-to-end with Visual Studio Code

Some Docker commands used:
docker build -t {image_name}:{image_tag} . -> run from the folder, where dockerfile is located
docker images -> lists all currently existing images

For the unit tests we set:
dotnet add reference ..\Catalog.API\Catalog.API.csproj -> reference to the Catalog.API project, which is unit tested
dotnet add package Microsoft.Extensions.Logging.Abstractions
dotnet add package moq -> Mock library - which helps us in the creation of Mocks in the unit tests class
dotnet add package FluentAssertions -> helps us to easier make assertions of the objects in the unit tests