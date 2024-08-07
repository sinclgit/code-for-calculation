using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Sprache;
using Contracts;
using Data;
using AutoMapper;
using Extensions;
using Entities;
using Models.Domain;

namespace Services
{
    

    public class Calculator : ICalculator
    {
        private TransientDataContext _context;
        private readonly IMapper _mapper;
        private static readonly Parser<Expression> Constant =
            Parse.DecimalInvariant
                .Select(n => double.Parse(n, CultureInfo.InvariantCulture))
                .Select(n => Expression.Constant(n, typeof(double)))
                .Token();

        private static readonly Parser<ExpressionType> Operator =
            Parse.Char('+').Return(ExpressionType.Add)
                .Or(Parse.Char('-').Return(ExpressionType.Subtract))
                .Or(Parse.Char('*').Return(ExpressionType.Multiply))
                .Or(Parse.Char('/').Return(ExpressionType.Divide));

        private static readonly Parser<Expression> Operation =
            Parse.ChainOperator(Operator, Constant, Expression.MakeBinary);

        private static readonly Parser<Expression> FullExpression =
            Operation.Or(Constant).End();

        public Calculator(
        TransientDataContext context,
        IMapper mapper)
        {
        _context = context;
        _mapper = mapper;
        }


        /// <summary>
        /// Determine type of expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>Type of expression</returns>
        public string OperationType(string expression, CalculationModel model)
        {
            var combineOperations = new Regex(@"[+*/-]");
            var matches = combineOperations.Matches(expression);
            var calculation = _mapper.Map<Calculation>(model);            


            // If arithmetic operators two or more type - combine
            if (matches.Count > 1)
            {


                // save calc
                _context.Calculations.Add(calculation);
                _context.SaveChanges();
                return "Combine";
            }

            if (expression.Contains('+'))
            {

                // save calc
                _context.Calculations.Add(calculation);
                _context.SaveChanges();
                return "Combine";
            }
            if (expression.Contains('-'))
            {

                // save calc
                _context.Calculations.Add(calculation);
                _context.SaveChanges();
                return "Combine";
            }
            if (expression.Contains('-'))
            {

                // save calc
                _context.Calculations.Add(calculation);
                _context.SaveChanges();
                return "Combine";
            }
            if (expression.Contains('/'))
            {

                // save calc
                _context.Calculations.Add(calculation);
                _context.SaveChanges();
                return "Combine";
            }


            return "Other";
        }

        /// <summary>
        /// Expression string parsing to <see cref="Expression"/> and calculates
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public double Calculate(string expression, CalculationModel model)
        {

            // validate
            if (_context.Calculations.Any(x => x.Id == model.Id))
            throw new AppException("Already exists");

            var operation = FullExpression.Parse(expression);
            var func = Expression.Lambda<Func<double>>(operation).Compile();
            var calculation = _mapper.Map<Calculation>(model);              

                // save calc
                _context.Calculations.Add(calculation);
                _context.SaveChanges();            

            return func();
        }
    }
}