using System.Collections.Generic;
using UnityEngine;

public class CheckDistanceSelector : Selector
{
    private Transform target;
    private float validDistance;

    public CheckDistanceSelector(Transform target, float validDistance, List<Node> children) : base(children)
    {
        this.target = target;
        this.validDistance = validDistance;
    }

    public override bool Check(GameObject agent)
    {
        float distance = Vector3.Distance(agent.transform.position, target.position);
        Debug.Log($"Distancia al objetivo: {distance}");
        return distance <= validDistance;
    }
}

public class MoveTask : Task
{
    private AI_Controller controller;

    public MoveTask(AI_Controller controller)
    {
        this.controller = controller;
    }

    public override bool Execute(GameObject agent)
    {
        controller.SetChaseStatus(true);
        return true;
    }
}

public class JumpTask : Task
{
    private Rigidbody rigidbody;
    private float jumpForce;
    private AI_Controller controller; 

    public JumpTask(Rigidbody rigidbody, float jumpForce, AI_Controller controller)
    {
        this.rigidbody = rigidbody;
        this.jumpForce = jumpForce;
        this.controller = controller;
    }

    public override bool Execute(GameObject agent)
    {
        controller.SetChaseStatus(false);

        if (rigidbody != null && rigidbody.linearVelocity.y == 0)
        {
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log("¡Saltando!");
            return true; 
        }
        return false; 
    }
}

public class WaitTask : Task
{
    private AI_Controller controller;

    public WaitTask(AI_Controller controller)
    {
        this.controller = controller;
    }

    public override bool Execute(GameObject agent)
    {
        controller.StartWait();
        return true;
    }
}

public class SimpleSelector : Selector
{
    public SimpleSelector(List<Node> children) : base(children) 
    {

    }

    public override bool Check(GameObject agent)
    {
        return true;
    }
}
