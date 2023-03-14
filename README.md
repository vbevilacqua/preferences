# Preferences



## Usage

With [docker](https://docker.com/) installed, run to build:

    $ docker-compose up

## Running swagger

Decided to use Swagger for easier documentation and to allow us to call the api easier.

Access the webpage from any browser:

```
    https://localhost:7122/swagger/
```

All documentation necessary for the endpoints should be in the swagger page.

![alt text](https://github.com/vbevilacqua/preferences/blob/main/assets/swagger.png?raw=true)

## Using PgAdmin

URL: http://localhost:5050/browser/

Credentials:
- User: admin@admin.com
- Password: root
  Architecture


## Clean Architecture
The architecture is simple, the client sends a message to controllers and the controllers uses a Postgre database to persist data.

![alt text](https://github.com/vbevilacqua/preferences/blob/main/assets/cleanArchitecture.jpg?raw=true)


### Domain

This will contain all entities, enums, exceptions, interfaces, types and logic specific to the domain layer.

### Application

This layer contains all application logic. It is dependent on the domain layer, and has no dependencies on any other layer or project. This layer defines interfaces that are implemented by outside layers. For example, if the user wants a new permission, a new interface would be added to application and an implementation would be created within infrastructure.

### Infrastructure

This layer contains classes for accessing external resources such as file systems, web services, smtp, and so on. These classes should be based on interfaces defined within the application layer.

### webapi

This layer is a webapi that allow clients to call rest endpoints. This layer depends on both the Application and Infrastructure layers, however, the dependency on Infrastructure is only to support dependency injection. Therefore only Startup.cs should reference Infrastructure.





## Next Steps
- Add frontend 
- Introduce better logging 
- Move db connection string to be an environment variable
- Improve integration tests