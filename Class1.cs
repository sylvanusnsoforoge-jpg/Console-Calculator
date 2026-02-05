using System;

namespace ScientificCalculator
{
    class Program
    {
        public class Calculator
        {
            private double[] previousResults = new double[10];
            private int resultIndex = 0;

            public void Run()
            {
                double currentResult = 0;
                bool usePreviousResult = false;

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Scientific Calculator");
                    Console.WriteLine("---------------------");
                    Console.WriteLine("+: Addition");
                    Console.WriteLine("-: Subtraction");
                    Console.WriteLine("*: Multiplication");
                    Console.WriteLine("/: Division");
                    Console.WriteLine("^: Power x^y");
                    Console.WriteLine("%: Modulus x%y");
                    Console.WriteLine("s: Sine (degrees)");
                    Console.WriteLine("c: Cosine (degrees)");
                    Console.WriteLine("t: Tangent (degrees)");
                    Console.WriteLine("r: View previous results");
                    Console.WriteLine("sa: Start afresh");
                    Console.WriteLine("q: Exit");
                    Console.Write("Enter option: ");

                    string operation = Console.ReadLine().Trim().ToLower();

                    if (operation == "q")
                        break;

                    if (operation == "sa")
                    {
                        ResetCalculator(ref currentResult, ref usePreviousResult);
                        continue;
                    }

                    if (operation == "r")
                    {
                        ShowPreviousResults(ref currentResult, ref usePreviousResult);
                        continue;
                    }

                    bool success;
                    double result;

                    if (usePreviousResult)
                    {
                        success = PerformWithPrevious(operation, currentResult, out result);
                        usePreviousResult = false;
                    }
                    else
                    {
                        success = PerformNew(operation, out result);
                    }

                    if (success)
                        SaveResult(result);

                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }

            private void ResetCalculator(ref double currentResult, ref bool usePrevious)
            {
                previousResults = new double[10];
                resultIndex = 0;
                currentResult = 0;
                usePrevious = false;
            }

            private void ShowPreviousResults(ref double currentResult, ref bool usePrevious)
            {
                if (resultIndex == 0)
                {
                    Console.WriteLine("No previous results.");
                }
                else
                {
                    for (int i = 0; i < resultIndex; i++)
                        Console.WriteLine($"Result {i + 1}: {previousResults[i]}");

                    Console.Write("Use last result? (yes/no): ");
                    if (Console.ReadLine().ToLower() == "yes")
                    {
                        currentResult = previousResults[resultIndex - 1];
                        usePrevious = true;
                        Console.WriteLine($"Using: {currentResult}");
                    }
                }
                Console.ReadKey();
            }

            private bool PerformWithPrevious(string op, double prev, out double result)
            {
                result = 0;

                try
                {
                    if (IsTrig(op))
                    {
                        result = CalculateTrigonometric(op, prev);
                        Console.WriteLine($"Result: {result}");
                    }
                    else
                    {
                        double num2 = ReadNumber("Enter second number: ");
                        result = CalculateOperation(op, prev, num2);
                        Console.WriteLine($"Result: {result}");
                    }
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                    return false;
                }
            }

            private bool PerformNew(string op, out double result)
            {
                result = 0;

                try
                {
                    if (IsTrig(op))
                    {
                        double num = ReadNumber("Enter number (degrees): ");
                        result = CalculateTrigonometric(op, num);
                    }
                    else
                    {
                        double a = ReadNumber("Enter first number: ");
                        double b = ReadNumber("Enter second number: ");
                        result = CalculateOperation(op, a, b);
                    }
                    Console.WriteLine($"Result: {result}");
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                    return false;
                }
            }

            private void SaveResult(double result)
            {
                if (resultIndex < 10)
                    previousResults[resultIndex++] = result;
                else
                {
                    Array.Copy(previousResults, 1, previousResults, 0, 9);
                    previousResults[9] = result;
                }
            }

            // ✅ FIXED METHOD
            private double ReadNumber(string message)
            {
                Console.Write(message);

                double num;
                while (!double.TryParse(Console.ReadLine(), out num))
                {
                    Console.Write("Invalid input. Try again: ");
                }

                return num;
            }

            private bool IsTrig(string op) => op == "s" || op == "c" || op == "t";

            private double CalculateTrigonometric(string op, double degrees)
            {
                double radians = degrees * Math.PI / 180;

                if (op == "t" && Math.Cos(radians) == 0)
                    throw new ArgumentException("Tangent undefined at this angle.");

                return op switch
                {
                    "s" => Math.Sin(radians),
                    "c" => Math.Cos(radians),
                    "t" => Math.Tan(radians),
                    _ => throw new ArgumentException("Invalid operation")
                };
            }

            private double CalculateOperation(string op, double a, double b)
            {
                return op switch
                {
                    "+" => a + b,
                    "-" => a - b,
                    "*" => a * b,
                    "/" => b == 0 ? throw new DivideByZeroException("Cannot divide by zero.") : a / b,
                    "%" => a % b,
                    "^" => Math.Pow(a, b),
                    _ => throw new ArgumentException("Invalid operation")
                };
            }
        }

        static void Main()
        {
            new Calculator().Run();
        }
    }
}
