using NUnit.Framework;

namespace BehaviourTree.BtreeTests
{
    public class TestProcess : TreeTest
    {
        [Test]
        public void RootProcessBeyondChildren()
        {
            tree.AddChild(oneshot);
            tree.Process();
            Assert.AreEqual(Status.Success, tree.Process());
        }

        [Test]
        public void RootFailure()
        {
            tree.AddChild(failingOneShot);
            tree.Process();
            tree.Process();
            Assert.AreEqual(Status.Failure, tree.Process());
        }

        [Test]
        public void OneShotTest()
        {
            tree.AddChild(oneshot);
            tree.Process();
            Assert.IsTrue(oneshot.done);
            Assert.AreEqual(Status.Success, oneshot.status);
        }

        [Test]
        public void TwoOneShots()
        {
            OneShotNode two = new OneShotNode("2");

            tree.AddChild(oneshot);
            tree.AddChild(two);
            Assert.IsFalse(oneshot.done);
            Assert.AreEqual(Status.Running, tree.Process());
            Assert.IsTrue(oneshot.done);
            Assert.IsFalse(two.done);
            Assert.AreEqual(Status.Running, tree.Process());
            Assert.IsTrue(two.done);
            Assert.AreEqual(Status.Success, tree.Process());
            Assert.AreEqual(Status.Success, tree.Process());
        }

        [Test]
        public void TestRunningTree()
        {
            ThreeRunningNode run = new ThreeRunningNode("runningTree");
            tree.AddChild(run);

            Assert.AreEqual(Status.Running, tree.Process());
            Assert.AreEqual(1, run.counter);
            Assert.AreEqual(Status.Running, tree.Process());
            Assert.AreEqual(2, run.counter);
            Assert.AreEqual(Status.Running, tree.Process());
            Assert.AreEqual(3, run.counter);
            Assert.AreEqual(Status.Running, tree.Process());
            Assert.IsTrue(run.isComplete);
            Assert.AreEqual(Status.Success, tree.Process());
        }


        [Test]
        public void DefaultName()
        {
            tree = new BehaviourTreeRoot("root");
            Assert.AreEqual("root", tree.name);
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
            Assert.AreEqual("root\n-first\n", tree.TreeString());
        }

        [Test]
        public void PrintTreeWith2Child()
        {
            tree.AddChild(new OneShotNode("first"));
            tree.AddChild(new OneShotNode("second"));
            Assert.AreEqual("root\n-first\n-second\n", tree.TreeString());
        }

        [Test]
        public void PrintTree2Deep()
        {
            Node child = new OneShotNode("child");
            Node grandchild = new OneShotNode("grandchild");
            child.AddChild(grandchild);
            tree.AddChild(child);
            Assert.AreEqual("root\n-child\n--grandchild\n", tree.TreeString());
        }
    }

    public class OneShotNode : Node
    {
        public bool done;
        public Status status;

        public OneShotNode(string name) : base(name)
        {
        }


        public override Status Process()
        {
            done = true;
            return Status.Success;
        }
    }

    public class ThreeRunningNode : Node
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

        public ThreeRunningNode(string name) : base(name)
        {
        }
    }
}