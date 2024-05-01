# EcommerceReact

## Fork the repository
 1. Make sure in solution properties 'Multiple startup projects' are selected

## Setup DB
 In the Package Manager Console run the below commands
 1. Add-Migration InitialCreate -Context DataContext
 2. Update-Database -Context DataContext

## To start the project in Visual Studio
 1. Build the solution
 2. cd ecommercereact.client
 3. npm run dev
