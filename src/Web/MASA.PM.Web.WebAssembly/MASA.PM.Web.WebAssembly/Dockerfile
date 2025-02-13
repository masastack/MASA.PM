FROM registry.cn-hangzhou.aliyuncs.com/masa/sdk:8.0.203 AS publish
WORKDIR /src
COPY /src/MASA.TSC .
ENV CSPROJ="src/Web/Masa.Tsc.Web.Admin.Wasm/Masa.Tsc.Web.Admin.Wasm.csproj"
RUN dotnet nuget add source http://devops.lonsid.cn:8080/nuget
RUN dotnet restore $CSPROJ && dotnet publish $CSPROJ -c Release -o /app/publish


FROM nginx:latest
WORKDIR /usr/share/nginx/html
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime && \
    rm /usr/share/nginx/html/index.html /usr/share/nginx/html/50x.html
COPY --from=publish /app/publish/wwwroot /usr/share/nginx/html
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["nginx", "-g", "daemon off;"]
