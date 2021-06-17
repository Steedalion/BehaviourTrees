using System.Collections.Generic;
using NUnit.Framework;

public class TestProcess
{
    private Node tree;

    [SetUp]
    public void CreateTree()
    {
        tree = new BehaviourTree();
    }

    [Test]
    public void OneShotTest()
    {
        OneShotNode oneshot = new OneShotNode();
        tree.AddChild(oneshot);
        tree.Process();
        Assert.IsTrue(oneshot.done);
        Assert.AreEqual(Status.Success, oneshot.status);
    }

    [Test]
    public void TwoOneShots()
    {
        OneShotNode one = new OneShotNode();
        OneShotNode two = new OneShotNode();

        tree.AddChild(one);
        tree.AddChild(two);
        Assert.IsFalse(one.done);
        Assert.AreEqual(Status.Running, tree.Process());
        Assert.IsTrue(one.done);

        Assert.IsFalse(two.done);
        Assert.AreEqual(Status.Success, tree.Process());
        Assert.IsTrue(two.done);
    }

    [Test]
    public void RunningTree()
    {
        RunningTree run = new RunningTree();
        tree.AddChild(run);


        Assert.AreEqual(Status.Running, tree.Process());
        Assert.AreEqual(1, run.counter);
        Assert.AreEqual(Status.Running, tree.Process());
        Assert.AreEqual(2, run.counter);
        Assert.AreEqual(Status.Running, tree.Process());
        Assert.AreEqual(3, run.counter);
        Assert.AreEqual(Status.Success, tree.Process());
        Assert.IsTrue(run.isComplete);
    }
}

public class RunningTree : Node
{
    public bool isComplete;
    public int counter = 0;

    public override Status Process()
    {
        if (counter < 3)
        {
            counter++;
            return Status.Running;
        }

        isComplete = true;
        return Status.Success;
    }
}

public enum Status
{
    Success,
    Running
}

public class OneShotNode : Node
{
    public bool done;
    public Status status;

    public override Status Process()
    {
        done = true;
        return Status.Success;
    }
}

public class BehaviourTree : Node
{
    private int currentChild = 0;

    public override Status Process()
    {
        if (children[currentChild].Process() == Status.Success)
        {
            currentChild++;
        }

        if (currentChild >= children.Count)
        {
            return Status.Success;
        }

        return Status.Running;
    }
}

public abstract class Node
{
    protected List<Node> children = new List<Node>();
    public abstract Status Process();

    public void AddChild(Node child)
    {
        children.Add(child);
    }
}