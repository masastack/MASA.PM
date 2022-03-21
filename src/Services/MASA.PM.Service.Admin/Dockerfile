#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/MASA.PM.Service.Admin/MASA.PM.Service.Admin.csproj", "src/Services/MASA.PM.Service.Admin/"]
COPY ["src/Contracts/MASA.PM.Contracts.Environment/MASA.PM.Contracts.Base.csproj", "src/Contracts/MASA.PM.Contracts.Environment/"]
RUN dotnet restore "src/Services/MASA.PM.Service.Admin/MASA.PM.Service.Admin.csproj"
COPY . .
WORKDIR "/src/src/Services/MASA.PM.Service.Admin"
RUN dotnet build "MASA.PM.Service.Admin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MASA.PM.Service.Admin.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MASA.PM.Service.Admin.dll"]