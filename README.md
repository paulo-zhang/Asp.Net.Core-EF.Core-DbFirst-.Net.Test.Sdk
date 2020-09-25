# Asp.Net.Core-EF.Core-DbFirst-.Net.Test.Sdk
An example of Asp.Net core web api using Entity Framework core, database first as well as .Net Test SDK and auto mapping.

This project is built with VS2019

## Requirement
### Technical Requirement: 
AspnetCore EF Core .netStandard and Microsoft.NET.Test.Sdk.
### Business Requirement: 
* An user management web api solution that can add, update, delete and query users to a database.
* The Users are broken in to two types: Managers and Clients.
* Managers will have an additional 'Position' property (Junior, Senior) and Clients will have an int 'Level' property.
* Unit test the api controller.

## Database design
* Since the Managers and Clients share most of their properties, we have a 'base' table Users.
* Both the Managers and Clients table have a foreign key that points to the Users table, these are one-one relationship.
* One manager can manage many clients, while one client only belongs to one manager, therefore Managers and Clients are one-many  relationship.
* We also want to demostrate how to handle the 'base type' scenario in the project, where the Users will be implemented as a base type of *  Managers and Clients.
  
  ![Database table design](/images/db-diagram.PNG)

## Lets code!
### Create UserManagement.UserAPI

#### Create asp.net core web api
![Create asp.net core web api](/images/create-asp.net-core-web-application.PNG)
#### choose location
![choose location](/images/choose-location.PNG)
#### select API project
![select API project](/images/select-API-project.PNG)
#### The new project may look like this:
![project created](/images/project-created.PNG)

### Install Entity Framework
Open Tools -> NuGet Package Manager -> Package Manager Console.

Run the following commands one by one:

``` Package Manager Console Commands
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.EntityFrameworkCore.Tools –Pre
Install-Package Microsoft.EntityFrameworkCore.SqlServer.Design
```
### Create Models
Create a folder Models in the project.

Again, open Tools -> NuGet Package Manager -> Package Manager Console.

Run (You need to change database connection string accordingly)

``` Package Manager Console Commands
Scaffold-DbContext "Server=(local)\SqlExpress;Database=UserManagement;UID=sa;PWD=sa12345;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
```
Now the code has models in it:

![models created](/images/models-created.PNG)

### Config models
Delete the entire method OnConfiguring() in UserManagementContext.cs.

Open Startup.cs, and add this line of code at the end of ConfigureServices():

``` Don't forget to resolve namespace reference using VS2019 code suggestion
services.AddDbContext<UserManagementContext>(item => item.UseSqlServer(Configuration.GetConnectionString("UserManagementConnection")));
```
Open appsettings.json, modify it as:

``` appsettings.json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DomainConnection": "Server=(local)\\SqlExpress;Database=UserManagement;UID=sa;PWD=sa12345;"
  }
}
```
### Create controller
Right click on Controllers folder, 'Add -> Controller...'.

Select 'API Controller with read/write actions'.

Click 'Add'.

Enter 'UserAPIController'.
UserAPIController.cs is created in Controllers.

### Create ViewModels
ViewModels are used to represent the data structure that sent to UI.

With Entity Framework, it's easy to implement a 'Table Per Hierarchy' relationship and 'Table Per Type' relationship:
``` Entity Framework Implementations
https://docs.microsoft.com/en-us/ef/ef6/modeling/designer/inheritance/tph?redirectedfrom=MSDN
https://docs.microsoft.com/en-us/ef/ef6/modeling/designer/inheritance/tpt
``` 
However, this is untrue for Entity Framework Core.
ViewModels are typically used to represent data to view, by doing this, we can tailor-made data structure for view to bind.
Here, it's just some simple viewmodel examples, which look similar to models.
 
### Use AutoMaper
AutoMaper is used to simplify the data convertion between models and viewmodels.
However, you can writing your own mapping logic.


