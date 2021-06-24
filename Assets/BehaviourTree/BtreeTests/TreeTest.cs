using System.Diagnostics;
using BehaviourTree.BtreeTests;
using NUnit.Framework;

public class TreeTest
{
    protected BehaviourTreeRoot tree;
    protected OneShotNode oneshot;
    
   protected Node failingOneShot = new ActionNode("Failure", () => Status.Failure);


    [SetUp]
    public void CreateAll()
    {
        CreateTree();
        CreateOneShotNode();
    }

    public void CreateTree()
    {
        tree = new BehaviourTreeRoot("root");
    }

    public void CreateOneShotNode()
    {
        oneshot = new OneShotNode("1");
    }

    [TearDown]
    public void DestroyTree()
    {
        tree = null;
    }
    
}