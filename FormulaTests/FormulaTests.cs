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
// Normalizer tests
    [TestMethod(), Timeout(2000)]
    [TestCategory("1")]
    public void TestNormalizerGetVars()
    {
      Formula f = new Formula("2+x1", s => s.ToUpper(), s => true);
      HashSet<string> vars = new HashSet<string>(f.GetVariables());

      Assert.IsTrue(vars.SetEquals(new HashSet<string> { "X1" }));
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("2")]
    public void TestNormalizerEquals()
    {
      Formula f = new Formula("2+x1", s => s.ToUpper(), s => true);
      Formula f2 = new Formula("2+X1", s => s.ToUpper(), s => true);

      Assert.IsTrue(f.Equals(f2));
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("3")]
    public void TestNormalizerToString()
    {
      Formula f = new Formula("2+x1", s => s.ToUpper(), s => true);
      Formula f2 = new Formula(f.ToString());

      Assert.IsTrue(f.Equals(f2));
    }

    // Validator tests
    [TestMethod(), Timeout(2000)]
    [TestCategory("4")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestValidatorFalse()
    {
      Formula f = new Formula("2+x1", s => s, s => false);
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("5")]
    public void TestValidatorX1()
    {
      Formula f = new Formula("2+x", s => s, s => (s == "x"));
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("6")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestValidatorX2()
    {
      Formula f = new Formula("2+y1", s => s, s => (s == "x"));
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("7")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestValidatorX3()
    {
      Formula f = new Formula("2+x1", s => s, s => (s == "x"));
    }


    // Simple tests that return FormulaErrors
    [TestMethod(), Timeout(2000)]
    [TestCategory("8")]
    public void TestUnknownVariable()
    {
      Formula f = new Formula("2+X1");
      Assert.IsInstanceOfType(f.Evaluate(s => { throw new ArgumentException("Unknown variable"); }), typeof(FormulaError));
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("9")]
    public void TestDivideByZero()
    {
      Formula f = new Formula("5/0");
      Assert.IsInstanceOfType(f.Evaluate(s => 0), typeof(FormulaError));
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("10")]
    public void TestDivideByZeroVars()
    {
      Formula f = new Formula("(5 + X1) / (X1 - 3)");
      Assert.IsInstanceOfType(f.Evaluate(s => 3), typeof(FormulaError));
    }


    // Tests of syntax errors detected by the constructor
    [TestMethod(), Timeout(2000)]
    [TestCategory("11")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestSingleOperator()
    {
      Formula f = new Formula("+");
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("12")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestExtraOperator()
    {
      Formula f = new Formula("2+5+");
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("13")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestExtraCloseParen()
    {
      Formula f = new Formula("2+5*7)");
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("14")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestExtraOpenParen()
    {
      Formula f = new Formula("((3+5*7)");
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("15")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestNoOperator()
    {
      Formula f = new Formula("5x");
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("16")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestNoOperator2()
    {
      Formula f = new Formula("5+5x");
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("17")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestNoOperator3()
    {
      Formula f = new Formula("5+7+(5)8");
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("18")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestNoOperator4()
    {
      Formula f = new Formula("5 5");
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("19")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestDoubleOperator()
    {
      Formula f = new Formula("5 + + 3");
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("20")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestEmpty()
    {
      Formula f = new Formula("");
    }

    // Some more complicated formula evaluations
    [TestMethod(), Timeout(2000)]
    [TestCategory("21")]
    public void TestComplex1()
    {
      Formula f = new Formula("y1*3-8/2+4*(8-9*2)/14*x7");
      Assert.AreEqual(5.14285714285714, (double)f.Evaluate(s => (s == "x7") ? 1 : 4), 1e-9);
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("22")]
    public void TestRightParens()
    {
      Formula f = new Formula("x1+(x2+(x3+(x4+(x5+x6))))");
      Assert.AreEqual(6, (double)f.Evaluate(s => 1), 1e-9);
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("23")]
    public void TestLeftParens()
    {
      Formula f = new Formula("((((x1+x2)+x3)+x4)+x5)+x6");
      Assert.AreEqual(12, (double)f.Evaluate(s => 2), 1e-9);
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("53")]
    public void TestRepeatedVar()
    {
      Formula f = new Formula("a4-a4*a4/a4");
      Assert.AreEqual(0, (double)f.Evaluate(s => 3), 1e-9);
    }

    // Test of the Equals method
    [TestMethod(), Timeout(2000)]
    [TestCategory("24")]
    public void TestEqualsBasic()
    {
      Formula f1 = new Formula("X1+X2");
      Formula f2 = new Formula("X1+X2");
      Assert.IsTrue(f1.Equals(f2));
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("25")]
    public void TestEqualsWhitespace()
    {
      Formula f1 = new Formula("X1+X2");
      Formula f2 = new Formula(" X1  +  X2   ");
      Assert.IsTrue(f1.Equals(f2));
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("26")]
    public void TestEqualsDouble()
    {
      Formula f1 = new Formula("2+X1*3.00");
      Formula f2 = new Formula("2.00+X1*3.0");
      Assert.IsTrue(f1.Equals(f2));
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("27")]
    public void TestEqualsComplex()
    {
      Formula f1 = new Formula("1e-2 + X5 + 17.00 * 19 ");
      Formula f2 = new Formula("   0.0100  +     X5+ 17 * 19.00000 ");
      Assert.IsTrue(f1.Equals(f2));
    }


    // Tests of == operator
    [TestMethod(), Timeout(2000)]
    [TestCategory("29")]
    public void TestEq()
    {
      Formula f1 = new Formula("2");
      Formula f2 = new Formula("2");
      Assert.IsTrue(f1 == f2);
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("30")]
    public void TestEqFalse()
    {
      Formula f1 = new Formula("2");
      Formula f2 = new Formula("5");
      Assert.IsFalse(f1 == f2);
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("31")]
    public void TestEqSameNumDiffString()
    {
      Formula f1 = new Formula("2.0");
      Formula f2 = new Formula("2");
      Assert.IsTrue(f1 == f2);
    }


    // Tests of != operator
    [TestMethod(), Timeout(2000)]
    [TestCategory("32")]
    public void TestNotEq()
    {
      Formula f1 = new Formula("2");
      Formula f2 = new Formula("2");
      Assert.IsFalse(f1 != f2);
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("33")]
    public void TestNotEqTrue()
    {
      Formula f1 = new Formula("2");
      Formula f2 = new Formula("5");
      Assert.IsTrue(f1 != f2);
    }


    // Test of ToString method
    [TestMethod(), Timeout(2000)]
    [TestCategory("34")]
    public void TestString()
    {
      Formula f = new Formula("2*5");
      Assert.IsTrue(f.Equals(new Formula(f.ToString())));
    }


    // Tests of GetHashCode method
    [TestMethod(), Timeout(2000)]
    [TestCategory("35")]
    public void TestHashCode()
    {
      Formula f1 = new Formula("2*5");
      Formula f2 = new Formula("2*5");
      Assert.IsTrue(f1.GetHashCode() == f2.GetHashCode());
    }

    // Technically the hashcodes could not be equal and still be valid,
    // extremely unlikely though. Check their implementation if this fails.
    [TestMethod(), Timeout(2000)]
    [TestCategory("36")]
    public void TestHashCodeFalse()
    {
      Formula f1 = new Formula("2*5");
      Formula f2 = new Formula("3/8*2+(7)");
      Assert.IsTrue(f1.GetHashCode() != f2.GetHashCode());
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("37")]
    public void TestHashCodeComplex()
    {
      Formula f1 = new Formula("2 * 5 + 4.00 - _x");
      Formula f2 = new Formula("2*5+4-_x");
      Assert.IsTrue(f1.GetHashCode() == f2.GetHashCode());
    }


    // Tests of GetVariables method
    [TestMethod(), Timeout(2000)]
    [TestCategory("38")]
    public void TestVarsNone()
    {
      Formula f = new Formula("2*5");
      Assert.IsFalse(f.GetVariables().GetEnumerator().MoveNext());
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("39")]
    public void TestVarsSimple()
    {
      Formula f = new Formula("2*X2");
      List<string> actual = new List<string>(f.GetVariables());
      HashSet<string> expected = new HashSet<string>() { "X2" };
      Assert.AreEqual(actual.Count, 1);
      Assert.IsTrue(expected.SetEquals(actual));
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("40")]
    public void TestVarsTwo()
    {
      Formula f = new Formula("2*X2+Y3");
      List<string> actual = new List<string>(f.GetVariables());
      HashSet<string> expected = new HashSet<string>() { "Y3", "X2" };
      Assert.AreEqual(actual.Count, 2);
      Assert.IsTrue(expected.SetEquals(actual));
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("41")]
    public void TestVarsDuplicate()
    {
      Formula f = new Formula("2*X2+X2");
      List<string> actual = new List<string>(f.GetVariables());
      HashSet<string> expected = new HashSet<string>() { "X2" };
      Assert.AreEqual(actual.Count, 1);
      Assert.IsTrue(expected.SetEquals(actual));
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("42")]
    public void TestVarsComplex()
    {
      Formula f = new Formula("X1+Y2*X3*Y2+Z7+X1/Z8");
      List<string> actual = new List<string>(f.GetVariables());
      HashSet<string> expected = new HashSet<string>() { "X1", "Y2", "X3", "Z7", "Z8" };
      Assert.AreEqual(actual.Count, 5);
      Assert.IsTrue(expected.SetEquals(actual));
    }

    // Tests to make sure there can be more than one formula at a time
    [TestMethod(), Timeout(2000)]
    [TestCategory("43")]
    public void TestMultipleFormulae()
    {
      Formula f1 = new Formula("2 + a1");
      Formula f2 = new Formula("3");
      Assert.AreEqual(2.0, f1.Evaluate(x => 0));
      Assert.AreEqual(3.0, f2.Evaluate(x => 0));
      Assert.IsFalse(new Formula(f1.ToString()) == new Formula(f2.ToString()));
      IEnumerator<string> f1Vars = f1.GetVariables().GetEnumerator();
      IEnumerator<string> f2Vars = f2.GetVariables().GetEnumerator();
      Assert.IsFalse(f2Vars.MoveNext());
      Assert.IsTrue(f1Vars.MoveNext());
    }

    // Repeat this test to increase its weight
    [TestMethod(), Timeout(2000)]
    [TestCategory("44")]
    public void TestMultipleFormulaeB()
    {
      TestMultipleFormulae();
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("45")]
    public void TestMultipleFormulaeC()
    {
      TestMultipleFormulae();
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("46")]
    public void TestMultipleFormulaeD()
    {
      TestMultipleFormulae();
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("47")]
    public void TestMultipleFormulaeE()
    {
      TestMultipleFormulae();
    }

    // Stress test for constructor
    [TestMethod(), Timeout(2000)]
    [TestCategory("48")]
    public void TestConstructor()
    {
      Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
    }

    // This test is repeated to increase its weight
    [TestMethod(), Timeout(2000)]
    [TestCategory("49")]
    public void TestConstructorB()
    {
      Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("50")]
    public void TestConstructorC()
    {
      Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
    }

    [TestMethod(), Timeout(2000)]
    [TestCategory("51")]
    public void TestConstructorD()
    {
      Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
    }

    // Stress test for constructor
    [TestMethod(), Timeout(2000)]
    [TestCategory("52")]
    public void TestConstructorE()
    {
      Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
    }
  
}
