// See https://aka.ms/new-console-template for more information

using FormulaEvaluator;

if (Evaluator.Evaluate("5+5",null) == 10) 
    Console.WriteLine("Happy Day!");
else
{
    Console.WriteLine(Evaluator.Evaluate("5+5",null));
    Console.WriteLine("Did not work");
}