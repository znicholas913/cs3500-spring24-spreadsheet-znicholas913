using System.Text.RegularExpressions;

namespace FormulaEvaluator;
/// <summary>
/// Created by: Nicholas Zamani
/// Date: January 21, 2024
/// This class takes in a string of a formula and turns it into the correct integer answer. For example, "5+5" would be
/// imputed and 10 would be returned. It also uses a delegate to give variables values to be used in the formula.
/// </summary>
public static class Evaluator
{
    public delegate int Lookup(String variable_name);

    /// <summary>
    /// The purpose of this function is to take in a string and calculate the value from it.
    /// For example, we would enter a string of "5+6" and it should return 11.
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
        string[] substrings = Regex.Split(expression, "(\\()|(\\))|(\\-)|(\\+)|(\\*)|(\\/)");
        //Removes the whitespace from each string in the array.
        for (int i = 0; i < substrings.Length; i++)
        {
            substrings[i] = substrings[i].Trim();
            if (substrings[i] == "" || substrings[i] == " ")
                continue;
            int j = 0;
            bool result = int.TryParse(substrings[i], out j);




            if (result) // if t is an integer
            {
                if (oper.Count != 0 && (oper.Peek() == "*" || oper.Peek() == "/")) // if the top operator of the stack is multiplication or division 
                {
                    //pop the top value and the top operator and apply them to the next value t
                    value.Push(calc(value.Pop(), Int32.Parse(substrings[i]), oper.Pop()));
                }
                else
                {
                    value.Push(Int32.Parse(substrings[i]));
                }
                continue;
            }
            // if t is a variable
            if (substrings[i] != "+" && substrings[i] != "-" && substrings[i] != "*" && substrings[i] != "/"
                && substrings[i] != ")" && substrings[i] != "(") 
            {
                // if the top operator of the stack is multiplication or division 
                if (oper.Count != 0 && (oper.Peek() == "*" || oper.Peek() == "/")) 
                {
                    //pop the top value and the top operator and apply them to the next value t
                    try
                    {
                        value.Push(calc(value.Pop(), variableEvaluator(substrings[i]), oper.Pop()));
                    }
                    catch (Exception e)
                    {
                        throw new ArgumentException("Variable has no value.");
                    }
                }
                else
                {
                    try
                    {
                        value.Push(variableEvaluator(substrings[i]));
                    }
                    catch (Exception e)
                    {
                        throw new ArgumentException("Variable has no value.");
                    }
                }
                continue;
            }
            //if the next t is + or - 
            if (substrings[i] == "+" || substrings[i] == "-") 
            {
                // if the top operator of the stack is addition or subtraction
                if (oper.Count != 0 && (oper.Peek() == "+" || oper.Peek() == "-")) 
                {
                    if (value.Count < 2 )
                    {
                        throw new ArgumentException("This is a bad formula");
                    }
                    //pop the top two values and the top operator and push the end result
                    value.Push(calc(value.Pop(), value.Pop(), oper.Pop()));
                }

                oper.Push(substrings[i]);
                continue;
            }

            //if the next t is * or / then just push it to the operator stack
            if (substrings[i] == "*" || substrings[i] == "/") 
            {
                oper.Push(substrings[i]);
                continue;
            }
            // if the next t is ( then push it to the operator stack
            if (substrings[i] == "(") 
            {
                oper.Push(substrings[i]);
                continue;
            }
            //if the next t is ) then continue
            if (substrings[i] == ")")
            {
                //1. if + or - is at the top , pop the value stack twice and operator stack once. Push result into value stack.
                // if the top operator of the stack is addition or subtraction.
                if (oper.Peek() == "+" || oper.Peek() == "-") 
                {
                    //pop the top two values and the top operator and push the end result.
                    value.Push(calc(value.Pop(), value.Pop(), oper.Pop()));
                }
                //2. the top of the operator stack should be ( , pop it.
                if (oper.Count > 0 && oper.Peek() == "(")
                    oper.Pop();
                else
                {
                    throw new ArgumentException("Invalid Formula");
                }

                //3. If * or / is at the top, pop the value stack twice and operator stack once. Push result into value stack.
                if (oper.Count > 0 && (oper.Peek() == "*" || oper.Peek() == "/"))
                {
                    //pop the top two values and the top operator and push the end result.
                    value.Push(calc(value.Pop(), value.Pop(), oper.Pop()));
                }
            }
        }

        if (oper.Count == 0 && value.Count == 0)
        {
            throw new ArgumentException("Stack is empty");
        }
        if (oper.Count == 0)
            return value.Pop();
        if (oper.Count != 0)
        {
            //checks to see if there is not enough values or if there are too many operators left.
            if (value.Count < 2 || oper.Count > 1)
            {
                throw new ArgumentException("This is a bad formula");
            }
            return calc(value.Pop(), value.Pop(), oper.Pop());
        }
        return -1;
    }

    /// <summary>
/// Takes in 2 numbers and an operator. It checks to see what the operator is and returns the value of the expression.
/// It will also check to make sure we are not dividing by zero.
/// </summary>
/// <param name="x"></param>
/// <param name="y"></param>
/// <param name="oper"></param>
/// <returns>int value of the expression</returns>
    private static int calc(int x, int y, String oper)
    {
        if (oper == "+")
            return x + y;
        if (oper == "-")
            return y - x;
        if (oper == "*")
            return x * y;
        if (oper == "/" && y == 0)
            throw new ArgumentException("Cannot divide by zero.");
        if (oper == "/")
            return x / y;
        return 0;
    }
}