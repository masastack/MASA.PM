FROM registry.cn-hangzhou.aliyuncs.com/masa/dotnet_sdk:8.0 AS publish
WORKDIR /src
COPY . .
ENV CSPROJ="src/Services/MASA.PM.Service.Admin/MASA.PM.Service.Admin.csproj"
RUN dotnet restore $CSPROJ && dotnet publish $CSPROJ -c Release -o /app/publish

FROM registry.cn-hangzhou.aliyuncs.com/masa/dotnet_aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://0.0.0.0:80
EXPOSE 80
ENTRYPOINT ["dotnet","MASA.PM.Service.Admin.dll"]