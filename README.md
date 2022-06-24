# dotnetcore-microservice
The architecture proposes a microservice oriented architecture implementation with multiple autonomous microservices (each one owning its own data/db). The microservices also showcase different approaches from simple CRUD to more elaborate DDD/CQRS patterns. HTTP is the communication protocol between client apps and microservices, and asynchronous message based communication between microservices. Message queues can be handled either with RabbitMQ, to convey integration events.

Domain events are handled in the ordering microservice, by using MediatR, a simple in-process implementation the Mediator pattern.

API Gateways are implemented using Ocelot, an OSS high-performant, production ready, proxy and API Gateway. Currently these API Gateways only perform request forwarding to internal microservices and custom aggregators, giving the clients then experience of a single base URL.
