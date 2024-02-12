// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens


using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
  /// <summary>
  /// Represents formulas written in standard infix notation using standard precedence
  /// rules.  The allowed symbols are non-negative numbers written using double-precision 
  /// floating-point syntax (without unary preceeding '-' or '+'); 
  /// variables that consist of a letter or underscore followed by 
  /// zero or more letters, underscores, or digits; parentheses; and the four operator 
  /// symbols +, -, *, and /.  
  /// 
  /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
  /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
  /// and "x 23" consists of a variable "x" and a number "23".
  /// 
  /// Associated with every formula are two delegates:  a normalizer and a validator.  The
  /// normalizer is used to convert variables into a canonical form, and the validator is used
  /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
  /// that it consist of a letter or underscore followed by zero or more letters, underscores,
  /// or digits.)  Their use is described in detail in the constructor and method comments.
  /// Finished by: Nicholas Zamani
  /// Date: 02/04/2024
  /// </summary>
  public class Formula
  {

    /// <summary>
    /// Creates a Formula from a string that consists of an infix expression written as
    /// described in the class comment.  If the expression is syntactically invalid,
    /// throws a FormulaFormatException with an explanatory Message.
    /// 
    /// The associated normalizer is the identity function, and the associated validator
    /// maps every string to true.  
    /// </summary>
    private Func<string, string> _normalize;
    private Func<string, bool> _isValid;
    private readonly string [] _expression;
    public Formula(String formula) :
        this(formula, s => s, s => true)
    {
      this._isValid = s => true;
      this._normalize = s => s;
      _expression = new string[GetTokens(formula).Count()];
      int count = 0;
      foreach (var token in GetTokens(formula))
      {
          _expression[count] = token;
          count++;
      }
      CheckFormula();
    }

    /// <summary>
    /// Creates a Formula from a string that consists of an infix expression written as
    /// described in the class comment.  If the expression is syntactically incorrect,
    /// throws a FormulaFormatException with an explanatory Message.
    /// 
    /// The associated normalizer and validator are the second and third parameters,
    /// respectively.  
    /// 
    /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
    /// throws a FormulaFormatException with an explanatory message. 
    /// 
    /// If the formula contains a variable v such that isValid(normalize(v)) is false,
    /// throws a FormulaFormatException with an explanatory message.
    /// 
    /// Suppose that N is a method that converts all the letters in a string to upper case, and
    /// that V is a method that returns true only if a string consists of one letter followed
    /// by one digit.  Then:
    /// 
    /// new Formula("x2+y3", N, V) should succeed
    /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
    /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
    /// </summary>
    public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
    {
      this._normalize = normalize;
      this._isValid = isValid;
      _expression = new string[GetTokens(formula).Count()];
      int count = 0;
      foreach (var token in GetTokens(formula))
      {
          _expression[count] = token;
          count++;
      }
      CheckFormula();
    }

    /// <summary>
    /// Evaluates this Formula, using the lookup delegate to determine the values of
    /// variables.  When a variable symbol v needs to be determined, it should be looked up
    /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
    /// the constructor.)
    /// 
    /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
    /// in a string to upper case:
    /// 
    /// new Formula("x+7", N, s => true).Evaluate(L) is 11
    /// new Formula("x+7").Evaluate(L) is 9
    /// 
    /// Given a variable symbol as its parameter, lookup returns the variable's value 
    /// (if it has one) or throws an ArgumentException (otherwise).
    /// 
    /// If no undefined variables or divisions by zero are encountered when evaluating 
    /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
    /// The Reason property of the FormulaError should have a meaningful explanation.
    ///
    /// This method should never throw an exception.
    /// </summary>
    public object Evaluate(Func<string, double> lookup)
    {
        //helper method 
      return Calculate(lookup);
    }

    /// <summary>
    /// Enumerates the normalized versions of all of the variables that occur in this 
    /// formula.  No normalization may appear more than once in the enumeration, even 
    /// if it appears more than once in this Formula.
    /// 
    /// For example, if N is a method that converts all the letters in a string to upper case:
    /// 
    /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
    /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
    /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
    /// </summary>
    public IEnumerable<String> GetVariables()
    {
        List<string> variables = new List<string>();
        //loop through all of the tokens
        foreach (var item in _expression)
        {
            int j = 0;
            bool result = int.TryParse(item, out j);
            //check to make sure the item is a variable.
            if (!result && item != "+" && item != "-" && item != "*" && item != "/"
                && item != ")" && item != "(" && !variables.Contains(item))
            {
                //add the variables to a list.
                variables.Add(item);
            }
        }
        return variables;
    }

    /// <summary>
    /// Returns a string containing no spaces which, if passed to the Formula
    /// constructor, will produce a Formula f such that this.Equals(f).  All of the
    /// variables in the string should be normalized.
    /// 
    /// For example, if N is a method that converts all the letters in a string to upper case:
    /// 
    /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
    /// new Formula("x + Y").ToString() should return "x+Y"
    /// </summary>
    public override string ToString()
    {
        double j = 0;
        String toString = "";
        for (int i = 0; i < _expression.Length; i++)
        {
            if (double.TryParse(_expression[i], out j))
            {
                toString += RemoveZeros(_expression[i]);
            }
            //add the token to the string
            else
                toString += _expression[i];
        } 
        return toString;
    }
/// <summary>
/// Remove any unwanted or extra zeros from the end of a double.
/// </summary>
/// <param name="s"></param>
/// <returns></returns>
    private string RemoveZeros(string s)
    {
        char[] characters = s.ToCharArray();
        List<char> charList = characters.ToList();
        if (!charList.Contains('.'))
            return s;
        for (int i = charList.Count - 1; i > 0; i--)
        {
            if (charList[i] != '0')
                break;
            else
            {
                charList.Remove(charList[i]);
            }
        }

        if (charList[charList.Count-1] == '.')
            charList.Remove(charList[charList.Count-1]);
        char[] temp2 = charList.ToArray();
        return new string(temp2);
    }
    /// <summary>
    ///  <change> make object nullable </change>
    ///
    /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
    /// whether or not this Formula and obj are equal.
    /// 
    /// Two Formulae are considered equal if they consist of the same tokens in the
    /// same order.  To determine token equality, all tokens are compared as strings 
    /// except for numeric tokens and variable tokens.
    /// Numeric tokens are considered equal if they are equal after being "normalized" 
    /// by C#'s standard conversion from string to double, then back to string. This 
    /// eliminates any inconsistencies due to limited floating point precision.
    /// Variable tokens are considered equal if their normalized forms are equal, as 
    /// defined by the provided normalizer.
    /// 
    /// For example, if N is a method that converts all the letters in a string to upper case:
    ///  
    /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
    /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
    /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
    /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
    /// </summary>
    public override bool Equals(object? obj)
    {
        //Check if the object is null or not a formula.
        if (obj == null || !(obj is Formula))
            return false;
        //Check if both strings are the same.
        if (obj.ToString() == this.ToString())
            return true; 
        return false;
    }

    /// <summary>
    ///   <change> We are now using Non-Nullable objects.  Thus neither f1 nor f2 can be null!</change>
    /// Reports whether f1 == f2, using the notion of equality from the Equals method.
    /// 
    /// </summary>
    public static bool operator ==(Formula f1, Formula f2)
    {
        //Check if both strings are the same.
        if (f1.ToString() == f2.ToString())
            return true;
        return false;
    }

    /// <summary>
    ///   <change> We are now using Non-Nullable objects.  Thus neither f1 nor f2 can be null!</change>
    ///   <change> Note: != should almost always be not ==, if you get my meaning </change>
    ///   Reports whether f1 != f2, using the notion of equality from the Equals method.
    /// </summary>
    public static bool operator !=(Formula f1, Formula f2)
    {
        //Check if both strings are the same.
        if (f1.ToString() == f2.ToString())
            return false;
        return true;
    }

    /// <summary>
    /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
    /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
    /// randomly-generated unequal Formulae have the same hash code should be extremely small.
    /// </summary>
    public override int GetHashCode()
    {
        int code = 0;
        foreach (var item in _expression)
        {
            int j = 0;
            bool result = int.TryParse(item, out j);
            //If the current token is an integer, add that value to the hashcode.
            if (result)
                code += int.Parse(item);
            //If the current token is + or - then add one to the hashcode.
            if (item == "+" || item == "-")
                code += 1;
            //If the current token is / or * then add 2 to the hashcode. 
            if (item == "/" || item == "*")
                code += 2;
            //For any other tokens, add 3 to the hashcode.
            else
            {
                code += 3;
            }
        }
        return code;;
    }

    /// <summary>
    /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
    /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
    /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
    /// match one of those patterns.  There are no empty tokens, and no token contains white space.
    /// </summary>
    private static IEnumerable<string> GetTokens(String formula)
    {
      // Patterns for individual tokens
      String lpPattern = @"\(";
      String rpPattern = @"\)";
      String opPattern = @"[\+\-*/]";
      String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
      String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
      String spacePattern = @"\s+";

      // Overall pattern
      String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                      lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

      // Enumerate matching tokens that don't consist solely of white space.
      foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
      {
        if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
        {
          yield return s;
        }
      }

    }
    /// <summary>
    /// This will verify that the formula created is a valid formula. This will also normalize and
    /// validate any variables. If anything is wrong with the formula it will throw an exception.
    /// </summary>
    /// <returns>Return true if it's a valid formula.</returns>
    /// <exception cref="FormulaFormatException"></exception>
    private bool CheckFormula()
    {
      int intCount = 0;
      int opCount = 0;
      int rightParen = 0;
      int leftParen = 0;
      for ( int i = 0; i < _expression.Length; i++)
      {
        int j = 0;
        bool result = int.TryParse(_expression[i], out j);
        //If the token is an integer, add to counter.
        if (result)
        {
          intCount++;
          continue;
        }
        //checks if it is a variable and if the variable is valid. Will also normalize the variable.
        if (_expression[i] != "+" && _expression[i] != "-" && _expression[i] != "*" && _expression[i] != "/"
            && _expression[i] != ")" && _expression[i] != "(")
        { 
            _expression[i] = _normalize(_expression[i]);
          if (!_isValid(_normalize(_expression[i])))
            throw new FormulaFormatException("This is not a valid variable.");
          //Since this is a number as well, we add to the integer counter.
          intCount++;
        }
        //checks if the current index is an operator and it keeps track of how many there have been.
        if (_expression[i] == "+" || _expression[i] == "-" || _expression[i] == "*" || _expression[i] == "/")
        {
          opCount++;
          continue;
        }
        //Keep track of the amount of left and right parentheses.
        if (_expression[i] != "(")
          leftParen++;
        if (_expression[i] != ")")
          rightParen++;
        
        else
        {
          //checks to see if the variable is valid.
          if (!_isValid(_normalize(_expression[i])))
          {
            throw new FormulaFormatException("This is not a valid variable.");
          }
        }
      }
      //checks to see if there are too many operators.
      if (opCount >= intCount)
      {
        throw new FormulaFormatException("There are too many operators in this formula.");
      }
      //makes sure the formula doesn't end with an operator.
      if (_expression[_expression.Length-1] == "+" || _expression[_expression.Length-1] == "-" || 
          _expression[_expression.Length-1] == "/" || _expression[_expression.Length-1] == "*" ||
          _expression[_expression.Length-1] == "(")
      {
        throw new FormulaFormatException("Cannot end the formula with an operator.");
      }
      //makes sure the formula doesn't start with an operator.
      if (_expression[0] == "+" || _expression[0] == "-" || 
          _expression[0] == "/" || _expression[0] == "*" ||
          _expression[0] == ")")
      {
        throw new FormulaFormatException("Cannot start the formula with an operator.");
      }
      //checks to see if there are the same amount of right and left parentheses.
      if (rightParen != leftParen)
      {
        throw new FormulaFormatException("There are not the correct amount of parentheses");
      }
      return true;
    }
    /// <summary>
    /// The purpose of this function is to take in a string and calculate the value from it.
    /// For example, we would enter a string of "5+6" and it should return 11.
    /// 
    /// </summary>
    /// <param name="lookup"></param>
    /// <returns>The int value of the expression</returns>
    private double Calculate(Func<string, double> lookup)
    {
        //creates a value and an operator stack.
        Stack<double> value = new Stack<double>();
        Stack<String> oper = new Stack<String>();
        for (int i = 0; i < _expression.Length; i++)
        {
            double j = 0;
            bool result = double.TryParse(_expression[i], out j);
            
            if (result) // if t is a double.
            {
                if (oper.Count != 0 && (oper.Peek() == "*" || oper.Peek() == "/")) // if the top operator of the stack is multiplication or division 
                {
                    //pop the top value and the top operator and apply them to the next value t
                    value.Push(Calc(value.Pop(), Double.Parse(_expression[i]), oper.Pop()));
                }
                else
                {
                    value.Push(Double.Parse(_expression[i]));
                }
                continue;
            }
            // if t is a variable
            if (_expression[i] != "+" && _expression[i] != "-" && _expression[i] != "*" && _expression[i] != "/"
                && _expression[i] != ")" && _expression[i] != "(") 
            {
                // if the top operator of the stack is multiplication or division 
                if (oper.Count != 0 && (oper.Peek() == "*" || oper.Peek() == "/")) 
                {
                    //pop the top value and the top operator and apply them to the next value t
                    value.Push(Calc(value.Pop(), lookup(_expression[i]), oper.Pop()));
                }
                else
                {
                    value.Push(lookup(_expression[i]));
                }
                continue;
            }
            //if the next t is + or - 
            if (_expression[i] == "+" || _expression[i] == "-") 
            {
                // if the top operator of the stack is addition or subtraction
                if (oper.Count != 0 && (oper.Peek() == "+" || oper.Peek() == "-")) 
                {
                    //pop the top two values and the top operator and push the end result
                    value.Push(Calc(value.Pop(), value.Pop(), oper.Pop()));
                }

                oper.Push(_expression[i]);
                continue;
            }

            //if the next t is * or / then just push it to the operator stack
            if (_expression[i] == "*" || _expression[i] == "/") 
            {
                oper.Push(_expression[i]);
                continue;
            }
            // if the next t is ( then push it to the operator stack
            if (_expression[i] == "(") 
            {
                oper.Push(_expression[i]);
                continue;
            }
            //if the next t is ) then continue
            if (_expression[i] == ")")
            {
                //1. if + or - is at the top , pop the value stack twice and operator stack once. Push result into value stack.
                // if the top operator of the stack is addition or subtraction.
                if (oper.Peek() == "+" || oper.Peek() == "-") 
                {
                    //pop the top two values and the top operator and push the end result.
                    value.Push(Calc(value.Pop(), value.Pop(), oper.Pop()));
                }
                //2. the top of the operator stack should be ( , pop it. 
                if (oper.Count > 0 && oper.Peek() == "(")
                    oper.Pop();
                //3. If * or / is at the top, pop the value stack twice and operator stack once. Push result into value stack.
                if (oper.Count > 0 && (oper.Peek() == "*" || oper.Peek() == "/"))
                {
                    //pop the top two values and the top operator and push the end result.
                    value.Push(Calc(value.Pop(), value.Pop(), oper.Pop()));
                }
            }
        }
        if (oper.Count == 0)
            return value.Pop();
        if (oper.Count != 0)
        {
            return Calc(value.Pop(), value.Pop(), oper.Pop());
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
    private static double Calc(double x, double y, String oper)
    {
        if (oper == "+")
            return x + y;
        if (oper == "-")
            return y - x;
        if (oper == "*")
            return x * y;
        if (oper == "/" && y == 0)
            throw new ArgumentException("Cannnot divide by zero.");
        if (oper == "/")
            return x / y;
        return 0;
    }
  }
  

  /// <summary>
  /// Used to report syntactic errors in the argument to the Formula constructor.
  /// </summary>
  public class FormulaFormatException : Exception
  {
    /// <summary>
    /// Constructs a FormulaFormatException containing the explanatory message.
    /// </summary>
    public FormulaFormatException(String message)
        : base(message)
    {
    }
  }

  /// <summary>
  /// Used as a possible return value of the Formula.Evaluate method.
  /// </summary>
  public struct FormulaError
  {
    /// <summary>
    /// Constructs a FormulaError containing the explanatory reason.
    /// </summary>
    /// <param name="reason"></param>
    public FormulaError(String reason)
        : this()
    {
      Reason = reason;
    }

    /// <summary>
    ///  The reason why this FormulaError was created.
    /// </summary>
    public string Reason { get; private set; }
  }
}


// <change>
//   If you are using Extension methods to deal with common stack operations (e.g., checking for
//   an empty stack before peeking) you will find that the Non-Nullable checking is "biting" you.
//
//   To fix this, you have to use a little special syntax like the following:
//
//       public static bool OnTop<T>(this Stack<T> stack, T element1, T element2) where T : notnull
//
//   Notice that the "where T : notnull" tells the compiler that the Stack can contain any object
//   as long as it doesn't allow nulls!
// </change>
