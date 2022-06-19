# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY EmployeeManagement.Backend.API/*.csproj ./EmployeeManagement.Backend.API/
COPY EmployeeManagement.Backend.Business/*.csproj ./EmployeeManagement.Backend.Business/
COPY EmployeeManagement.Backend.DatabaseContext/*.csproj ./EmployeeManagement.Backend.DatabaseContext/
COPY EmployeeManagement.Backend.Interfaces/*.csproj ./EmployeeManagement.Backend.Interfaces/
COPY EmployeeManagement.Backend.Model.Entity/*.csproj ./EmployeeManagement.Backend.Model.Entity/
COPY EmployeeManagement.Backend.Model.View/*.csproj ./EmployeeManagement.Backend.Model.View/

RUN dotnet restore

# copy everything else and build app
COPY EmployeeManagement.Backend.API/. ./EmployeeManagement.Backend.API/
COPY EmployeeManagement.Backend.Business/. ./EmployeeManagement.Backend.Business/
COPY EmployeeManagement.Backend.DatabaseContext/. ./EmployeeManagement.Backend.DatabaseContext/
COPY EmployeeManagement.Backend.Interfaces/. ./EmployeeManagement.Backend.Interfaces/
COPY EmployeeManagement.Backend.Model.Entity/. ./EmployeeManagement.Backend.Model.Entity/
COPY EmployeeManagement.Backend.Model.View/. ./EmployeeManagement.Backend.Model.View/

WORKDIR /source/EmployeeManagement.Backend.API
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "EmployeeManagement.Backend.API.dll"]
