# Use the official .NET Core SDK as a parent image
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

# Copy the rest of the application code
COPY /ApplicationCore/ ./ApplicationCore/
COPY /Infrastructure/ ./Infrastructure/
COPY /Web/ ./Web/
RUN dotnet restore ./Web/
RUN dotnet publish -c Release -o out/Web ./Web/
# Build the runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime

WORKDIR /app

#COPY --from=build /app/Common ./Common
#COPY --from=build /app/ApplicationCore ./ApplicationCore
#COPY --from=build /app/Infrastructure ./Infrastructure
COPY --from=build /app/out/Web .

# Expose the port your application will run on
EXPOSE 80
EXPOSE 443

# Start the application
ENTRYPOINT ["dotnet", "uniceu.dll"]