Welcome, stranger\! 👋

You've stumbled upon the source code for **TextInatorX**, my personal hobby project. This repository showcases a web application designed to extract text from images. While the OCR (Optical Character Recognition) functionality is central, the project's primary focus is on demonstrating a robust and scalable **microservice architecture**. Communication between these services is orchestrated using **Kafka**.

It's important to note that this project was primarily a practical learning exercise for me. My main goals were to gain hands-on experience with Kafka and understand how to integrate OCR capabilities in C#. Therefore, while the architecture is detailed, TextInatorX is not intended as a production-ready example or a blueprint for others, but rather a practical exploration of these technologies.

-----
## Showcase

https://github.com/user-attachments/assets/5c282f0e-1094-46c0-9ba0-67f2ffb57410


## Architecture Overview

TextInatorX is built around three core microservices, seamlessly integrated using **.NET Aspire**:

* **Frontend Service**: The user-facing component.
* **Image Storage Service**: Manages image data and metadata.
* **Image Processing Service**: Handles the heavy lifting of text extraction.

Each microservice leverages **Wolverine**, a powerful mediator framework with built-in Kafka support, for efficient communication.

-----

## Service Deep Dive

### Frontend Service

Built with **ASP.NET MVC** and lightly styled with Bootstrap, the Frontend Service acts as the gateway for user requests, passing them on to other services via Kafka. This service also leverages SignalR for real-time communication with clients.

### Image Storage Service

This **ASP.NET Core** service, also using Wolverine, adheres to a **clean architecture** approach. It employs a repository abstraction for database access and is responsible for storing both the image files themselves and their associated metadata. **Azure Blob Storage** is used for image data, while **SQLite** and **Entity Framework Core** manage the metadata.

### Image Processing Service

Another **ASP.NET Core** service powered by Wolverine, the Image Processing Service also follows a clean architecture. It stores processed text in **MongoDB**, with data access handled via the MongoDB driver and Entity Framework Core. The actual image processing is performed by **Tesseract OCR for .NET**.

-----

## Technology Highlights

### .NET Aspire

**TextInatorX** harnesses the power of **.NET Aspire** for effortless launch and comprehensive observability of its microservices. Each project includes its own telemetry configuration, defining specific observability settings and health checks. Currently, all microservices are configured to run with three replicas each, ensuring high availability.

### Configuration

Every project in TextInatorX utilizes the **IOptions pattern** for configuration and has its own dedicated launch settings file, promoting maintainability and clear separation of concerns.

### Docker

The project relies heavily on Docker, with all necessary images defined in a `docker-compose.yml` file. This includes helpful interfaces like **kafka-ui** and **mongo-express** for simplified debugging.

-----

## Getting Started

To run TextInatorX locally, you'll need the **.NET 9 SDK** and **Docker** installed.

1.  **Start Docker Containers**:
    Begin by spinning up the Docker containers using:

    ```bash
    docker compose up -d
    ```

2.  **Apply Database Migrations**:
    Each service has its own migrations. You'll find the necessary commands for applying them in the root folder of each service's persistence layer. (Note: Migrations are designed to run automatically, but this feature is still undergoing thorough testing.)

3.  **Run the Project**:
    Once migrations are applied, execute the following command to launch the application:

    ```bash
    dotnet run --project TextInatorX.Apphost --launch-profile https
    ```
    
## IMPORTANT
please check if your project contains tessdata folder inside the imageprocessing application layer.
if not run this command:
```bash
git submodule update --init --recursive
```
