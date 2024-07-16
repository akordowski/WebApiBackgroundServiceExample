# WebApi Background Service Example

As the `ExecuteAsync` method of the `BackgroundService` can block the startup of the host the preferred solution is `Solution1`.

Sources:  
https://blog.stephencleary.com/2020/05/backgroundservice-gotcha-startup.html  
https://github.com/dotnet/runtime/issues/36063
