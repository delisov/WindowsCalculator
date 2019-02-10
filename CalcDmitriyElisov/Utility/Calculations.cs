using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalcDmitriyElisov.Models;

namespace CalcDmitriyElisov.Utility
{
    public class Calculations
    {

        public static double CalculateResultDoubleOp(double firstOperand, double secondOperand, string operation)
        {
            try
            {
                if (operation == "")
                {
                    return firstOperand;
                }
                else if (operation == CalcOperation.PlusOp.Name)
                {
                    return firstOperand + secondOperand;
                }
                else if (operation == CalcOperation.MinusOp.Name)
                {
                    return firstOperand - secondOperand;
                }
                else if (operation == CalcOperation.MultOp.Name)
                {
                    return firstOperand * secondOperand;
                }
                else if (operation == CalcOperation.DivOp.Name)
                {
                    if (firstOperand == 0)
                        throw new Exception("Деление на ноль невозможно");
                    return firstOperand / secondOperand;
                }
                else
                {
                    throw new Exception("Неизвестная операция: " + operation);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static double CalculateResultSingularOp(double operand, string operation)
        {
            try
            {
                if (operation == CalcOperation.SqrtOp.Name)
                {
                    if (operand < 0)
                        throw new Exception("Введены неверные данные");
                    return Math.Sqrt(operand);
                }
                else if (operation == CalcOperation.SqrOp.Name)
                {
                    return operand * operand;
                }
                else if (operation == CalcOperation.NegOp.Name)
                {
                    return -operand;
                }
                else if (operation == CalcOperation.ByXOp.Name)
                {
                    double res = 1 / operand;
                    if (double.IsInfinity(res))
                    {
                        throw new Exception("Деление на ноль невозможно");
                    }
                    return res ;
                }
                else
                {
                    throw new Exception("Неизвестная операция: " + operation);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
