Files for the calculation project

These have the in-memory solution only as my destop had issues with the SQL server driver and .net core 8.x as well as other issues with some drive corruption.
A docker file is also added and next steps are to replace the Controller with "[assembly: FunctionsStartup(typeof(AzureFunctionsTodo.Startup))]
" after adding the after including the Microsoft.Azure.Functions.Extensions package--all which need to be included in a new Docker.  This must be used with the Startup to extend to 
Azure:

Startup : FunctionStartup
