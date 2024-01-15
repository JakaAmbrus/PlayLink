# PlayLink Development Setup Guide

This guide is intended for developers who want to set up a local development environment for the PlayLink application. It includes instructions for running and testing the Angular frontend and ASP.NET Core Web API separately.

## Prerequisites

Before you begin, ensure you have the following installed:

- Git
- Node.js and npm
- Angular CLI
- .NET Core SDK
- Docker (for database setup, if needed)

## Cloning the Repository

First, clone the PlayLink repository using the following command:

```bash
git clone git@github.com:JakaAmbrus/PlayLink.git
```

Navigate to the project directory:

```bash
cd PlayLink
```

## Configuring Cloudinary Settings

To get the full experience of the site, enabling photo uploads and showcase you need to add your Cloudinary details.

### In Client Angular:

1. Go to the client/src/environments folder.
2. Open the environment.development.ts file
3. Replace the existing Cloudinary URL with your own.

```ts
export const environment = {
  cloudinaryUrl: "https://res.cloudinary.com/xxxx", //replace xxxx with your own username
};
```

### In Server ASP.NET Web API:

1. Open appsettings.json in the WebAPI layer
2. Locate the Cloudinary settings section
3. Fill in your Cloudinary CloudName, ApiKey, and ApiSecret.

```json
  "CloudinarySettings": {
    "CloudName": "",
    "ApiKey": "",
    "ApiSecret": ""
  },
```

## Running Angular Client Locally

### Installing Node Modules

1. Navigate to the Angular project directory:

```bash
cd PlayLink/client
```

2. Install the necessary npm packages:

```bash
npm install
```

### Running Angular in Development Mode

1. Serve the Angular application on port 4200:

```bash
ng serve --port 4200
```

The Angular application will be accessible at http://localhost:4200.
The port 4200 is chosen because the API allows this port on http or https access.

### Configuring HTTPS for Angular

1. If you wish to test the Angular client over HTTPS, obtain an SSL certificate or generate a self-signed certificate.

2. Configure `angular.json` to use the SSL certificate. Under the "serve" options, add:

```json
   "ssl": true,
   "sslKey": "path/to/ssl.key",
   "sslCert": "path/to/ssl.crt"
```

3. Serve the Angular application with the SSL configuration:

```bash
ng serve --ssl --port 4200
```

The Angular application will be accessible at https://localhost:4200.
The port 4200 is chosen because the API allows this port on http or https access.

## Testing ASP.NET Core Web API Locally

### Setting Up Postgresql In Docker

In order for the API to run as expected, you have to configure the database that it seeds and interacts with on startup.

This guide will walk you through the process of setting up a Docker container running PostgreSQL with a custom connection string. The provided connection string connects to a PostgreSQL server running on the local machine.

#### Step 1: Pull the PostgreSQL Docker Image

To create a PostgreSQL container, you first need to pull the official PostgreSQL Docker image from Docker Hub. Open your terminal or command prompt and run the following command:

```bash
docker pull postgres
```

This command will download the latest PostgreSQL image from Docker Hub.

#### Step 2: Create a Docker Container

Now that you have the PostgreSQL image, you can create a Docker container with a custom connection string. Use this command that matches the appsettings.development.json settings in server/WebAPI layer:

```bash
docker run --name playlink-postgres -e POSTGRES_PASSWORD=postgrespw -e POSTGRES_USER=postgres -e POSTGRES_DB=playlink -p 5432:5432 -d postgres
```

#### Step 3: Verify the Container

To verify that the PostgreSQL container is running, use the following command to list all running containers in docker, or use Docker Desktop to see them.

```bash
docker ps
```

Look for the playlink-postgres container.

### Running ASP.NET Core Web API

Navigate to the ASP.NET Core project directory WebApi layer:

```bash
cd PlayLink/server/WebAPI
```

#### Running in HTTP

To run the ASP.NET Core Web API in HTTP mode, use:

```bash
dotnet run
```

- The API will be accessible over HTTP at http://localhost:5055.
- Use this mode for development and testing without HTTPS.

If you wish to test the angular application alongside the api:

1. Go to the client/src/environments folder.
2. Open the environment.development.ts file
3. Replace the api and hub Urls:

```ts
export const environment = {
  apiUrl: "https://localhost:7074/api/", // Replace with http://localhost:5055
  hubUrl: "https://localhost:7074/hubs/", // Replace with http://localhost:5055
};
```

#### Running in HTTPS

To run the ASP.NET Core Web API in HTTPS mode, you can use a self-signed certificate or obtain a valid SSL certificate from a trusted authority. Here, we assume you have a self-signed certificate (`server.crt` and `server.key`) generated.

Some IDEs like Visual Studio offer https in development without manually configuring it, in that case just run it on the https setting.

Use the following command to run the API in HTTPS mode with the self-signed certificate:

```bash
dotnet run --urls=https://localhost:7074 --certificatePath=server.crt --certificateKeyPath=server.key
```

- The API will be accessible over HTTPS at https://localhost:7074.
- Replace `server.crt` and `server.key` with the actual paths to your SSL certificate and key.

## Show Your Support

If PlayLink has been helpful or interesting to you, I would greatly appreciate your support! Giving a star ⭐️ on GitHub is a fantastic way to show your appreciation and helps more people discover my project. If you are interested, visit my [LinkedIn profile](www.linkedin.com/in/jaka-ambrus) and connect with me there.

Thank you for trying out PlayLink. If you're interested in professional collaboration or if my skills and project align with what you're looking for in a developer, I'm open to discussing potential opportunities. Please feel free to reach out to me for any professional inquiries or collaborations.

## Troubleshooting

If you encounter any issues or have questions while setting up the PlayLink application, don't hesitate to reach out for assistance. You can contact me through GitHub issues, email, or any other preferred method. I'm here to help!

If you encounter any challenges during the setup process, please reach out. This section will be continuously updated with solutions to common issues based on user feedback and inquiries.

## Development Setup Guide

For docker setup instructions, so you can just run the site and see it in action, see [DockerSetupGuide.md](./DockerSetupGuide.md).
