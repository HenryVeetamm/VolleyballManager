FROM mcr.microsoft.com/dotnet/sdk:latest AS build
WORKDIR /app

COPY *.sln .
COPY *.props .

#Base
COPY Base.BLL/*.csproj ./Base.BLL/
COPY Base.DAL.EF/*.csproj ./Base.DAL.EF/
COPY Base.Domain/*.csproj ./Base.Domain/
COPY Base.Extension/*.csproj ./Base.Extension/

#Base.Contracts
COPY Base.Contracts/Base.Contracts.BLL/*.csproj ./Base.Contracts/Base.Contracts.BLL/
COPY Base.Contracts/Base.Contracts.DAL/*.csproj ./Base.Contracts/Base.Contracts.DAL/
COPY Base.Contracts/Base.Contracts.Domain/*.csproj ./Base.Contracts/Base.Contracts.Domain/
COPY Base.Contracts/Base.Resources/*.csproj ./Base.Contracts/Base.Resources/

#App
COPY App.BLL/*.csproj ./App.BLL/
COPY App.BLL.Contracts/*.csproj ./App.BLL.Contracts/
COPY App.BLL.DTO/*.csproj ./App.BLL.DTO/
COPY App.DAL.Contracts/*.csproj ./App.DAL.Contracts/
COPY App.DAL.DTO/*.csproj ./App.DAL.DTO/
COPY App.DAL.EF/*.csproj ./App.DAL.EF/
COPY App.Domain/*.csproj ./App.Domain/
COPY App.Resources/*.csproj ./App.Resources/
COPY PublicAPI.DTO.v1/*.csproj ./PublicAPI.DTO.v1/
COPY WebApp/*.csproj ./WebApp/

#Tests
COPY WebApp.Tests/*.csproj ./WebApp.Tests/

RUN dotnet restore

#Base
COPY Base.BLL/. ./Base.BLL/
COPY Base.DAL.EF/. ./Base.DAL.EF/
COPY Base.Domain/. ./Base.Domain/
COPY Base.Extension/. ./Base.Extension/

#Base.Contracts
COPY Base.Contracts/Base.Contracts.BLL/. ./Base.Contracts/Base.Contracts.BLL/
COPY Base.Contracts/Base.Contracts.DAL/. ./Base.Contracts/Base.Contracts.DAL/
COPY Base.Contracts/Base.Contracts.Domain/. ./Base.Contracts/Base.Contracts.Domain/
COPY Base.Contracts/Base.Resources ./Base.Contracts/Base.Resources/

#App
COPY App.BLL/. ./App.BLL/
COPY App.BLL.Contracts/. ./App.BLL.Contracts/
COPY App.BLL.DTO/. ./App.BLL.DTO/
COPY App.DAL.Contracts/. ./App.DAL.Contracts/
COPY App.DAL.DTO/. ./App.DAL.DTO/
COPY App.DAL.EF/. ./App.DAL.EF/
COPY App.Domain/. ./App.Domain/
COPY App.Resources/. ./App.Resources/
COPY PublicAPI.DTO.v1/. ./PublicAPI.DTO.v1/
COPY WebApp/. ./WebApp/

#Tests
COPY WebApp.Tests/. ./WebApp.Tests/

WORKDIR /app/WebApp

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:latest as runtime
WORKDIR /app
ENV ConnectionStrings:AzureConnection="Server=volleyballdatabase.postgres.database.azure.com;Database=volleyballdb;Port=5432;User Id=postgres@volleyballdatabase;Password=Server,login1;"
COPY --from=build /app/WebApp/out ./

ENTRYPOINT ["dotnet", "WebApp.dll"]

