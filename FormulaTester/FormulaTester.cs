using SpreadsheetUtilities;

namespace FormulaTester;

[TestClass]
public class FormulaTester
{
    /// <summary>
    ///This is my is valid method and it is checking if the second character is an integer.
    /// </summary>
    /// <param name="s"></param>
    /// <returns>true or false based on if the variable is valid.</returns>
    public bool validate(String s)
    {
        int j = 0;
        char[] characters = s.ToCharArray();
        if (characters.Length > 1 && Char.IsNumber(characters[1]))
            return true;
        return false;
    }

    private double VariableAssign(String s)
    {
        return 20.0;
    }
    
    
    [TestMethod]
    public void TestFormulaConstructor()
    {
        //create new formula object, make sure it doesn't throw.
        Formula tester = new Formula("3+5", s => s, validate);
    }
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException),
        "This is not a valid variable.")]
    public void TestFormulaConstructorThrows()
    {
        //create new formula object, make sure it doesn't throw.
        Formula tester = new Formula("3+5+3s", s => s, validate);
    }
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException),
        "There are too many operators in this formula.")]
    public void TestFormulaConstructorThrowsForOperators()
    {
        //create new formula object, make sure it doesn't throw.
        Formula tester = new Formula("3+5+", s => s, validate);
    }
    [TestMethod]
    public void TestFormulaConstructorWithValidVariables()
    {
        //create new formula object, make sure it doesn't throw.
        Formula tester = new Formula("3+s5*s4", s => s, validate);
    }
    [TestMethod]
    public void TestFormulaConstructorFailBecauseNormalized()
    {
        //create new formula object, make sure it doesn't throw.
        Formula tester = new Formula("3+s5*T4", s => s, validate);
    }
    [TestMethod]
    public void TestEvaluate()
    {
        //create new formula object, make sure it doesn't throw.
        Formula tester = new Formula("3+s5*4", s => s, validate);
        Assert.AreEqual(83.0, tester.Evaluate(VariableAssign));
    }
    [TestMethod]
    public void TestEvaluateWithDecimals()
    {
        //create new formula object, make sure it doesn't throw.
        Formula tester = new Formula("3.5+s5*0.3", s => s,s =>true);
        Assert.AreEqual(9.5, tester.Evaluate(VariableAssign));
    }
}
