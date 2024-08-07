using Models.Domain;

namespace Contracts
{
    public interface ICalculator
    {
        double Calculate(string expression, CalculationModel model);
        string OperationType(string expression, CalculationModel model);
    }
}