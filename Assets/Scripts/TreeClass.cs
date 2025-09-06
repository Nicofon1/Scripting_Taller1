using System.Collections.Generic;
using UnityEngine;

public class TreeClass
{

}
public abstract class Node
{
    List<Node> children = new List<Node>();


}
public class Task : Node
{

}
public class Root : Node
{

}
public class Sequence : Root
{

}
public class Composite : Node
{

}
public class Selector : Composite
{

}















