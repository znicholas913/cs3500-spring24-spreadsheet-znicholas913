using SS;
using SpreadsheetUtilities;

namespace SS;

public class Spreadsheet : AbstractSpreadsheet
{
    private Dictionary<string, object> spreadsheet;
    private HashSet<string> keys = new HashSet<string>();
    private DependencyGraph Depends = new DependencyGraph();
    private Formula formulaVar;
    /// <summary>
    /// The Spreadsheet constructor that creates a new empty spreadsheet.
    /// </summary>
    public Spreadsheet()
    {
        //zero param constructor that creates an empty spreadsheet
        spreadsheet = new Dictionary<string, object>();
    }
    public override IEnumerable<string> GetNamesOfAllNonemptyCells()
    {
        List<string> nonEmptyCells = new List<string>();
        foreach (var index in spreadsheet)
        {
            if (index.Value == "")
            {
                continue;
            }
            else
            {
                nonEmptyCells.Add(index.Key);
            }
        }
        return nonEmptyCells;
    }
    public override object GetCellContents(String name)
    {
        if (name == null || !spreadsheet.ContainsKey(name))
            throw new InvalidNameException();
        return spreadsheet[name];
    }
    public override ISet<String> SetCellContents(String name, double number)
    {
        SetCells(name, number);
        keys.Add(name);
        return keys;
    }
    public override ISet<String> SetCellContents(String name, String text)
    {
        SetCells(name, text);
        keys.Add(name);
        return keys;
    }
    public override ISet<String> SetCellContents(String name, Formula formula)
    {
        SetCells(name, formula);
        keys.Add(name);
        return keys;
    }
    protected override IEnumerable<String> GetDirectDependents(String name)
    {
        if (name == null)
            throw new ArgumentNullException("The name is null.");
        if (name == null || !spreadsheet.ContainsKey(name))
            throw new InvalidNameException();
        //check if each value is a formula 
        foreach (var item in spreadsheet)
        {
            if (item.Value is Formula)
            {
                formulaVar = (Formula) item.Value;
                //get the variables
                List<string> vars = formulaVar.GetVariables().ToList();
                //if the variable is the same as the string then add it
                foreach (var variable in vars)
                {
                    //every item.key is dependent on each of the variables inside of it.
                    Depends.AddDependency(variable, item.Key);
                }
            }
            
        }
            //check to see if the formula contains the variables.
            //make a helper method to put them into a dependency list.
            //add the variables to the dependency list
            //return it
            
        //check if that string contains tne string that is being passed in
        //make a dependency graph take in a formula
        return Depends.GetDependents(name);
    }
    /// <summary>
    /// This is a helper method that is meant to set the value of a cell. If the cell doesn't exist, it will add it
    /// and if it does exist then it will just set the value to the item.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="item"></param>
    private void SetCells(string name, object item)
    {
        
        if (spreadsheet.ContainsKey(name))
            spreadsheet[name] = item;
        else
        {
            spreadsheet.Add(name, item);
        }

    }
    // /// <summary>
    // /// This is only used to test getDirectDependents.
    // /// </summary>
    // /// <param name="name"></param>
    // /// <returns></returns>
    // public List<string> forTestsOnly(string name)
    // {
    //     return GetDirectDependents(name).ToList();
    // }
}