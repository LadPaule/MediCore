# Todo 1: Build the .NET App
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . .


WORKDIR "/src/src/MediCore.BlazorUI"

# Todo: print the files in the logs in case of a failure.

RUN ls -la

# Todo: Restore dependencies
RUN dotnet restore "MediCore.BlazorUI.csproj"

# Todo: 5. Publish the app
RUN dotnet publish "MediCore.BlazorUI.csproj" -c Release -o /app/publish /p:UseAppHost=false /p:BuildingInsideContainer=true

#Todo: STAGE 2: Final Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .

#Todo: Standardize on port 8080
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "MediCore.BlazorUI.dll"]