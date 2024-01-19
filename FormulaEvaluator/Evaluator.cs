using System.Text.RegularExpressions;

namespace FormulaEvaluator;
/// <summary>
/// put name date and summary here
/// </summary>
public static class Evaluator
{
    public delegate int Lookup(String variable_name);

    /// <summary>
    /// The purpose of this function is to take in a string and calculate the value from it.
    /// For example, we would enter a string of "5+5" and it should return 10.
    /// 
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="variableEvaluator"></param>
    /// <returns>The int value of the expression</returns>
    public static int Evaluate(String expression,
        Lookup variableEvaluator)
    {
        //creates a value and an operator stack.
        Stack<int> value = new Stack<int>();
        Stack<String> oper = new Stack<String>();
        //creates an array of all the characters in the string.
        string[] substrings = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/) ");
        //Removes the whitespace from each string in the array.
        for (int i = 0; i < substrings.Length; i++)
        {
            substrings[i] = substrings[i].Trim();
            int j = 0;
            bool result = int.TryParse(substrings[i], out j);
            //if the string is an int, it will be added to the value stack, otherwise it gets added to the operator stack.
            if (result)
                value.Push(Int32.Parse(substrings[i]));
            else
            {
                oper.Push(substrings[i]);
            }
        }
        

        return 0;
    }
}