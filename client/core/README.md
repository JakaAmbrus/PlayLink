# PlayLink - Angular Client

This project is a showcase of my creative application and ongoing learning in Angular and Frontend development in general. More updates and improvements are planned in terms of features and readability, alongside thorough testing to enhance functionality and user experience.

<!--
## Client Technical Insight Video

Check out my [Client Technical Insight Video](incoming potentially)!

--- -->

## Tech Stack

![HTML5](https://img.shields.io/badge/-HTML5-E34F26?style=flat&logo=html5&logoColor=white)
![CSS3](https://img.shields.io/badge/-CSS3-1572B6?style=flat&logo=css3&logoColor=white)
![SCSS](https://img.shields.io/badge/-SCSS-CC6699?style=flat&logo=sass&logoColor=white)
![JavaScript](https://img.shields.io/badge/-JavaScript-yellow?style=flat&logo=javascript&logoColor=white)
![TypeScript](https://img.shields.io/badge/-TypeScript-007ACC?style=flat&logo=typescript&logoColor=white)
![Angular](https://img.shields.io/badge/-Angular-DD0031?style=flat&logo=angular&logoColor=white)
![Angular Material](https://img.shields.io/badge/-Angular%20Material-0081CB?style=flat&logo=angular&logoColor=white)
![RxJS](https://img.shields.io/badge/-RxJS-B7178C?style=flat&logo=reactivex&logoColor=white)
![Figma](https://img.shields.io/badge/-Figma-F24E1E?style=flat&logo=figma&logoColor=white)
![NGINX](https://img.shields.io/badge/-NGINX-009639?style=flat&logo=nginx&logoColor=white)

## Development Setup Guide

For a comprehensive guide on setting up the client development environment, refer to: [DevelopmentSetupGuide.md](../DevelopmentSetupGuide.md).

## Key Features

- **UI/UX Design**: Independently crafted in Figma for a user-friendly interface and engaging user experience.
- **Responsive Design**: Ensures smooth operation across various devices, from desktops to mobiles and tablets.
- **Reactive Forms**: Advanced form handling with custom validations for a robust and interactive user experience.
- **Security**: Implements token-based authentication and custom guards to maintain high-security standards.
- **Error Handling**: Features global error management with custom notifications, tailored to interact with backend design.
- **Websockets** for real-time communication between the client and server.
- **Caching Strategy**: Utilizes centralized caching with thoughtful invalidation, minimizing server load while ensuring data freshness.
- **Image Optimization**: Integrates Cloudinary CDN for efficient image delivery, coupled with Angular 17's NgSrc for improved performance.
- **Animations**: Includes Angular route and custom component animations, enhancing the visual appeal and user interaction.
- **Reactive Programming**: Employs RxJS and Angular Signals for dynamic data handling and responsive UI updates.
- **Performance**: Uses OnPush change detection strategy for optimized application performance.
- **User Feedback**: Custom UI indicators for loading and errors, providing clear and intuitive user interaction.
- **Lazy Loading**: Implements lazy loading with preloading strategies to enhance application speed and efficiency.
- **NGINX Deployment**: The live site is served via NGINX, ensuring optimal performance and reliable access, with a development configuration included for reference, but the nginx.dev.conf is just a simple configuration for development purposes, the live site uses a more complex configuration with better security and performance practices.

## Architecture

- `core/`: Core services and singleton components.
- `features/`: Feature modules(now standalone) encapsulating specific functionality.
- `shared/`: Shared components, directives, and pipes used across the application.

This structure reflects my approach to modular and maintainable code organization, originally planned with the modules in mind, but later transitioned to standalone components after the release of Angular 17 in the middle of development, I kept the organization due to liking the file structure and the ability to easily transition back to modules if needed.

## Performance and Best Practices

In developing this project, special attention was dedicated to learning and trying to keep performance and best practices in mind, particularly concerning the initial page load and SEO on that page.

The following image displays the Lighthouse desktop score for the [live hosted site](https://ambrusocialmedia.com) initial page which you can visit and test yourselves.

![Lighthouse Desktop Score](https://res.cloudinary.com/dsdleukb7/image/upload/v1705175792/playlink-images/Screenshot_2024-01-13_215737_vcqw49.jpg)

_Note: This Lighthouse score reflects the site's status as of the README's last update._

## Gaming Section

When I started my developer journey while still in University, I created a few HTML, CSS and Javascript games. While making them months before I started this project I still had in mind that I would like to eventually create my own Social media site with a gaming section. While developing these now simple games, it made me decide to try to switch from Electrical Engineering career path into development.

Here are the original repositories of each of the games in the gaming section:

- [Hollow X Hollow](https://github.com/JakaAmbrus/Hollow_x_Hollow)
- [PlaySketchPortable](https://github.com/JakaAmbrus/PlaySketch_Portable)
- [Rock Paper Scissors](https://github.com/JakaAmbrus/Rock_Paper_Scissors)

## Testing

Currently, this project is in the process of integrating testing and further code cleanup. A dedicated testing folder has been set up. While the implementation of tests is still underway, .spec files have been included in the project structure as part of the groundwork for future usage.
