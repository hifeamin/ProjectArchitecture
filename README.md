# Project Architecture

These are the base layers of my projects which are based on DDD models (anomic model). I use this for my microservices and jobs that do anything.

In my experience of working on different projects, I realized that the Anemic Model is very useful and suitable for microservice architecture in that each service does a specific and simple task. I use this three-layer architecture for each of my microservices.

## Domain-Driven Models

There are two types of implementation of DDD that could be implemented: Anomic Model and Rich Model.

In brief Anemic Model is a Domain Model where Domain Objects contain little or no business logic. Rich Model is a Domain Model where each model has its own business and handles business in its functions.

There are lots of documents that compare these two types. Also, developers usually like one of them and do programming with that. In my experience when you want to create an enterprise application with a complex business and it should be monolithic, you should use DDD as a Rich Model which reduces the complexity of a project. But if you use microservice architecture and each microservice is little and simple, the Anemic Model is better than the Rich model because it has simple and easy to learn and develop.

## Project Layers

### Data Layer

Data Layer is the low-level layer that works with databases. I use the repository pattern to implement this layer. This layer has two Projects (interfaces and implementations). The implementation project implements all interfaces that define in the base project. The key point of this layer is that there is no business. also, it does simple CRUD for each repository.

### Business Layer

#### Domain

Domain Project contains all interfaces of services and models. There is no implementation in this project. This project introduces all of the functionality of the project. At the first of each project, Domain will be defined and then each developer implement the interfaces.

#### Business

This project is the implementation of the Domain layer. It might have some interfaces and services for itself (like validation services, identity service, ...) although they are used for implementing domain services.

### User Interface (UI)

#### Bootstrapper

This part of the project configures dependency injection (DI) to use in UI projects. Each UI might have its services (like its own serializer, logger, ...), this part will define services in DI.

#### API

I like to divide the frontend and backend of projects. It has lots of benefits for scaling and higher availability. So the API project will host APIs of services that are used by client apps like the web page, mobile app, or other services.

#### Client

The SPA pages are here. In this sample, I use Blazor however others could be used. This project calls APIs when it needs. Also, this could be hosted in API by adding the reference to API and some codes.

#### Jobs

Sometimes projects want to do some action periodically. They will be in this part and services will be called by this jobs. They will host as windows service or docker service.

### Test

The test project is placed in this part to write tests.

## Contributors

Special thanks to [Ali Jalali](https://github.com/aliprogrammer69) who help to complete this architecture.

Any idea to be better architecture will be acceptable.

## Licence

Distributed under the MIT License. See [LICENSE](LICENSE.txt) for more information.
