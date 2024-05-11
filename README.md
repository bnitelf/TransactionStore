# TransactionStore
This project demonstates how to handle file upload in csv and xml file formats then save (upsert) to DB.
Also the project contains both web page and web api. Web page is for upload transaction files. And Web API for query the uploaded data.

## Tools used
- Visual Studio Community 2022
- SQL Server Express 2022 (local)
- SSMS 20.1
- Postman

## Design Pattern used
- Factory Pattern
- Strategy Pattern
- Validator Pattern
- For Repository Pattern, I decided to used EF Core as it already acted as Repository pattern itself.
- Dependency Injection
- MVC
