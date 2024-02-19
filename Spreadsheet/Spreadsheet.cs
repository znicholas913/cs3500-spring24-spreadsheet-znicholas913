using System.Text;
using System.Xml;
using System.Xml.Linq;
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
    private Func<string, bool> isValid;
    private Func<string, string> normalize;
    private bool changeFlag = false;
    

    /// <summary>
    /// The Spreadsheet constructor that creates a new empty spreadsheet.
    /// Makes the version the default version.
    /// </summary>
    public Spreadsheet() : this(s => true, s => s, "Default")
    {
        this.normalize = s => s;
        this.isValid = s => true;
        this.Version = "Default";
        spreadsheet = new Dictionary<string, object>();
    }
    public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string Version) : 
        base(isValid, normalize, Version)
    {
        this.normalize = normalize;
        this.isValid = isValid;
        this.Version = Version;
        spreadsheet = new Dictionary<string, object>();
    }
    public Spreadsheet(string filename, Func<string, bool> isValid, Func<string, string> normalize, string Version) : 
        base(isValid, normalize, Version)
    {
        //TODO
        this.normalize = normalize;
        this.isValid = isValid;
        this.Version = Version;
        spreadsheet = new Dictionary<string, object>();
        //for each item in the spreadsheet we have to put each item into the appropriate cell.
        //use try catch so it can throw an error if anything goes wrong
        using (XmlReader reader = new XmlTextReader(filename))
        {
            string name = null, value = null;
            int i = 0;
            
            reader.MoveToContent();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == "name")
                    {
                        XElement el = XNode.ReadFrom(reader) as XElement;
                        name = el.ToString();
                        i = 1;
                    }
                    if (reader.Name == "content")
                    {
                        XElement el = XNode.ReadFrom(reader) as XElement;
                        value = el.ToString();
                        i = 2;
                    }
                    if (i == 2)
                    {
                        SetCellContents(name, value);
                        i = 0;
                    }
                }
            }
            
        }
        
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
        changeFlag = true;
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
        changeFlag = true;
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
        changeFlag = true;
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
        //if the content is a double then we set the cell to that double and return all that depend on that cell.
        if (double.TryParse(content, out double d))
        {
            SetCells(name, d);
            return GetDirectDependents(name).ToList();
        }
        //turn the string into a character array.
        char[] charArr = content.ToCharArray();
        //if the first character of content is = then we check the three steps.
        if (charArr[0] == '=')
        {
            //removes the first character from the string.
            content = content.Substring(1);
            //creates a new formula from that string. Will throw the correct error if the formula is not valid.
            Formula contents = new Formula(content);
            //sets the cell to the formula. Will check for the circular exception and throw if needed.
            SetCellContents(name, contents);
            return GetDirectDependents(name).ToList();
        }
        //if none of the other statements pass, then it will set the cell to the content as a string.
        SetCellContents(name, content);
        return GetDirectDependents(name).ToList();
    }
    
    public override bool Changed
    {
        get
        {
            return changeFlag;
        }
        protected set
        {
            if (changeFlag) Changed = true;
            else Changed = false;
        }
    }

    public override string GetSavedVersion(String filename)
    {
        try
        {
            XmlReader reader = XmlReader.Create(new StreamReader(filename, Encoding.GetEncoding("UTF-16")));
            while (reader.Read())
            {
                return reader.GetAttribute("version");
            }
        }
        catch (Exception e)
        {
            throw new SpreadsheetReadWriteException("There was a problem with the file.");
        }

        return null;
    }


    public override void Save(String filename)
    {
        //uses the getxml method to write the entire string to a file.
        if (Changed)
        {
            File.WriteAllText(filename, GetXML());
            changeFlag = false;
        }
        
    }
    public override string GetXML()
    {
        //loops through each item in the spreadsheet and puts them into an xml formatted string.
        try
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";

            StringBuilder sb = new StringBuilder();
            using(XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                // starts the string
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", Version);
                //loop through each cell in the spreadsheet.
                foreach (var item in spreadsheet)
                {
                    //creates the cell
                    writer.WriteStartElement("cell");
                    //creates and sets the name
                    writer.WriteElementString("name", item.Key);
                    //if the value is a formula then we add the equals sign when content is created
                    if (item.Value is Formula)
                        writer.WriteElementString("content", "=" + item.Value.ToString());
                    //otherwise we create the content and set the value to it as a string.
                    else 
                        writer.WriteElementString("content", item.Value.ToString());
                    writer.WriteEndElement();
                }
                //close the whole spreadsheet.
                writer.WriteEndElement();
                sb.AppendLine(writer.ToString());
            }
            return sb.ToString();
        }
        catch (Exception e)
        {
            throw new SpreadsheetReadWriteException("There was a problem with the file.");
        }
    }
    public override object GetCellValue(String name)
    {
        if (!CheckCellName(name))
            throw new InvalidNameException();
        if (spreadsheet[name] is string)
            return spreadsheet[name];
        if (spreadsheet[name] is double)
            return spreadsheet[name];
        if (spreadsheet[name] is Formula)
        {
            Formula form = (Formula)spreadsheet[name];
            //figure out what lookup is
            return form.Evaluate(Lookup);
        }
        return null;
    }

    private double Lookup(string name)
    {
        if (spreadsheet[name] is double)
            return (double)spreadsheet[name];
        else
        {
            throw new ArgumentException();
        }
    }
    
    
}