


#Overview
___

## Domain
Contains entities, enums, interfaces and types specfic to the domain layer.


## Application
This layer contains all application logic. It depends on the Domain layer. It defines interfaces that are implemented by other layers.
E.g. the application needs access to the Display of the Terminal, then a new interface would be added and implemented within the infrastructure layer.


## Infrastructure
Contains classes to communicate with external resources such as the Terminal or GRPC server