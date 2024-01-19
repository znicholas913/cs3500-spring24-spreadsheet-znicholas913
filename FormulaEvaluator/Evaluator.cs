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




            if (result) // if t is an integer
            {
                if (oper.Peek() == "*" ||
                    oper.Peek() == "/") // if the top operator of the stack is multiplication or division 
                {
                    //pop the top value and the top operator and apply them to the next value t
                    int x = value.Pop();
                    int y = Int32.Parse(substrings[i]);
                    String opp = oper.Pop();
                    value.Push(calc(x, y, opp));
                }
                else
                {
                    value.Push(Int32.Parse(substrings[i]));
                }
            }





            else if (substrings[i] != "+" || substrings[i] != "-" || substrings[i] != "*" || substrings[i] != "/"
                     || substrings[i] != ")" || substrings[i] != "(") // t is a variable
            {
                if (oper.Peek() == "*" ||
                    oper.Peek() == "/") // if the top operator of the stack is multiplication or division 
                {
                    //pop the top value and the top operator and apply them to the next value t
                    int x = value.Pop();
                    int y = variableEvaluator(substrings[i]);
                    String opp = oper.Pop();
                    value.Push(calc(x, y, opp));
                }
                else
                {
                    value.Push(variableEvaluator(substrings[i]));
                }
            }



            else if (substrings[i] == "+" || substrings[i] == "-") //if the next t is + or - 
            {
                if (oper.Peek() == "+" ||
                    oper.Peek() == "-") // if the top operator of the stack is addition or subtraction
                {
                    //pop the top two values and the top operator and push the end result
                    int x = value.Pop();
                    int y = value.Pop();
                    String opp = oper.Pop();
                    value.Push(calc(x, y, opp));
                }

                oper.Push(substrings[i]);
            }


            else if
                (substrings[i] == "*" ||
                 substrings[i] == "/") //if the next t is * or / then just push it to the operator stack
            {
                oper.Push(substrings[i]);
            }
            else if (substrings[i] == "(") // if the next t is ( then push it to the operator stack
            {
                oper.Push(substrings[i]);
            }
            //if the next t is ) then continue
            else if (substrings[i] == ")")
            {
                //1. if + or - is at the top , pop the value stack twice and operator stack once. Push result into value stack.
                if (oper.Peek() == "+" ||
                    oper.Peek() == "-") // if the top operator of the stack is addition or subtraction.
                {
                    //pop the top two values and the top operator and push the end result.
                    int x = value.Pop();
                    int y = value.Pop();
                    String opp = oper.Pop();
                    value.Push(calc(x, y, opp));
                }
                //2. the top of the operator stack should be ( , pop it.
                else if (oper.Peek() == "(")
                    oper.Pop();

                //3. If * or / is at the top, pop the value stack twice and operator stack once. Push result into value stack.
                if (oper.Peek() == "*" || oper.Peek() == "/")
                {
                    //pop the top two values and the top operator and push the end result.
                    int x = value.Pop();
                    int y = value.Pop();
                    String opp = oper.Pop();
                    value.Push(calc(x, y, opp));
                }
            }
        }
        return value.Pop();
    }

    /// <summary>
/// Takes in 2 numbers and an operator. It checks to see what the operator is and returns the value of the expression.
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
            return x - y;
        if (oper == "*")
            return x * y;
        if (oper == "/")
            return x / y;
        return 0;
    }
}