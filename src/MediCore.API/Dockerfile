FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . .

RUN apt-get update && apt-get install -y libkrb5-3 netcat-traditional && rm -rf /var/lib/apt/lists/*


RUN dotnet restore src/MediCore.API/MediCore.API.csproj
RUN dotnet publish src/MediCore.API/MediCore.API.csproj -c Release -o /app/publish

# COPY wait-for-db.sh /src/wait-for-db.sh
# RUN chmod +x ./wait-for-db.sh

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT [ "dotnet", "MediCore.API.dll" ]

