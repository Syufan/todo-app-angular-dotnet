    # build stage 
    FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
    WORKDIR /src
    
    COPY server/*.csproj server/
    RUN dotnet restore server/server.csproj
    
    COPY server/ server/
    WORKDIR /src/server
    RUN dotnet publish -c Release -o /out /p:UseAppHost=false
    
    # runtime stage
    FROM mcr.microsoft.com/dotnet/aspnet:9.0
    WORKDIR /app
    COPY --from=build /out ./
    
    ENV ASPNETCORE_URLS=http://0.0.0.0:5264
    ENV ASPNETCORE_ENVIRONMENT=Production
    EXPOSE 5264
    
    ENTRYPOINT ["dotnet","server.dll"]