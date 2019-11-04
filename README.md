# Asp.Net.Core-EF.Core-DbFirst-.Net.Test.Sdk
An example of Asp.Net core web api using EntityFrameWork core, database first as well as .Net Test SDK and auto mapping.

## Requirement
### Technical Requirement: 
AspnetCore EF Core .netStandard and Microsoft.NET.Test.Sdk 
### Business Requirement: 
* An user management web api solution that can add, update, delete and query users to a database.
* The Users are broken in to two types: Managers and Clients
* Managers will have an additional 'Position' property (Junior, Senior) and Clients will have an int 'Level' property.
* Unit test the api controller.

## Database design
* Since the Managers and Clients share most of their properties, we have a 'base' tabse Users.
* The Managers and Clients table have a foreign key that points to the Users table, these are one-one relationship.
* One manager can manage many clients, while one client only belongs to one manager, therefore Managers and Clients are one-many  relationship.
*  We also want to demostrate how to handle the 'base type' scenario in the project, where the Users will be implemented as a base type of *  Managers and Clients.
  
  ![Database table design]("/images/db%20diagram.PNG")
