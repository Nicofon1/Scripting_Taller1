using System.Collections.Generic;
using UnityEngine;

public class TreeClass
{

}



public abstract class Node
{
    protected List<Node> children = new List<Node>();


    //public void AddChild(Node node)
    //{
    //    children.Add(node);
    //}

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
       // La única misión del Root es ejecutar a su único hijo.
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

    // La condición específica que debe cumplirse
    public abstract bool Check(GameObject agent);

    public override bool Execute(GameObject agent)
    {
        // Si la condición se cumple, intenta ejecutar a los hijos.
        if (Check(agent))
        {
            foreach (var child in children)
            {
                // Si un hijo tiene éxito, todo el selector tiene éxito. (Lógica OR)
                if (child.Execute(agent))
                {
                    return true;
                }
            }
        }
        // Falla si la condición 'Check' es falsa, o si ninguno de los hijos tuvo éxito.
        return false;
    }

}
