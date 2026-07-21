FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY TaskManagerApi/TaskManagerApi.csproj TaskManagerApi/
RUN dotnet restore TaskManagerApi/TaskManagerApi.csproj

COPY TaskManagerApi/ TaskManagerApi/
WORKDIR /src/TaskManagerApi
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "TaskManagerApi.dll"]