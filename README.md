
# 2023 Capstone Project - Web Application

## Overview

The 2023 Capstone Project is a web-based application that utilizes both React and Blazor for the frontend and follows the Clean Architecture pattern for the backend. The project aims to provide a seamless user experience with real-time communication, web crawling, and database management.

## Frontend - React

### Technologies Used

- **Axios**: Axios is utilized for API communications, enabling seamless data exchange between the frontend and backend.

- **Material UI 5 and Semantic UI Library**: The components and pages are constructed using Material UI 5 and Semantic UI libraries, providing a modern and visually appealing user interface.

- **Formik and Yup**: Formik and Yup are used for form control and validation, ensuring data integrity and error handling in user input.

### Features

- **User Registration by Email Confirmation**: Users can register using their email address and confirm their registration via email.

- **Login Functionality**: Registered users can log in securely to access the application's features.

- **Login with Google**: Users have the option to log in using their Google accounts for streamlined authentication.

- **Order Initiation**: Users can initiate and place orders conveniently through the application.

- **Webpage Crawling**: The application includes a web crawler that extracts data from targeted web pages.

- **Live Logs for Order Events and Products over SignalR**: SignalR is employed to provide real-time logs for order events and product updates, allowing users to monitor the status of orders and products as they happen.

- **Pages for User, Orders, OrderEvents, and Products**: User-friendly pages are available for managing users, orders, order events, and products, providing essential functionalities for easy data management.

- **Real-Time Notifications**: Users receive real-time notifications upon order completion and crawler task completion, keeping them informed of significant updates.

- **Saved Notifications**: Users can access and view saved notifications for future reference and review.

## Frontend - Blazor

The Blazor part is can be found UpSchool-FullStack-Development-Bootcamp/FinalProject/ repository link.


## Backend - Clean Architecture

### CQRS Design Pattern

The backend utilizes the CQRS (Command Query Responsibility Segregation) design pattern to optimize scalability and performance. By segregating command and query operations, the application efficiently handles data processing tasks.

### BackgroundService

The backend includes a BackgroundService component, responsible for executing tasks in the background. This enables the application to perform various automated and scheduled operations.

### SignalR Library

The SignalR library is utilized for real-time, bi-directional communication between the backend and frontend. It facilitates the transmission of live order status and logs to connected clients, enabling instant updates and monitoring.

### Selenium Framework for Web Crawler

To enable web crawling functionality, the backend incorporates the Selenium Framework. Selenium provides a powerful toolset for automating web browsers, allowing the application to extract data from targeted web pages.

### Entity Framework (EF) ( .Net EntityFramework Core 7)

Entity Framework (EF) with .NET EntityFramework Core 7 is the chosen ORM (Object-Relational Mapping) tool for database access and management. It simplifies interactions with the database and enables seamless integration with the Clean Architecture layers.

### MySQL MariaDB for Database

The backend utilizes the MySQL MariaDB database management system. MariaDB is chosen for its performance, reliability, and open-source nature, making it an excellent fit for this project's requirements.

---

Please note that the above description is an outline of the frontend and backend technologies for the 2023 Capstone Project. You can further elaborate on each section with implementation details, code examples, and any other relevant information in your actual README.md file.
