using NUnit.Framework;

namespace BehaviourTree.BtreeTests
{
    public class TestSelector : TreeTest
    {
        private Node selector;

        [Test]
        public void CreateSelector()
        {
            selector = new Selector("Seltester");
        }

        [Test]
        public void NotNull()
        {
            Assert.NotNull(selector);
        }

        [Test]
        public void AddChildren()
        {
            selector.AddChild(oneshot);
            Assert.AreSame(oneshot, selector.GetCurrentChild());
        }

        [Test]
        public void OneShotShouldSucceed()
        {
            selector.AddChild(oneshot);
            Assert.AreEqual(Status.Success, selector.Process());
        }

        [Test]
        public void ShouldReturnFailureIfFail()
        {
            selector.AddChild(failingOneShot);
            Assert.AreEqual(Status.Failure, selector.Process());
        }

        [Test]
        public void ShouldRunIfMoreNodesLeft()
        {
            selector.AddChild(failingOneShot);
            selector.AddChild(oneshot);

            Assert.AreEqual(Status.Running, selector.Process());
            Assert.AreEqual(Status.Success, selector.Process());
        }
    }

    internal class Selector : Node
    {
        public Selector(string name) : base(name)
        {
        }

        public override Status Process()
        {
            throw new System.NotImplementedException();
        }
    }
}