# DisasterAlleviationFoundation
#Links:
Please access source code here : https://github.com/AaliyahAllie/DisasterAlleviationFoundation.git 
Please access azure repo through the organization 
disasteralleviationwebsitegiftofthegivers-f5f6hcdbchhdgtdu.southafricanorth-01.azurewebsites.net (not functional)
access video functionality here: https://youtu.be/p2ivF5m0AbQ?si=k3RJaWZ0VhheTJKT

APP FUNCTIONALITY:
The purpose of this repository is to hold the Disaster Alleviation Foundation C# Website.

The application holds the following functionalities:
- Runs the database connection to my Azure SQL Database through SSMS
- Allows users to login, register and edit their profile or delete it
- Allows users to volunteer and for volunteers to be assigned roles (tasks to do at incident sites)
- Incident reporting (Incident Name, Serverity type, description and imaging)
- Allows users to donate (type of donations and quanitity)
- Has user friendly design
- About us to describe what the foundation is about
- Dashboards on home to track the amout of volunteers, donations and incidents reported

App flow:
- Home Page (Index):
   -Navigation:
    -Register/Login - Profile edits
    -Volunteer : Register as volunteer
    -Admin login - Manage volunteer and volunteer tasks
    - Donation - submit donation
    - Disaster - report disaster (details)

Explaination of models: 
In this application, models in C# (classes in the Models folder) represent the structure of the data we work with in the code. Each model corresponds to a database table in Azure SQL:

Properties of a model (like DonorName, ResourceType, Quantity in the Donation model) map to columns in the table.

When the application runs, Entity Framework Core uses these models to create, read, update, and delete records in the database.

Changes to the model structure (adding/removing properties) are applied to the database via migrations, keeping the code and database in.
