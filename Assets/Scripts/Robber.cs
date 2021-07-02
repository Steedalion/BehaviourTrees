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
    bool nearDestination => agent.remainingDistance < 1;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        root = new BehaviourTreeRoot("Robber");
        Sequence stealDiamond = new Sequence("Steal Diamond");

        ActionNode goToDiamond =
            new ActionNode("GoToDiamond", () => { return GoTo(diamond); });
        ActionNode reachedDestination =
            new ActionNode("arrive", () => { return nearDestination ? Status.Success : Status.Running; });


        ActionNode goToCar = new ActionNode("Go To Card", () => { return GoTo(truck); });

        ActionNode diamondDissapear = new ActionNode("diamondDissapear", () =>
        {
            diamond.SetActive(false);
            return Status.Success;
        });

        Selector OpenADoor = new Selector("findOpenDoor");
        Sequence CheckFrontDoor = new Sequence("Check front");
        ActionNode GoToFrontDoor = new ActionNode("gtfront", goToFrontDoor);
        Node OpenDoorFrontDoor = new ActionNode("OpenFdoor", OpenFdoor);

        CheckFrontDoor.AddChild(GoToFrontDoor);
        CheckFrontDoor.AddChild(reachedDestination);
        Node fdoorNotLocked = new Condition("fdoorOpen?", () =>
        {
            bool isLocked = frontDoor.GetComponent<Lock>().isLocked;
            return !isLocked;
        });
        CheckFrontDoor.AddChild(fdoorNotLocked);
        CheckFrontDoor.AddChild(OpenDoorFrontDoor);
        OpenADoor.AddChild(CheckFrontDoor);


        Sequence CheckBackDoor = new Sequence("Check back");
        ActionNode GoToBackDoor = new ActionNode("gtBack", goToBackDoor);
        CheckBackDoor.AddChild(GoToBackDoor);
        CheckBackDoor.AddChild(reachedDestination);
        Node bdoorNotLocked = new Condition("bdoorOpen?", () =>
        {
            return !backDoor.GetComponent<Lock>().isLocked;
        });

    CheckBackDoor.AddChild(bdoorNotLocked);
        Node openBackdoor = new ActionNode("openbd", () =>
        {
            backDoor.SetActive(false);
            return Status.Success;
        });
        CheckBackDoor.AddChild(openBackdoor);
        OpenADoor.AddChild(CheckBackDoor);


        stealDiamond.AddChild(goToDiamond);
        stealDiamond.AddChild(reachedDestination);
        stealDiamond.AddChild(diamondDissapear);

        Sequence getAway = new Sequence("get Away");
        getAway.AddChild(goToCar);
        getAway.AddChild(reachedDestination);
        getAway.AddChild(goToDiamond);


        root.AddChild(OpenADoor);
        root.AddChild(stealDiamond);
        root.AddChild(getAway);
        Debug.Log(root.TreeString());
    }

    private Status OpenFdoor()
    {
        OpenDoor(frontDoor);
        return Status.Success;
    }

    private void OpenDoor(GameObject o)
    {
        o.SetActive(false);
    }


    private Status goToBackDoor()
    {
        return GoToDoor(backDoor);
    }

    private Status goToFrontDoor()
    {
        return GoToDoor(frontDoor);
    }

    private Status GoToDoor(GameObject door)
    {
        GoTo(door);
        return Status.Success;
    }

    private Status GoTo(GameObject destinationObject)
    {
        agent.SetDestination(destinationObject.transform.position);
        if (!nearDestination)
        {
            return Status.Running;
        }
        return Status.Success;
    }

    // Update is called once per frame
    void Update()
    {
        root.Process();
    }
}