using SS;
using SpreadsheetUtilities;

namespace SS;

[TestClass]
public class SpreadsheetTests
{
    Spreadsheet tester = new Spreadsheet();
    /// <summary>
    ///Test to see if the get Names of all empty cells method returns an empty list when
    /// there are no cells with anything in them.
    /// </summary>
    [TestMethod]
    public void TestGetNamesOfNonEmptyCellsWithEmptySpreadsheet()
    {
        tester.SetCellContents("A1", "");
        tester.SetCellContents("A2", "");
        tester.SetCellContents("A3", "");
        tester.SetCellContents("A4", "");
        tester.SetCellContents("A5", "");
        Assert.AreEqual(0,tester.GetNamesOfAllNonemptyCells().Count());
    }
    /// <summary>
    /// This tests to see if it correctly picks out the cells that have something in them.
    /// </summary>
    [TestMethod]
    public void TestGetNamesOfNonEmptyCellContents()
    {
        tester.SetCellContents("A1", "");
        tester.SetCellContents("A2", "");
        tester.SetCellContents("A3", "");
        tester.SetCellContents("A4", "Ants");
        tester.SetCellContents("A5", 5);
        Assert.AreEqual("A4",tester.GetNamesOfAllNonemptyCells().ToList()[0]);
        Assert.AreEqual("A5",tester.GetNamesOfAllNonemptyCells().ToList()[1]);
    }
/// <summary>
///Tests to see if get cell contents works with all 3 of the types accepted.
/// </summary>
    [TestMethod]
    public void TestGetCellContents()
    {
        Formula testing = new Formula("A1 + 5");
        tester.SetCellContents("A1", 5);
        tester.SetCellContents("A2", testing);
        tester.SetCellContents("A3", "hi");
        Assert.AreEqual("hi",tester.GetCellContents("A3"));
        Assert.AreEqual(testing,tester.GetCellContents("A2"));
        Assert.AreEqual(5.0 ,tester.GetCellContents("A1"));
    }
/// <summary>
/// Checks to make sure getcellcontents throws when an invalid name is passed in.
/// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void TestGetCellContentsWithInvalidName()
    {
        tester.SetCellContents("A1", "");
        tester.SetCellContents("A2", "");
        tester.SetCellContents("A3", "");
        tester.SetCellContents("A4", "Ants");
        tester.SetCellContents("A5", 5);
        tester.GetCellContents("A6");
    }
    /// <summary>
    /// checks to make sure that get cell contents throws when a null value is passed in.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidNameException))]
    public void TestGetCellContentsWithNull()
    {
        tester.SetCellContents("A1", "");
        tester.SetCellContents("A2", "");
        tester.SetCellContents("A3", "");
        tester.SetCellContents("A4", "Ants");
        tester.SetCellContents("A5", 5);
        tester.GetCellContents(null);
    }

    [TestMethod]
    [ExpectedException(typeof(CircularException))]
    public void TestCircularException()
    {
        Formula test1 = new Formula("B1");
        Formula test2 = new Formula("A1*A1");
        Formula test3 = new Formula("B1+A1");
        Formula test4 = new Formula("B1-C1");
        tester.SetCellContents("A1", test1);
        tester.SetCellContents("B1", test2);
        tester.SetCellContents("C1", test3);
        tester.SetCellContents("D1", test4);
    }

    // [TestMethod]
    // public void TestGetDirectDependents()
    // {
    //     Formula test1 = new Formula("3");
    //     Formula test2 = new Formula("A1*A1");
    //     Formula test3 = new Formula("B1+A1");
    //     Formula test4 = new Formula("B1-C1");
    //     tester.SetCellContents("A1", test1);
    //     tester.SetCellContents("B1", test2);
    //     tester.SetCellContents("C1", test3);
    //     tester.SetCellContents("D1", test4);
    //     Assert.AreEqual(2,tester.forTestsOnly("A1").Count);
    //     Assert.AreEqual("B1",tester.forTestsOnly("A1")[0]);
    //     Assert.AreEqual("C1",tester.forTestsOnly("A1")[1]);
    // }
}