# KSU Lacrosse: Data Access Layer

This repository contains the Data Access Layer (DAL) for the Kennesaw State University (KSU) Lacrosse team’s application. The DAL serves as an intermediary between the application and the database, facilitating efficient data retrieval and manipulation.

# Features
- **Entity Definitions**: Represents the application’s data structures.
- **Data Context**: Manages database connections and transactions.
- **Repository Interfaces**: Defines methods for data operations.
- **Repository Implementations**: Provides concrete data access methods.

## Technologies Used
- **Programming Language**: C#
- **Framework**: .NET Framework 4.0
- **ORM**: Entity Framework
- **Database**: SQL Server

## Getting Started
1. Clone the Repository:
	```
	git clone https://github.com/brettdavies/ksulax-Data-Access-Layer.git
	cd ksulax-Data-Access-Layer
	```
2. Open the Project:
   - Open `DAL.csproj` in Visual Studio.
3. Restore NuGet Packages:
   - In Visual Studio, go to `Tools > NuGet Package Manager > Manage NuGet Packages for Solution` and restore the required packages.
4. Configure the Database Connection:
   - Update the connection string in `App.config` to point to your SQL Server instance.
5. Build the Project:
   - Press `Ctrl+Shift+B` to build the project.

## Usage
- **Integration**: Reference this DAL project in your main application to handle data operations.
- **Data Operations**: Utilize the repository methods to perform CRUD operations on the database.

## Contributing
Contributions are welcome. Please fork the repository, create a new branch for your feature or bug fix, and submit a pull request for review.

# 
*Note: Ensure that your development environment is properly set up with the necessary tools and dependencies before running the application.*