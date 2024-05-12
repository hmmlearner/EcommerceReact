# EcommerceReact

This is a basic Ecommerce application with backend built on .Net Core 7 and react front end, SQLExpress for DB.
This application uses JWT authentication for customer login and Stripe for payments. 

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
