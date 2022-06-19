## About

A simple SPA to manipulate basic employee data for demonstration purposes. Some features are:

- Vue.js on frontend
- Dotnet Core Web Api on backend
- EF Core with code-first approach
- PostgreSQL database
- Ability to bulk import from large CSV files


## How To Run

All the system components can be run with docker-compose:

`docker-compose up -d`

After this command the application should be accessible from the address: http://localhost:3000/


If system components are to be run individually some requirements should be satisfied.

- A running PostgreSQL server instance should be accessible with a database named "employee_management"
- The connection string in the backend API's appsettings.json file should be configured
- FILES_PATH environment variable should be set appropriately in order for the backend application to store uploaded files
- .env file in the frontend application's root directory should be configured in accordance with backend api's base address


## Todo:

- Expand unit test coverage
