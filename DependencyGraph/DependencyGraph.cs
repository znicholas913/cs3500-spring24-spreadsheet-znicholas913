// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta 
//               (Clarified meaning of dependent and dependee.)
//               (Clarified names in solution/project structure.)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities
{

  /// <summary>
  /// (s1,t1) is an ordered pair of strings
  /// t1 depends on s1; s1 must be evaluated before t1
  /// 
  /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
  /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
  /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
  /// set, and the element is already in the set, the set remains unchanged.
  /// 
  /// Given a DependencyGraph DG:
  /// 
  ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
  ///        (The set of things that depend on s)    
  ///        
  ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
  ///        (The set of things that s depends on) 
  //
  // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
  //     dependents("a") = {"b", "c"}
  //     dependents("b") = {"d"}
  //     dependents("c") = {}
  //     dependents("d") = {"d"}
  //     dependees("a") = {}
  //     dependees("b") = {"a"}
  //     dependees("c") = {"a"}
  //     dependees("d") = {"b", "d"}
  /// </summary>
  public class DependencyGraph
  {
    
    /// <summary>
    /// Creates an empty DependencyGraph.
    /// </summary>
    public DependencyGraph()
    {
    }
    //Create a hash map with strings and a list of strings.
    Dictionary<string, List<string>> depend = new Dictionary<String, List<string>>();
    private int size = 0;

    /// <summary>
    /// The number of ordered pairs in the DependencyGraph.
    /// </summary>
    public int Size
    {
      get
      {
        return size;
      }
    }


    /// <summary>
    /// The size of dependees(s).
    /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
    /// invoke it like this:
    /// dg["a"]
    /// It should return the size of dependees("a")
    /// </summary>
    public int this[string s]
    {
      //return the amount of dependees
      get { return depend[s].Count; }
    }


    /// <summary>
    /// Reports whether dependents(s) is non-empty.
    /// </summary>
    public bool HasDependents(string s)
    {
      //
      if (depend.ContainsKey(s))
      {
        if (depend[s].Count > 0)
          return true;
      }
      return false;
    }


    /// <summary>
    /// Reports whether dependees(s) is non-empty.
    /// </summary>
    public bool HasDependees(string s)
    {
      //check to see if the dictionary contains a key.
      if (depend.ContainsKey(s))
      {
        return true;
      }
      return false;
    }


    /// <summary>
    /// Enumerates dependents(s).
    /// </summary>
    public IEnumerable<string> GetDependents(string s)
    {
      //must be O(1) probably needs to be fixed!!!!
      //it there are dependents
      if (HasDependents(s))
      { 
        return depend[s];
      }
      return null;
    }

    /// <summary>
    /// Enumerates dependees(s).
    /// </summary>
    public IEnumerable<string> GetDependees(string s)
    {
      IEnumerable<string> enumItems = depend.Keys;
      return enumItems;
    }


    /// <summary>
    /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
    /// 
    /// <para>This should be thought of as:</para>   
    /// 
    ///   t depends on s
    ///
    /// </summary>
    /// <param name="s"> s must be evaluated first. T depends on S</param>
    /// <param name="t"> t cannot be evaluated until s is</param>        ///
    
    public void AddDependency(string s, string t)
    {
      //checks to see if the dependee is in the dictionary
      bool temp = depend.ContainsKey(s);
      //if the dependee is not in the dictionary it will add a new arraylist to that key.
      if (!temp)
      {
        List<string> dependsOn = new List<string>();
        dependsOn.Add(t);
        depend.Add(s, dependsOn);
      }
      else
      {
        //add the dependent to the list at the key.
        depend[s].Add(t);
      }
      size++;
    }


    /// <summary>
    /// Removes the ordered pair (s,t), if it exists
    /// </summary>
    /// <param name="s"></param>
    /// <param name="t"></param>
    public void RemoveDependency(string s, string t)
    {
      //checks to see if the dictionary contains the key
      if (depend.ContainsKey(s))
      {
        //checks to see if the value array contains the dependent.
        if (depend[s].Contains(t))
        {
          //removes the dependent
          depend[s].Remove(t);
        }
        //reduces the size. 
        size--;
      }
    }


    /// <summary>
    /// Removes all existing ordered pairs of the form (s,r).  Then, for each
    /// t in newDependents, adds the ordered pair (s,t).
    /// </summary>
    public void ReplaceDependents(string s, IEnumerable<string> newDependents)
    {
      depend.Remove(s);
      List<string> asList = newDependents.ToList();
      depend.Add(s, asList);
    }


    /// <summary>
    /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
    /// t in newDependees, adds the ordered pair (t,s).
    /// </summary>
    public void ReplaceDependees(string s, IEnumerable<string> newDependees)
    {
      List<string> asList = newDependees.ToList();
      //which ever keys contain a value s, get rid of the s.
      foreach (var key in GetDependees(s))
      {
          depend[key].Remove(s);
      }
      //need to create a new key with s in the list or add it to an already created list.
      foreach (var t in newDependees)
      {
        AddDependency(t,s);
      }
    }
  }

}
