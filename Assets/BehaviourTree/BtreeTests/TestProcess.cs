using NUnit.Framework;
using UnityEngine;

public class TestProcess
{
    private BehaviourTree tree;

    [SetUp]
    public void CreateTree()
    {
        tree = new BehaviourTree("root");
    }

    [TearDown]
    public void DestroyTree()
    {
        tree = null;
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
    public void TestRunningTree()
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


    [Test]
    public void DefaultName()
    {
        Assert.AreEqual("root",tree.name);
    }

    [Test]
    public void PrintEmptyTree()
    {
        Assert.AreEqual("root\n", tree.TreeString());
    }
    
    [Test]
    public void PrintTreeWith1Child()
    {
        tree.AddChild(new OneShotNode("first"));
        Debug.Log(tree.TreeString());
        Assert.AreEqual("root\n-first\n", tree.TreeString());
    }
     [Test]
    public void PrintTreeWith2Child()
    {
        tree.AddChild(new OneShotNode("first"));
        tree.AddChild(new OneShotNode("second"));
        Debug.Log(tree.TreeString());
        Assert.AreEqual("root\n-first\n-second\n", tree.TreeString());
    }
    
      [Test]
    public void PrintTree2Deep()
    {
        Node child = new OneShotNode("child");
        Node grandchild = new OneShotNode("grandchild");
        child.AddChild(grandchild);
        tree.AddChild(child);
        Debug.Log(tree.TreeString());
        Assert.AreEqual("root\n-child\n--grandchild\n", tree.TreeString());
    }
    
    

    private class RunningTree : Node
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


    private class OneShotNode : Node
    {
        public bool done;
        public Status status;

        public OneShotNode(string name) : base(name)
        {
        }

        public OneShotNode()
        {
        }

        public override Status Process()
        {
            done = true;
            return Status.Success;
        }
    }
}