
using FormulaEvaluator;
using System;

namespace TestProject1;


public class Tests
{

  [Test]
    public void TestSingleNumber()
    {
      Assert.AreEqual(5, Evaluator.Evaluate("5", s => 0));
    }
  [Test]
    public void TestSingleVariable()
    {
      Assert.AreEqual(13, Evaluator.Evaluate("X5", s => 13));
    }
  [Test]
    public void TestAddition()
    {
      Assert.AreEqual(8, Evaluator.Evaluate("5+3", s => 0));
    }
  [Test]
    public void TestSubtraction()
    {
      Assert.AreEqual(8, Evaluator.Evaluate("18-10", s => 0));
    }
  [Test]
    public void TestMultiplication()
    {
      Assert.AreEqual(8, Evaluator.Evaluate("2*4", s => 0));
    }
  [Test]
    public void TestDivision()
    {
      Assert.AreEqual(8, Evaluator.Evaluate("16/2", s => 0));
    }
  [Test]
    public void TestArithmeticWithVariable()
    {
      Assert.AreEqual(6, Evaluator.Evaluate("2+X1", s => 4));
    }
  [Test]
    public void TestUnknownVariable()
    {
      Evaluator.Evaluate("2+X1", s => { throw new ArgumentException("Unknown variable"); });
    }
  [Test]
    public void TestLeftToRight()
    {
      Assert.AreEqual(15, Evaluator.Evaluate("2*6+3", s => 0));
    }
  [Test]
    public void TestOrderOperations()
    {
      Assert.AreEqual(20, Evaluator.Evaluate("2+6*3", s => 0));
    }
  [Test]
    public void TestParenthesesTimes()
    {
      Assert.AreEqual(24, Evaluator.Evaluate("(2+6)*3", s => 0));
    }
  [Test]
    public void TestTimesParentheses()
    {
      Assert.AreEqual(16, Evaluator.Evaluate("2*(3+5)", s => 0));
    }
  [Test]
    public void TestPlusParentheses()
    {
      Assert.AreEqual(10, Evaluator.Evaluate("2+(3+5)", s => 0));
    }
  [Test]
    public void TestPlusComplex()
    {
      Assert.AreEqual(50, Evaluator.Evaluate("2+(3+5*9)", s => 0));
    }
  [Test]
    public void TestOperatorAfterParens()
    {
      Assert.AreEqual(0, Evaluator.Evaluate("(1*1)-2/2", s => 0));
    }
  [Test]
    public void TestComplexTimesParentheses()
    {
      Assert.AreEqual(26, Evaluator.Evaluate("2+3*(3+5)", s => 0));
    }
  [Test]
    public void TestComplexAndParentheses()
    {
      Assert.AreEqual(194, Evaluator.Evaluate("2+3*5+(3+4*8)*5+2", s => 0));
    }
  [Test]
    public void TestDivideByZero()
    {
      Evaluator.Evaluate("5/0", s => 0);
    }
  [Test]
    public void TestSingleOperator()
    {
      Evaluator.Evaluate("+", s => 0);
    }
  [Test]
    public void TestExtraOperator()
    {
      Evaluator.Evaluate("2+5+", s => 0);
    }
  [Test]
    public void TestExtraParentheses()
    {
      Evaluator.Evaluate("2+5*7)", s => 0);
    }
  [Test]
    public void TestInvalidVariable()
    {
      Evaluator.Evaluate("xx", s => 0);
    }
  [Test]
    public void TestPlusInvalidVariable()
    {
      Evaluator.Evaluate("5+xx", s => 0);
    }
  [Test]
    public void TestParensNoOperator()
    {
      Evaluator.Evaluate("5+7+(5)8", s => 0);
    }
  [Test]
    public void TestEmpty()
    {
      Evaluator.Evaluate("", s => 0);
    }
  [Test]
    public void TestComplexMultiVar()
    {
      Assert.AreEqual(6, Evaluator.Evaluate("y1*3-8/2+4*(8-9*2)/14*x7", s => (s == "x7") ? 1 : 4));
    }
  [Test]
    public void TestComplexNestedParensRight()
    {
      Assert.AreEqual(6, Evaluator.Evaluate("x1+(x2+(x3+(x4+(x5+x6))))", s => 1));
    }
  [Test]
    public void TestComplexNestedParensLeft()
    {
      Assert.AreEqual(12, Evaluator.Evaluate("((((x1+x2)+x3)+x4)+x5)+x6", s => 2));
    }
  [Test]
    public void TestRepeatedVar()
    {
      Assert.AreEqual(0, Evaluator.Evaluate("a4-a4*a4/a4", s => 3));
    }
  [Test]
    public void TestClearStacks()
    {
        //Test if code doesn't clear stacks between evaluations
        Assert.AreEqual( 15, Evaluator.Evaluate( "2*6+3", s => 0 ) );
        Assert.AreEqual( 15, Evaluator.Evaluate( "2*6+3", s => 0 ) );
    }
    
}