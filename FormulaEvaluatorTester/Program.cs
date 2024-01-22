using FormulaEvaluator;

/// <summary>
/// Created by: Nicholas Zamani
/// Date: January 21, 2024
/// This file is the tester file for the FormulaEvaluator class.
/// </summary>

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
if (Evaluator.Evaluate("10*47",null) == 470) 
    Console.WriteLine(Evaluator.Evaluate("10*47",null));
else
{
    Console.WriteLine("Did not work");
}
if (Evaluator.Evaluate("5+5*(2+6)+pp1",method) == 50) 
    Console.WriteLine("Happy Day!");
else
{
    Console.WriteLine("Did not work");
}
if (Evaluator.Evaluate("5/1",method) == 5) 
    Console.WriteLine("Happy Day!");
else
{
    Console.WriteLine("Did not work 44");
}
// if (Evaluator.Evaluate("-(5+5)",method) == 10) 
//     Console.WriteLine("Happy Day!");
// else
// {
//     Console.WriteLine("Did not work 54");
// }
if (Evaluator.Evaluate("5+pp1", method) == 10) 
    Console.WriteLine("Happy Day!");
else
{
    Console.WriteLine("Did not work 54");
}
// if (Evaluator.Evaluate("5/pp1", null) == 1) 
//     Console.WriteLine("Happy Day!");
// else
// {
//     Console.WriteLine("Did not work 54");
// }
if (Evaluator.Evaluate("pp1*pp2", method) == 30) 
    Console.WriteLine("Happy Day!");
else
{
    Console.WriteLine("Did not work 54");
}
// if (Evaluator.Evaluate("pp1/-pp2", method) == 30) 
//     Console.WriteLine("Happy Day!");
// else
// {
//     Console.WriteLine("Did not work 54");
// }
if (Evaluator.Evaluate("pp1&pp2", method) == 30) 
    Console.WriteLine("Happy Day!");
else
{
    Console.WriteLine("Did not work 86");
}

/// <summary>
/// Using this method to help test the delegate. Passes in a string to see if that variable has a given value.
/// </summary>
/// <param name="str"></param>
/// <returns>The int value of the varaibles</returns>
static int method(String str)
{
    if (str == "pp1")
        return 5;
    if (str == "pp2")
        return 6;
    throw new Exception("Variable doesn't exist");
}