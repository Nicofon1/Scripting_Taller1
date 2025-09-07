using System.Collections.Generic;
using UnityEngine;

public class AI_Controller : MonoBehaviour
{
    public Transform targetObject;
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float validDistance = 15f;
    public float waitTime = 2f;

    private Root behaviorTree;
    private bool isWaiting = false;
    private float waitTimer = 0f;
    private bool isChasing = false; 
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        var moveNode = new MoveTask(this);
        var waitNode = new WaitTask(this);
        var jumpNode = new JumpTask(rb, jumpForce, this); 

        var checkDistanceNode = new CheckDistanceSelector(
            targetObject,
            validDistance,
            new List<Node> { moveNode }
        );

        var mainSelector = new SimpleSelector(new List<Node>
        {
            checkDistanceNode,
            jumpNode
        });

        var mainSequence = new Sequence(new List<Node>
        {
            mainSelector,
            waitNode
        });

        behaviorTree = new Root(mainSequence);
    }

    void Update()
    {
        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                isWaiting = false;
            }
            return; 
        }

        if (behaviorTree != null)
        {
            behaviorTree.Execute(this.gameObject);
        }

        if (isChasing)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetObject.position,
                moveSpeed * Time.deltaTime);
        }
    }


    public void StartWait()
    {
        if (!isChasing || Vector3.Distance(transform.position, targetObject.position)<0.5)
        {
            isWaiting = true;
            waitTimer = 0f;
        }
    }

    public void SetChaseStatus(bool isChasing)
    {
        this.isChasing = isChasing;
    }
}