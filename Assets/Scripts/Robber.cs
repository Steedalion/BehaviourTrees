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
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
 root = new BehaviourTreeRoot("Robber");
 Sequence stealDiamond = new Sequence("Steal Diamond");
 
        ActionNode goToDiamond =
            new ActionNode("GoToDiamond", () => {
                agent.SetDestination(diamond.transform.position);
                return Status.Success;
            });
        Condition reachedDestination = new Condition("atVan", () =>{
            return agent.remainingDistance < 1; });

        ActionNode goToCar = new ActionNode("Go To Card", () =>
        {
            agent.SetDestination(truck.transform.position);
            return Status.Success;
        });
        stealDiamond.AddChild(goToDiamond);
        stealDiamond.AddChild(reachedDestination);
        stealDiamond.AddChild(goToCar);
        stealDiamond.AddChild(reachedDestination);
        stealDiamond.AddChild(goToDiamond);
        root.AddChild(stealDiamond);
    }

    // Update is called once per frame
    void Update()
    {
        root.Process();
    }
}
