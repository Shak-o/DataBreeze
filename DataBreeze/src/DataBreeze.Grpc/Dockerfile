FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/DataBreeze.Grpc/DataBreeze.Grpc.csproj", "src/DataBreeze.Grpc/"]
COPY ["src/DataBreeze.Application/DataBreeze.Application.csproj", "src/DataBreeze.Application/"]
COPY ["src/DataBreeze.Domain/DataBreeze.Domain.csproj", "src/DataBreeze.Domain/"]
COPY ["src/DataBreeze.Persistence/DataBreeze.Persistence.csproj", "src/DataBreeze.Persistence/"]
COPY ["src/DataBreeze.Aspire/DataBreeze.Aspire.ServiceDefaults/DataBreeze.Aspire.ServiceDefaults.csproj", "src/DataBreeze.Aspire/DataBreeze.Aspire.ServiceDefaults/"]
RUN dotnet restore "src/DataBreeze.Grpc/DataBreeze.Grpc.csproj"
COPY . .
WORKDIR "/src/src/DataBreeze.Grpc"
RUN dotnet build "DataBreeze.Grpc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataBreeze.Grpc.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataBreeze.Grpc.dll"]
