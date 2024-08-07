using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker;
using Entities;
using Contracts;
using Models.Domain;
using Data;
using Extensions;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class CalculationsController : ControllerBase
    {
        private readonly ILogger<CalculationsController> _logger;
        private readonly ICalculationInMemoryRepository _calculationInMemoryRepository;
        private readonly ICalculator _calculator;
        private readonly TransientDataContext _dbContext;
        private readonly IMapper _mapper;

        public CalculationsController(
            ILogger<CalculationsController> logger, ICalculationInMemoryRepository calculationInMemoryRepository, ICalculator calculator, IMapper mapper, TransientDataContext dbContext)
        {
            _logger = logger;
            _calculationInMemoryRepository = calculationInMemoryRepository;
            _calculator = calculator;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Azure Function to Display calculation history
        /// </summary>
        /// <returns>Calculation history</returns>
        [Authorize]
        [Function("History")]
        public async Task<IEnumerable<CalculationModel>> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)])
        {
             _logger.LogInformation("History calculations API call.");
            if (!_calculationInMemoryRepository.GetAll().Result.Any())
            {
                _logger.LogWarning("History is empty.");
                return NotFound();
            }

            var calculation = await _calculationInMemoryRepository.GetAll().Result
                .Select(calculation => _mapper.Map<Calculation, CalculationModel>(calculation))
                .ToList();

            return calculation;
        }

        /// <summary>
        /// Azure Function to get calculation by id number
        /// </summary>
        /// <param name="id">calculation number</param>
        /// <returns></returns>
        [Function("GetById")]
        public async Task<ActionResult<CalculationModel>> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "{id}")] int? id)
        {
            _logger.LogInformation($"Get {id} calculation API call.");

            if (!id.HasValue)
            {
                _logger.LogError("Missing id parameter");
                return BadRequest();
            }

            var calculation = await _calculationInMemoryRepository.FindByIdAsync(id.Value);
            if (calculation is null)
            {
                _logger.LogWarning($"Calculation with {id} doesn't exist.");
                return NotFound();
            }

            return _mapper.Map<CalculationModel>(calculation);
        }
        
        /// <summary>
        /// Azure Function to search calculations on history
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>       
        [Authorize]
        [Function("FindByPredicate")]
        async Task<ActionResult<IEnumerable<CalculationModel>>> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "search/{predicate}")] string predicate)
        {
            _logger.LogInformation($"Attempt to find calculations by {predicate}.");
            var calculations = _calculationInMemoryRepository
                .FindByHistory(c => c.Type.Contains(predicate) || c.Id.ToString().Contains(predicate)
                                    || c.Expression.Contains(predicate) || c.CreateDate.ToString().Contains(predicate)).Result
                .Select(calc => _mapper.Map<Calculation, CalculationModel>(calc))
                .ToList();

            if (calculations.Count == 0)
            {
                _logger.LogWarning($"Calculations with {predicate} don't exist.");
                return NotFound();
            }

            return calculations;
        }

        /// <summary>
        /// Azure Function to create calculation
        /// </summary>
        /// <param name="calculationModel">Calculation dto model</param>
        /// <param name="expression"></param>
        /// <returns>Created at action response - success and Unprocessable entity - model errors</returns>
        [Function("Create")]
        async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] [FromBody] CalculationModel calculationModel, string expression)
        {
            _logger.LogInformation($"Create {calculationModel} calculation API call.");

            var model = calculationModel;

            //calculationModel.CreateDate = DateTime.Now;
            calculationModel.Type = _calculator.OperationType(expression,model);
            calculationModel.Expression = expression;
            calculationModel.Result = _calculator.Calculate(expression,model);

            // Validation check model
            if (!ModelState.IsValid)
            {
                _logger.LogError("Calculation model is not valid.");
                return UnprocessableEntity();
            }

            await _calculationInMemoryRepository.CreateAsync(_mapper.Map<Calculation>(calculationModel));
            return CreatedAtAction(nameof(CalculationsController._calculator), new {id = calculationModel.Id}, calculationModel);
        }

        /// <summary>
        /// Azure Function to update calculation
        /// </summary>
        /// <param name="id">Calculation number</param>
        /// <param name="calculationModel"</param>
        /// <returns>Ok response - success and Bad request - invalid request</returns>
        [Function("Update")]
        async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "{id}")] int id, CalculationModel calculationModel)
        {
            _logger.LogInformation($"Attempt to UPDATE {id} calculation API call.");
            if (id != calculationModel.Id)
            {
                _logger.LogWarning($"Calculation with {id} doesn't exist.");
                return NotFound();
            }

            // Validation check model
            if (!ModelState.IsValid)
            {
                _logger.LogError("Calculation model is not valid.");
                return UnprocessableEntity();
            }

            await _calculationInMemoryRepository.UpdateAsync(_mapper.Map<Calculation>(calculationModel));

            return Ok(calculationModel);
        }

        /// <summary>
        /// Azure Function to update calculation
        /// </summary>
        /// <param name="id">Calculation number</param>
        /// <returns>No content response - success and Bad request - invalid request or Not found - current calculation doesn't exist</returns>
        [Function("Delete")]
        async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "{id}")] int? id)
        {
            _logger.LogInformation($"Attempt to DELETE {id} calculation API call.");

            if (!id.HasValue)
            {
                _logger.LogError("Missing id parameter");
                return BadRequest();
            }
            
            var calculation = await _calculationInMemoryRepository.FindByIdAsync(id.Value);
            if (calculation is null)
            {
                _logger.LogWarning($"Calculation with {id} doesn't exist");
                return NotFound();
            }
            await _calculationInMemoryRepository.DeleteAsync(_mapper.Map<Calculation>(calculation).Id);

            return NoContent();
        }
    }
}