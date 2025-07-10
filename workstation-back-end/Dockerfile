# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy the entire source
COPY . ./

# Build the app
RUN dotnet publish -c Release -o out

# Use a runtime image for the final container
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Expose port
EXPOSE 80

# Start the app
ENTRYPOINT ["dotnet", "workstation_back_end.dll"]
