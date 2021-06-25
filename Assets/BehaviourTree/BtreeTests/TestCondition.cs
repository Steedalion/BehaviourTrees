using NUnit.Framework;

namespace BehaviourTree.BtreeTests
{
    public class TestCondition
    {
        private Node condition;

        [SetUp]
        public void CreateNewCondition()
        {
            condition = new Condition("Condition", () => false);
        }

        [Test]
        public void NotNull()
        {
            Assert.IsNotNull(condition);
        }

        [Test]
        public void AlwaysReturnsFailure()
        {
            condition = new Condition("Condition", () => false);
            Assert.AreEqual(Status.Failure, condition.Process());
        }

        [Test]
        public void ReturnsTrueIfSet()
        {
            condition = new Condition("Always true", () => true);
            Assert.AreEqual(Status.Success, condition.Process());
        }

    }

    public class Condition : Node
    {
        public delegate bool myCondition();

        private myCondition eval;
        public Condition(string name, myCondition myCondition) : base(name)
        {
            eval = myCondition;
        }

        public override Status Process()
        {
            throw new System.NotImplementedException();
        }
    }
}