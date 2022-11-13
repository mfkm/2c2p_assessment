# Steps to run the services

1. Clone or download this project
2. Generate database using this SQL script that can be found under SQL_Script folder in the 2C2P.Assessment.DataLayer project in the solution
3. Update the database connection string in AssessmentDbContext.cs to target the database created in (2)
4. Set 2C2P.Assessment.API as startup project and run the API project. You must be able to see the swagger showing all available APIs
5. Set 2C2P.Assessment.WebApp as startup project and run the web application. You can now try to upload the csv/xml files
7. You can navigate back to the swagger in (4) and test the query APIs to see whether the data from the files is successfully processed and imported to the database
