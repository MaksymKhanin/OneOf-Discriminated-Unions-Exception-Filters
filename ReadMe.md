.Net6 Exception Filters with OneOf Template

Enhanced version of exceptionFilters.


In Controller in Send method in Match pattern you can throw Accepted or OK according to your needs.
Also there can be more response types (errorcodes). It depends on the number of parameters in OneOf
of your service. 
For example, if your service can throw 3 different exceptions, you declare your service method 
with OneOf<Success, Exception1, Exception2, Exception3>. Thus, your Match pattern will have 4 response types:
Accepted, and 3 Exceptions.

In mediator ErrorHandler happens mapping between Exception and dedicated Error. 
There isn`t any easy way how to make such mapping without mediator error handler. 
So if you want this exception type (oneOf + filters) - use mediator.
Otherwise, use exceptionFilters!

In PayloadSenderrorHandler register mapping between exception and errorCode.

In this app is used a simple version of mediator. Without Mediator pipeline behavior and mediator validation.


To add new exception:

1) Add related error code in Core DomainErrors
2) Add new Exception in Application Exceptions
3) throw exception in your handler or service.
4) Declare service method with this exception return type.
5) Do the same with Command and ErrorHandler. And Controller.
6) In Controller add parameter to match operator.

Magic!
