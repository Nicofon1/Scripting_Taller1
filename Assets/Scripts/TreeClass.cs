using System.Collections.Generic;
using UnityEngine;


public abstract class Node
{
    protected List<Node> children = new List<Node>();

    public abstract bool Execute(GameObject agent);

}
public abstract class Task : Node
{

}
public class Root : Node
{
  public Root(Node child)
   {
       children.Add(child);
   }

   public override bool Execute(GameObject agent)
   {
       if (children.Count > 0)
       {
           return children[0].Execute(agent);
       }
       return false;
   }

}
public class Sequence : Composite
{
  public Sequence(List<Node> childrenNodes)
    {
        this.children = childrenNodes;
    }

  public override bool Execute(GameObject agent)
  {
      foreach (var child in children)
      {

          if (!child.Execute(agent))
          {
              return false;
          }
      }

      return true;
  }

}
public abstract class Composite : Node
{

}
public abstract class Selector : Composite
{
  public Selector(List<Node> childrenNodes)
    {
        this.children = childrenNodes;
    }

    public abstract bool Check(GameObject agent);

    public override bool Execute(GameObject agent)
    {
        if (Check(agent))
        {
            foreach (var child in children)
            {
                if (child.Execute(agent))
                {
                    return true;
                }
            }
        }
        return false;
    }

}
