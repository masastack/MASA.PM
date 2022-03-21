#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/UI/MASA.PM.UI.Admin/MASA.PM.UI.Admin.csproj", "src/UI/MASA.PM.UI.Admin/"]
RUN dotnet restore "src/UI/MASA.PM.UI.Admin/MASA.PM.UI.Admin.csproj"
COPY . .
WORKDIR "/src/src/UI/MASA.PM.UI.Admin"
RUN dotnet build "MASA.PM.UI.Admin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MASA.PM.UI.Admin.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MASA.PM.UI.Admin.dll"]