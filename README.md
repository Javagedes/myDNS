This project simulates a simplified DNS as a webapp

This project uses React (Typescript) for the front end
This project uses C# (.NET) and the Entity Framework Core to connect to a SQLite Database

The project is split up into 2 main parts:
1. C:\..\myDNS\client-app - This is the front end, built with React + typescript
2. C:\..\myDNS\API - This is the back end, build with C# (.NET). including the API Calls located in ../API/Controllers/editRecordController.cs

The SQLite database is located at C:\..\myDNS\API\dns.db
You can view the database however you like. I personally use the SQLite extension for VSCode. You can press "ctrl+shift+p" and search SQLite and click on the open database option.

This web-app currently has 3 different functionalities:
1. Retrieve a DNS record from the database
2. Insert a DNS record to the database
3. Delete a DNS record from the server.

To start the project:
1. download or pull the code.
2. from C:\..\myDNS\ folder, type the command "dotnet run -p API/ to start the backend
3. from C:\..\myDNS\client-app\ folder, type "npm start" to start the front end