FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/MultiPosting/MultiPosting.Api/MultiPosting.Api.csproj", "src/MultiPosting/MultiPosting.Api/"]
COPY ["src/MultiPosting/MultiPosting.Application/MultiPosting.Application.csproj", "src/MultiPosting/MultiPosting.Application/"]
COPY ["src/MultiPosting/MultiPosting.Infrastructure/MultiPosting.Infrastructure.csproj", "src/MultiPosting/MultiPosting.Infrastructure/"]
RUN dotnet restore "src/MultiPosting/MultiPosting.Api/MultiPosting.Api.csproj"
COPY . .
WORKDIR "/src/src/MultiPosting/MultiPosting.Api"
RUN dotnet build "./MultiPosting.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MultiPosting.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MultiPosting.Api.dll"]
