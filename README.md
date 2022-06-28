# Car Pooling Application

[Application hosted on Heroku](https://carpoolptc.herokuapp.com/)

## Technologies

* [ASP .NET 6](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-6.0)
* [Entity Framework Core 6](https://docs.microsoft.com/en-us/ef/core/)
* [React 17](https://reactjs.org/)
* [MediatR](https://github.com/jbogard/MediatR)
* [AutoMapper](https://automapper.org/)
* [FluentValidation](https://fluentvalidation.net/)
* [NUnit](https://nunit.org/), [FluentAssertions](https://fluentassertions.com/), [Moq](https://github.com/moq) & [Respawn](https://github.com/jbogard/Respawn)
* [Docker](https://www.docker.com/)

## Getting Started

The easiest way to get started is to install the [NuGet package](https://www.nuget.org/packages/Clean.Architecture.Solution.Template) and run `dotnet new ca-sln`:

1. Install the latest [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
2. Install the latest [Node.js LTS](https://nodejs.org/en/)
3. Run `git clone https://github.com/RemyCedric/carpool.git` to pull the project
4. Add your cloudinary configuration in your `src/WebUI/appsettings.json` file, should be created
5. Add your sendGrid configuration in your `src/WebUI/appsettings.json` file, should be created
6. Navigate to `src/WebUI/ClientApp` and run `npm install`
7. Navigate to `src/WebUI/ClientApp` and run `npm start` to launch the front end (React)
8. Navigate to `src/WebUI` and run `dotnet run` to launch the back end (ASP.NET Core Web API)

### Docker Configuration

In order to get Docker working, you will need to add a temporary SSL cert and mount a volume to hold that cert.
You can find [Microsoft Docs](https://docs.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-6.0) that describe the steps required for Windows, macOS, and Linux.

For Windows:
The following will need to be executed from your terminal to create a cert
`dotnet dev-certs https --clean`
`dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p Your_password123`
`dotnet dev-certs https --trust`

NOTE: When using PowerShell, replace %USERPROFILE% with $env:USERPROFILE.

FOR macOS:
`dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p Your_password123`
`dotnet dev-certs https --trust`

FOR Linux:
`dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p Your_password123`

In order to build and run the docker containers, execute `docker-compose -f 'docker-compose.yml' up --build` from the root of the solution where you find the docker-compose.yml file.  You can also use "Docker Compose" from Visual Studio for Debugging purposes.
Then open http://localhost:5000 on your browser.

To disable Docker in Visual Studio, right-click on the **docker-compose** file in the **Solution Explorer** and select **Unload Project**.

### Heroku Configuration

In order to push in heroku, add in your heroku `apps` a ressource type of `Heroku Postgres`, this one should be `Attached as DATABASE`

You have to add to buildpacks in specific order, first `heroku/nodejs` and second `https://github.com/jincod/dotnetcore-buildpack`.

You need some `Config Vars` 

* `ASPNETCORE_ENVIRONMENT` set to `Production`
* `JWTTokenKey` at least 32 bits
* `UseInMemoryDatabase` set to `false`
* `UseProduction` set to `true`
* `Cloudinary:ApiKey` filled with your Cloudinary configuration
* `Cloudinary:ApiSecret` filled with your Cloudinary configuration
* `Cloudinary:CloudName` filled with your Cloudinary configuration
* `Sendgrid:User` filled with your Sendgrid configuration
* `Sendgrid:Password` filled with your Sendgrid configuration

### Database Configuration

The template is configured to use an in-memory database by default. This ensures that all users will be able to run the solution without needing to set up additional infrastructure (e.g. PostgreSQL).

If you would like to use SQL Server, you will need to update **WebUI/appsettings.json** as follows:

```json
  "UseInMemoryDatabase": false,
```

Verify that the **DefaultConnection** connection string within **appsettings.json** points to a valid PostgreSQL instance. 

When you run the application the database will be automatically created (if necessary) and the latest migrations will be applied.

### Database Migrations

To use `dotnet-ef` for your migrations please add the following flags to your command (values assume you are executing from repository root)

* `--project src/Infrastructure` (optional if in this folder)
* `--startup-project src/WebUI`
* `--output-dir Persistence/Migrations`

For example, to add a new migration from the root folder:

 `dotnet ef migrations add "SampleMigration" --project src\Infrastructure --startup-project src\WebUI --output-dir Persistence\Migrations`

## Overview

### Domain

This will contain all entities, enums, exceptions, interfaces, types and logic specific to the domain layer.

### Application

This layer contains all application logic. It is dependent on the domain layer, but has no dependencies on any other layer or project. This layer defines interfaces that are implemented by outside layers. For example, if the application need to access a notification service, a new interface would be added to application and an implementation would be created within infrastructure.

### Infrastructure

This layer contains classes for accessing external resources such as file systems, web services, smtp, and so on. These classes should be based on interfaces defined within the application layer.

### WebUI

This layer is a single page application based on React 17 and ASP.NET Core 6. This layer depends on both the Application and Infrastructure layers, however, the dependency on Infrastructure is only to support dependency injection. Therefore only *Startup.cs* should reference Infrastructure.

