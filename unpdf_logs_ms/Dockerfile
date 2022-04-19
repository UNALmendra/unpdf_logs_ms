#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["unpdf_logs_ms/unpdf_logs_ms.csproj", "unpdf_logs_ms/"]
RUN dotnet restore "unpdf_logs_ms/unpdf_logs_ms.csproj"
COPY . .
WORKDIR "/src/unpdf_logs_ms"
RUN dotnet build "unpdf_logs_ms.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "unpdf_logs_ms.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "unpdf_logs_ms.dll"]