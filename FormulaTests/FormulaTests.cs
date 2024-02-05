using SpreadsheetUtilities;

namespace FormulaTester;
/// <summary>
/// This is my test class which will be testing all of the code in the formula class.
/// Created by: Nicholas Zamani
/// Date: 02/04/2024
/// </summary>
[TestClass]
public class FormulaTests
{
    /// <summary>
    ///This is my is valid method and it is checking if the second character is an integer.
    /// </summary>
    /// <param name="s"></param>
    /// <returns>true or false based on if the variable is valid.</returns>
    public bool validate(String s)
    {
        char[] characters = s.ToCharArray();
        if (characters.Length > 1 && Char.IsNumber(characters[1]))
            return true;
        return false;
    }
/// <summary>
/// This will look for any letters in a variable and it will make them into uppercase characters and
/// then put back into the string.
/// </summary>
/// <param name="n"></param>
/// <returns>The same string will all the letters upper cased.</returns>
    public String Normal(String n)
    {
        char[] characters = n.ToCharArray();
        String temp = "";
        for (int i = 0; i < characters.Length; i ++)
        {
            if (!Char.IsNumber(characters[i]))
                characters[i] = char.ToUpper(characters[i]);
            temp += characters[i];
        }
        return temp;
    }
/// <summary>
/// Any variable is assigned the value of 20.
/// </summary>
/// <param name="s"></param>
/// <returns>20.0 as a double.</returns>
    private double VariableAssign(String s)
    {
        return 20.0;
    }
    
    /// <summary>
    /// Checking to make sure that the constructor doesn't throw when everything is correct.
    /// </summary>
    [TestMethod]
    public void TestFormulaConstructor()
    {
        //create new formula object, make sure it doesn't throw.
        Formula tester = new Formula("3+5", s => s, validate);
    }
    /// <summary>
    /// Checks to see if the constructor throws and error because the variable is invalid.
    /// The second character should be a number according to the validate method.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException),
        "This is not a valid variable.")]
    public void TestFormulaConstructorThrows()
    {
        //create new formula object, make sure it throws.
        Formula tester = new Formula("3+5+3s", s => s, validate);
    }
    /// <summary>
    /// Testing for an exception when there are too many operators in the formula.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException),
        "There are too many operators in this formula.")]
    public void TestFormulaConstructorThrowsForOperators()
    {
        //create new formula object, make sure it throws.
        Formula tester = new Formula("3+5+", s => s, validate);
    }
    /// <summary>
    /// Making sure that the formula constructor passes when the variables are valid and everything else is correct.
    /// </summary>
    [TestMethod]
    public void TestFormulaConstructorWithValidVariables()
    {
        //create new formula object, make sure it doesn't throw.
        Formula tester = new Formula("3+s5*s4", s => s, validate);
    }
    /// <summary>
    /// Another test to see if the constructor passes with different types of variables that
    /// are normalized.
    /// </summary>
    [TestMethod]
    public void TestFormulaConstructorNormalized()
    {
        //create new formula object, make sure it doesn't throw.
        Formula tester = new Formula("3+s5*T4", Normal , validate);
    }
    /// <summary>
    /// Testing to make sure that the evaluate method returns the correct answer.
    /// </summary>
    [TestMethod]
    public void TestEvaluate()
    {
        //create new formula object, make sure it doesn't throw.
        Formula tester = new Formula("3+s5*4", s => s, validate);
        Assert.AreEqual(83.0, tester.Evaluate(VariableAssign));
    }
    /// <summary>
    /// Testing the evaluate method with decimals as well as variables.
    /// </summary>
    [TestMethod]
    public void TestEvaluateWithDecimals()
    {
        //create new formula object, make sure it doesn't throw.
        Formula tester = new Formula("3.5+s5*0.3", s => s,s =>true);
        Assert.AreEqual(9.5, tester.Evaluate(VariableAssign));
    }
    /// <summary>
    /// Checking to make sure that the getVariables method returns only a list of variables and
    /// nothing else.
    /// </summary>
    [TestMethod]
    public void TestGetVariables()
    {
        //create new formula object, make sure it doesn't throw.
        Formula tester = new Formula("x+s+7+s4", s => s,s =>true);
        String[] testStrings = new[]{"x", "s", "s4"};
        IEnumerator<string> e1 = tester.GetVariables().GetEnumerator();
        for (int i = 0; i < tester.GetVariables().Count(); i++)
        {
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual(testStrings[i], e1.Current);
        }
    }
    /// <summary>
    /// Testing to make sure that the same variable doesn't get counted twice. We are also looking to see
    /// if the normalizer works in this situation. 
    /// </summary>
    [TestMethod]
    public void TestGetVariablesWithNormalizer()
    {
        //create new formula object, make sure it doesn't throw.
        Formula tester = new Formula("x+s+7+S", Normal,s =>true);
        String[] testStrings = new[]{"X", "S"};
        IEnumerator<string> e1 = tester.GetVariables().GetEnumerator();
        for (int i = 0; i < tester.GetVariables().Count(); i++)
        {
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual(testStrings[i], e1.Current);
        }
    }
    /// <summary>
    /// Checks to see if a string with a bunch of spaces returns a formula with no spaces and fully normalized.
    /// </summary>
    [TestMethod]
    public void TestToString()
    {
        //create new formula object, make sure it doesn't throw.
        Formula tester = new Formula("x + s + 7 + S", Normal,s =>true);
        Assert.AreEqual("X+S+7+S", tester.ToString());
    }
    /// <summary>
    /// Checks to see if a formula equals another formula with normalizer.
    /// </summary>
    [TestMethod]
    public void TestEquals()
    {
        //create new formula object, make sure it doesn't throw.
        Formula tester = new Formula("x + s + 7 + S", Normal,s =>true);
        Console.WriteLine(tester.ToString());
        Assert.IsTrue(tester.Equals(new Formula("X+S+7+S")));
    }
    /// <summary>
    /// Checks to see if a formula doesn't equals another formula when it shouldn't.
    /// </summary>
    [TestMethod]
    public void TestEqualsFalse()
    {
        //create new formula object, make sure it doesn't throw.
        Formula tester = new Formula("x + s + 7 + 5", Normal,s =>true);
        Assert.IsFalse(tester.Equals(new Formula("X+S+7+S")));
    }
    /// <summary>
    /// Checks to see if a formula doesn't equals another formula when the object is not a formula.
    /// </summary>
    [TestMethod]
    public void TestEqualsNotFormula()
    {
        //create new formula object, make sure it doesn't throw.
        Formula tester = new Formula("x + s + 7 + 5", Normal,s =>true);
        Assert.IsFalse(tester.Equals("X+S+7+5"));
    }
    /// <summary>
    /// Checks to see if a formula equals another formula.
    /// </summary>
    [TestMethod]
    public void TestEqualsOp()
    {
        //create new formula object, make sure it doesn't throw.
        Formula tester1 = new Formula("x + s + 7 + 5", Normal,s =>true);
        Formula tester2 = new Formula("X+S+7+5", Normal,s =>true);
        Assert.IsTrue(tester1==tester2);
    }
    /// <summary>
    /// Checks to see if a formula does not equal another formula.
    /// </summary>
    [TestMethod]
    public void TestNotEqualsOp()
    {
        //create new formula object, make sure it doesn't throw.
        Formula tester1 = new Formula("x + s + 7 + 5", Normal,s =>true);
        Formula tester2 = new Formula("X+S+7+s", Normal,s =>true);
        Assert.IsTrue(tester1!=tester2);
    }
    /// <summary>
    /// Checks to see if a formula does not not equal another formula.
    /// </summary>
    [TestMethod]
    public void TestEqualsOpFalse()
    {
        //create new formula object, make sure it doesn't throw.
        Formula tester1 = new Formula("x + s + 7 + 5", Normal,s =>true);
        Formula tester2 = new Formula("X+S+7+5", Normal,s =>true);
        Assert.IsFalse(tester1!=tester2);
    }
    /// <summary>
    /// Checks to see if a formula has the same hash code as the same formula.
    /// </summary>
    [TestMethod]
    public void TestGetHashCode()
    {
        //create new formula object, make sure it doesn't throw.
        Formula tester1 = new Formula("x + s + 7 + 5", Normal,s =>true);
        Formula tester2 = new Formula("X+S+7+5", Normal,s =>true);
        Assert.IsTrue(tester1.GetHashCode() == tester2.GetHashCode());
    }
    /// <summary>
    /// Checks to see if a formula has different hash code as a different formula.
    /// </summary>
    [TestMethod]
    public void TestGetHashCodeDiff()
    {
        //create new formula object, make sure it doesn't throw.
        Formula tester1 = new Formula("x + s + 7 + s", Normal,s =>true);
        Formula tester2 = new Formula("X+S+7+5", Normal,s =>true);
        Console.WriteLine(1*2.0000000000);
        Assert.IsFalse(tester1.GetHashCode() == tester2.GetHashCode());
    }
    /// <summary>
    /// Checks to see if a formula with many zeros at the end are the same as one with only one zero at the end.
    /// </summary>
    [TestMethod]
    public void TestEqualsWithManyZeros()
    {
        //create new formula object, make sure it doesn't throw.
        Formula tester1 = new Formula("1+2.00000", Normal,s =>true);
        Formula tester2 = new Formula("1+2.0", Normal,s =>true);
        Console.WriteLine(tester2.ToString());
        Console.WriteLine(tester1.ToString());
        Assert.IsTrue(tester1==tester2);
    }
    /// <summary>
    /// Checks to see if a formula with many zeros at the end are different because there is no decimal place.
    /// </summary>
    [TestMethod]
    public void TestEqualsWithManyZerosNoDecimal()
    {
        //create new formula object, make sure it doesn't throw.
        Formula tester1 = new Formula("1+200000", Normal,s =>true);
        Formula tester2 = new Formula("1+20", Normal,s =>true);
        Console.WriteLine(tester2.ToString());
        Console.WriteLine(tester1.ToString());
        Assert.IsFalse(tester1==tester2);
    }

}
