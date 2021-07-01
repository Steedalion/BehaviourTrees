using System.Collections;
using System.Collections.Generic;
using BehaviourTree.BtreeTests;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Robber : MonoBehaviour
{
    public GameObject diamond, truck;

    private NavMeshAgent agent;

    private BehaviourTreeRoot root;
    public GameObject frontDoor, backDoor;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        root = new BehaviourTreeRoot("Robber");
        Sequence stealDiamond = new Sequence("Steal Diamond");

        ActionNode goToDiamond =
            new ActionNode("GoToDiamond", () =>
            {
                agent.SetDestination(diamond.transform.position);
                return Status.Success;
            });
        ActionNode reachedDestination = new ActionNode("atVan", () =>
        {
            if (agent.remainingDistance < 1)
            {
                return Status.Success;
            }

            return Status.Running;
        });


        ActionNode goToCar = new ActionNode("Go To Card", () =>
        {
            agent.SetDestination(truck.transform.position);
            return Status.Success;
        });
        ActionNode diamondDissapear = new ActionNode("diamondDissapear", () =>
        {
            diamond.SetActive(false);
            return Status.Success;
        });

        Selector OpenDoor = new Selector("findOpenDoor");
        Sequence CheckFrontDoor = new Sequence("Check front");
        ActionNode GoToFrontDoor = new ActionNode("gtfront", goToFrontDoor);
        CheckFrontDoor.AddChild(GoToFrontDoor);
        CheckFrontDoor.AddChild(reachedDestination);
        OpenDoor.AddChild(CheckFrontDoor);


        Sequence CheckBackDoor = new Sequence("Check back");
        ActionNode GoToBackDoor = new ActionNode("gtBack", goToBackDoor);
        CheckBackDoor.AddChild(GoToBackDoor);
        CheckBackDoor.AddChild(reachedDestination);
        OpenDoor.AddChild(CheckBackDoor);


        stealDiamond.AddChild(goToDiamond);
        stealDiamond.AddChild(reachedDestination);
        stealDiamond.AddChild(diamondDissapear);

        Sequence getAway = new Sequence("get Away");
        getAway.AddChild(goToCar);
        getAway.AddChild(reachedDestination);
        getAway.AddChild(goToDiamond);


        root.AddChild(OpenDoor);
        root.AddChild(stealDiamond);
        root.AddChild(getAway);
        Debug.Log(root.TreeString());
    }

    private Status goToBackDoor()
    {
        return GoToDoor(backDoor);
    }

    private Status goToFrontDoor()
    {
        return GoToDoor(frontDoor);
    }

    private Status GoToDoor(GameObject frontDoor)
    {
        GoTo(frontDoor);
        if (frontDoor.GetComponent<Lock>().isLocked)
        {
            return Status.Failure;
        }

        frontDoor.SetActive(false);
        return Status.Success;
    }

    private Status GoTo(GameObject destinationObject)
    {
        agent.SetDestination(destinationObject.transform.position);
        return Status.Success;
    }

    // Update is called once per frame
    void Update()
    {
        root.Process();
    }
}