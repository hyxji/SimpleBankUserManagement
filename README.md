# SimpleBankUserManagement

A **C# .NET WPF application** that allows users to search for bank account details using **last name or index**, retrieving their **name, PIN, account number, balance,** and a **bitmap profile picture**. It features a **multi-tier architecture** with a **business logic layer**, **asynchronous processing**, and a **.NET WCF remoting server**. Precaution, some exception handling is not working and some bugs are present including the loading bar.

## Features  
- **Search Functionality** – Retrieve user details via last name or index
- **Multi-Tier Architecture** – Separates database, business logic, and UI
- **Asynchronous Processing** – Uses delegates for non-blocking operations
- **Access Logging** – Tracks searches within the business tier

##  Project Structure  
- **`dbserver`** – Handles remote database communication
- **`dblib`** – Core database library managing user data
- **`dbinterface`** – Defines interfaces for database operations
- **`dbapp`** – WPF application for searching users  
- **`commoncontracts`** – Shared contracts for data exchange  
- **`businesstier`** – Handles logic, access logging, and async processing  
- **`asyncdbapp`** – Enhanced WPF app with async capabilities

##  Future Improvements
- **Improve Exception Handling** – Currently, if a user enters a name that doesn't exist in the database, the app does not handle the error properly. Additionally, searching for the same nonexistent name repeatedly can cause issues, such as the loading bar disappearing after a couple of attempts
- Enhance **UI feedback** – Ensure that the loading bar remains consistent and provides clear error messages when a search fails
- Optimize **database queries** – Improve search performance and error handling for smoother user experience

## Installation & Setup  
1. Clone the repository:  
   ```sh
   git clone https://github.com/yourusername/bank-management-index-app.git
   cd bank-management-index-app
2. Open solution in Visual Studio
3. Restore NuGet packagaes and build the solution
4. Run dbserver and then asyncdbapp
