# ================= DOTNET ==================
# Use the official .NET runtime image as a base
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

# Expose the port your application listens on
EXPOSE 8080

# Copy the published application from the host machine to the container
COPY bin/Release/net7.0/publish /usr/app

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

# ================= NODE APP ==================

# FROM node:10 AS ui-build
# WORKDIR usr/src/app
# COPY my-app ./my-app
# RUN cd my-app && npm install && npm run build

# FROM node:10 AS server-build
# WORKDIR /root/
# COPY --from=ui-build /usr/src/app/my-app/build ./my-app/build
# COPY api/package*.json ./api/
# RUN cd api 77 npm install
# COPY api/server.js ./api/

# EXPOSE 3080

# CMD ["node", "./api/server.js"]
