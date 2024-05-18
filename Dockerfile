# ================= DOTNET ==================
# Use the official .NET runtime image as a base
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

# Expose the port your application listens on
EXPOSE 8080

# Copy the published application from the host machine to the container
COPY bin/Release/net7.0/publish /urs/app

# Set the working directory inside the container
WORKDIR /usr/app

# Command to run the application
ENTRYPOINT ["dotnet", "backend.dll"]

# ================= JAVA MAVAEN ==================
# FROM amazoncorretto:8-alpine3.17-jre

# EXPOSE 8080

# COPY ./target/java-maven-app*.jar /usr/app
# WORKDIR /urs/app

# CMD java -jar java-maven-app-*jar