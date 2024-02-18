using SS;
using SpreadsheetUtilities;
//checking if branch works
namespace SS;
/// <summary>
/// This is the spreadsheet class. THis is meant to create a new spreadsheet and check for validity and set cells to values.
/// Created by: Nicholas Zamani
/// Date: Feb 11, 2024
/// </summary>
public class Spreadsheet : AbstractSpreadsheet
{
    private Dictionary<string, object> spreadsheet;
    private DependencyGraph Depends;
    private Formula formulaVar;
    private List<string> allDependents;
    private string Version;

    /// <summary>
    /// The Spreadsheet constructor that creates a new empty spreadsheet.
    /// Makes the version the default version.
    /// </summary>
    public Spreadsheet()
    {
        //zero param constructor that creates an empty spreadsheet
        spreadsheet = new Dictionary<string, object>();
        Version = "Default";
    }
    public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version)
    {
        this.IsValid = isValid;
        this.Normalize = normalize;
        this.Version = version;
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
        if (!CheckCellName(name))
            throw new InvalidNameException();
        if (name == null)
            throw new InvalidNameException();
        if (!spreadsheet.ContainsKey(name))
            return "";
        return spreadsheet[name];
    }
    protected override IList<String> SetCellContents(String name, double number)
    {
        SetCells(name, number);
        Depends = new DependencyGraph();
        dependents();
        allDependents = new List<string>();
        allDependents.Add(name);
        GetAllDependents(name);
        return allDependents;
    }
    protected override IList<String> SetCellContents(String name, String text)
    {
        var temp = GetCellContents(name);
        try
        {
            SetCells(name, text);
            GetCellsToRecalculate(name);
        }
        catch (CircularException e)
        {
            SetCells(name, temp);
            throw e;
        }
        Depends = new DependencyGraph();
        dependents();
        allDependents = new List<string>();
        allDependents.Add(name);
        GetAllDependents(name);
        return allDependents;
    }
    protected override IList<String> SetCellContents(String name, Formula formula)
    {
        var temp = GetCellContents(name);
        try
        {
            SetCells(name, formula);
            GetCellsToRecalculate(name);
        }
        catch (CircularException e)
        {
            SetCells(name, temp);
            throw e;
        }
        Depends = new DependencyGraph();
        dependents();
        allDependents = new List<string>();
        allDependents.Add(name);
        GetAllDependents(name);
        return allDependents;
    }
    protected override IEnumerable<String> GetDirectDependents(String name)
    {
        if (name == null)
            throw new ArgumentNullException();
        if (name == null || !spreadsheet.ContainsKey(name))
            throw new InvalidNameException();
        //check if each value is a formula
        Depends = new DependencyGraph();
        dependents();
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

        if (CheckCellName(name))
        {
            if (spreadsheet.ContainsKey(name))
                spreadsheet[name] = item;
            else
            {
                spreadsheet.Add(name, item);
            }
        }
        else
        {
            throw new InvalidNameException();
        }
        

    }
    /// <summary>
    /// Checks to see if the cell name is a valid cell name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private bool CheckCellName(string name)
    {
        char[] characters = name.ToCharArray();
        if (characters[0] != '_' && !Char.IsLetter(characters[0]))
            return false;
        for (int i = 1; i < characters.Length; i++)
        {
            if (!Char.IsLetter(characters[i]) && !Char.IsDigit(characters[i]) && characters[i] != '_')
                return false;
        }
        return true;
    }
/// <summary>
/// adds all of the dependents into a dictionary.
/// </summary>
    private void dependents()
    {
        foreach (var item in spreadsheet)
        {
            if (item.Value is Formula)
            {
                formulaVar = (Formula)item.Value;
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
    }
/// <summary>
/// This is a recursive method that is used to get all of the dependents for a given cell. These can be direct
/// or indirect.
/// </summary>
/// <param name="name"></param>
    private void GetAllDependents(string name)
    {
        if (Depends.GetDependents(name).Count() == 0)
            return;
        foreach (var item in Depends.GetDependents(name))
        {
            allDependents.Add(item);
            GetAllDependents(item);
        }
    }

    public override IList<String> SetContentsOfCell(String name, String content)
    {
        //TODO
        return null;
    }
    //TODO
    public override bool Changed { get; protected set; }

    public override string GetSavedVersion(String filename)
    {
        //TODO
        return null;
    }

    public override void Save(String filename)
    {
        //TODO
    }
    public override string GetXML()
    {
        //TODO
        return null;
    }

    public override object GetCellValue(String name)
    {
        //TODO
        return null;
    }
}