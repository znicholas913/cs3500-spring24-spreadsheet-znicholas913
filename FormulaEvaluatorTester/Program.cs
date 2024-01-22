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
if (Evaluator.Evaluate("-(5+5)",method) == 10) 
    Console.WriteLine("Happy Day!");
else
{
    Console.WriteLine("Did not work 54");
}

// using this method to test the delegate
static int method(String str)
{
    if (str == "pp1")
        return 5;
    return -1;
}