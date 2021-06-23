using System.Diagnostics;
using NUnit.Framework;

namespace BehaviourTree.BtreeTests
{
    public class TestSequence : TreeTest
    {
        private Node sequence;

        [SetUp]
        public void CreateANewSequence()
        {
            sequence = new Sequence("MySequence");
        }

        [Test]
        public void AddNodeToSequence()
        {
            ThreeRunningNode threeRunningNode = new ThreeRunningNode("3");

            sequence.AddChild(oneshot);
            sequence.AddChild(threeRunningNode);

            Assert.AreSame(oneshot, sequence.GetCurrentChild());
            Assert.AreEqual(Status.Running, sequence.Process());
            Assert.AreSame(threeRunningNode, sequence.GetCurrentChild());
            Assert.AreEqual(Status.Running, sequence.Process());
            Assert.AreEqual(1, threeRunningNode.counter);
            Assert.AreEqual(Status.Running, sequence.Process());
            Assert.AreEqual(2, threeRunningNode.counter);
            Assert.AreEqual(Status.Running, sequence.Process());
            Assert.AreEqual(3, threeRunningNode.counter);
            Assert.AreEqual(Status.Success, sequence.Process());
        }

        [Test]
        public void FailingSequence()
        {
            Node failingOneShot = new ActionNode("Failure", () => Status.Failure);
            sequence.AddChild(failingOneShot);
            sequence.AddChild(oneshot);
            Assert.AreEqual(Status.Failure, sequence.Process());
        }
    }
}