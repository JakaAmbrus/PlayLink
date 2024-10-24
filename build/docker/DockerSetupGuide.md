# PlayLink Docker Setup Guide

This guide provides instructions on how to set up and run the PlayLink application on your local machine using Docker. The application consists of an ASP.NET backend, an Angular frontend, and a PostgreSQL database.

## Prerequisites

Before you begin, ensure you have the following installed:

- Git
- Docker

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

To get the full experience of the site, you need to add your Cloudinary settings to the docker-compose.dev.yml file.

1. Open docker-compose.dev.yml.
2. Locate the environment variables for the backend service.
3. Fill in your Cloudinary CloudName, ApiKey, and ApiSecret.

```yaml
environment:
  - CLOUDINARY_CLOUD_NAME=your_cloud_name_here
  - CLOUDINARY_API_KEY=your_api_key_here
  - CLOUDINARY_API_SECRET=your_api_secret_here
```

Next, update the Cloudinary URL in the Angular client:

1. Go to the client/src/environments folder.
2. Open the environment.ts file
3. Replace the existing Cloudinary URL with your own.

```ts
export const environment = {
  production: true,
  apiUrl: "api/",
  hubUrl: "hubs/",
  cloudinaryUrl: "https://res.cloudinary.com/xxxx", // Replace with your own Cloudinary URL
};
```

## Running the Application

Run the application using Docker Compose:

```bash
docker-compose -f docker-compose.dev.yml up --build
```

After the command completes, you can access the Angular site at http://localhost:4200.

## Stopping the Application

To stop the application and remove the containers, use the following command:

```bash
docker-compose -f docker-compose.dev.yml down
```

## Show Your Support

If PlayLink has been helpful or interesting to you, I would greatly appreciate your support! Giving a star ⭐️ on GitHub is a fantastic way to show your appreciation and helps more people discover my project. If you are interested, visit my [LinkedIn profile](www.linkedin.com/in/jaka-ambrus) and connect with me there.

Thank you for trying out PlayLink. If you're interested in professional collaboration or if my skills and project align with what you're looking for in a developer, I'm open to discussing potential opportunities. Please feel free to reach out to me for any professional inquiries or collaborations.

## Troubleshooting

If you encounter any issues or have questions while setting up the PlayLink application, don't hesitate to reach out for assistance. You can contact me through GitHub issues, email, or any other preferred method. I'm here to help!

If you encounter any challenges during the setup process, please reach out. This section will be continuously updated with solutions to common issues based on user feedback and inquiries.

## Development Setup Guide

For advanced setup instructions, including running Angular and ASP.NET Core Web API separately, see [DevelopmentSetupGuide.md](./DevelopmentSetupGuide.md).
