# technical requirement
Node v24.14.0
SDK 10.0.103
Database SQLserver

Backend: C# (.NET 8 / ASP.NET Core) 
Frontend: React - Vite 

# architecture
The system is based on a three-tier client-server architecture, which clearly separates the responsibilities of presentation, business logic, and data management. In the presentation layer, the frontend was developed using React, a JavaScript library that allows for building dynamic and modular user interfaces using reusable components. This layer is responsible for direct user interaction and sending requests to the server via HTTP. In the business logic layer, the backend was implemented with .NET and C#, exposing a REST API responsible for processing requests from the frontend, validating data, and executing necessary system operations. To organize the code and facilitate maintenance, the backend is structured into controllers, which contain the operational logic; models, which represent the system entities; and routes, which define the available API endpoints. Finally, in the data layer, the backend establishes a connection with a cloud-hosted SQL database, which allows the system's information to be stored, queried, and managed centrally, securely, and accessibly from the server, thus ensuring data persistence and availability. 

Server is running on https://localhost:5001 

# Dependence Backend
dotnet add package Microsoft.Data.SqlClient
dotnet add package Swashbuckle.AspNetCore
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.


# Dependence Frontend
npm Init -y
npm install

npm install react-router-dom
npm install react-hook-form
npm install axios  
npm install sweetalert2
npm install react-toastify

# Run Backend

1. cd Backend 
2. dotnet run

# Run Frontend
1. cd Frontend
2. npm run dev