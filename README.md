Files for the calculation project

These have the in-memory solution only as my destop had issues with the SQL server driver and .net core 8.x as well as other issues with some drive corruption.
A docker file is also added and next steps are to replace the Controller with "[assembly: FunctionsStartup(typeof(AzureFunctionsTodo.Startup))]
" after adding the after including the Microsoft.Azure.Functions.Extensions package--all which need to be included in a new Docker.  This must be used with the Startup to extend to 
Azure:

Startup : FunctionStartup

**Additional changes for Controller added as example**
An example added from the contoller showing change of a "post" to an Azure function:
**From:**
        /// Query to create calculation
        /// </summary>
        /// <param name="calculationModel">Calculation dto model</param>
        /// <param name="expression"></param>
        /// <returns>Created at action response - success and Unprocessable entity - model errors</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CalculationModel calculationModel, string expression)
**To:**
/// <summary>
        /// Azure Function to search calculations on history
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>       
        [Authorize]
        [FunctionName("FindByPredicate")]
        async Task<IActionResult<IEnumerable<CalculationModel>>> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "search/{predicate}")], string predicate)
        {
