// See https://aka.ms/new-console-template for more information

using EvaluatorLibrary;

try
{
    Console.WriteLine("Using ExpressionEvaluator.Calculate :");
    string expr = "5 + 2 - ( 2 * 3 ) / ( 1 + 7 * 2 )";
    Console.WriteLine($"Input is {expr}");
    double res = ExpressionEvaluator.Calculate(expr);
    Console.WriteLine($"Result is : {res.ToString()}");
    Console.WriteLine();
    Console.WriteLine();

    Console.WriteLine("Using ExpressionEvaluator.CalculateWithNestedBracketAndPrecedence :");
    expr = "5 + 2 - ( 2 * 3 ) / ( ( 1 + 7 ) * 2 )";
    Console.WriteLine($"Input is {expr}");
    res = ExpressionEvaluator.CalculateWithNestedBracketAndPrecedence(expr);
    Console.WriteLine($"Result is : {res.ToString()}");
    Console.WriteLine();
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
