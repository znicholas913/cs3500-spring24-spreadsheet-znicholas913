// See https://aka.ms/new-console-template for more information

using FormulaEvaluator;

if (Evaluator.Evaluate("5+5",null) == 10) 
    Console.WriteLine("Happy Day!");
else
{
    Console.WriteLine("Did not work");
}

if (Evaluator.Evaluate("5+5*7",null) == 40) 
    Console.WriteLine("Happy Day!");
else
{
    Console.WriteLine("Did not work");
}
if (Evaluator.Evaluate("8*13",null) == 104) 
    Console.WriteLine("Happy Day!");
else
{
    Console.WriteLine("Did not work");
}
if (Evaluator.Evaluate("5+5*(2+6)",null) == 45) 
    Console.WriteLine("Happy Day!");
else
{
    Console.WriteLine("Did not work");
}