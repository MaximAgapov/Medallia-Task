# Medallia-Task

## Task related info

Main service for calculation the task's requirement is in the OrderService: `.\src\Application\ShopItems\Services\OrderService.cs`

There are two sets of the test :
* Unit tests for coverage OrderService logic (`Application.UnitTests`).
* Integration tests for coverage Commands and Query functionality (`Application.IntegrationTests`)

I implemet that solution in the way that there are few special offers for the item. And the system should calculate the most optimal total for the customer.

Initional DB Seed are in the `.\src\Infrastructure\Persistence\ApplicationDbContextSeed.cs` file.

## Api

* [POST] ​/api​/Order​/StartNew - Closes old Orders and open new one.

* [POST] ​/api​/Order​/AddItemToOrder - Adding the new ShopItem to the order by name.

* [GET] ​/api​/Order​/GetTotal - Calculates and returns total price of the order.


## Architecture overview

For that project used CleanArchitecture .Net template. That’s template is based on the CQRS pattern.
I used MediatR NuGet package for implementing CQRS. This is a simple, unambitious mediator implementation in .NET.
Widely used with more than 10m downloads.

There are some props in that approach :
* Distributed request handling makes it relatively easy to replace some parts with separate microservices;
* Allow using Behaviors in handling pipelines with All Requests (same approach as Mediators). That allows splitting totally different domain login within one pipeline (ex, alarms, logging and auth)
* Possibility to split Read and write access to the DB. Because of tons of events coming from devices I expect that's a very critical possibility for the production life of the application.
* 
and cons as well:
* Slightly over-engineering at the beginning steps, but should perfectly fit for latest tasks.
* Slightly hidden request flow for first code investigation for persons who are not familiar with the library.
* Too much boilerplate for simply CRUD operation.

## Run
There are few ways to run the application: Run in the container and local run from .Net IDE.

### IDE
By Default application uses in memory DB.
Just run WebUI solution and navigate to `https://localhost:5001/api/`


### Docker
For run application in the containers requires specify local dev certificate ( TODO: improve for ONLY HTTP more run on local even via `docker-compose` command) 
Generate dev certificate if needed:

**MacOS, Linux**
 > dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p { password here }
 
> dotnet dev-certs https --trust

In the preceding commands, replace `{ password here }` with a password.

**Windows**

When using PowerShell, replace %USERPROFILE% with $env:USERPROFILE.
 
 > dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p { password here }

> dotnet dev-certs https --trust
 
In the preceding commands, replace `{ password here }` with a password.

Next, for run application needs to provide cert password generated or exists.
That can be done with editing docker-compose file or set it via run command.

**Option 1. docker-conpose edit**

Update lines:
* *ASPNETCORE_Kestrel__Certificates__Default__Path* ith path to cert.
* *ASPNETCORE_Kestrel__Certificates__Default__Password* with cert password 

Than just run app with next command:
> docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d

**Option 2. Specify cert date in one command**
Replace path to certificate and certificate password in the `docker-compose` command: 

>docker-compose -f docker-compose.yml -f docker-compose.override.yml -e ASPNETCORE_Kestrel__Certificates__Default__Password="password" -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx  up -d


Navigate to `https://localhost:5001/api` for display Swagger API page. 

# API

Swagger page available on `https://localhost:5001/api` There are some description per each method.
