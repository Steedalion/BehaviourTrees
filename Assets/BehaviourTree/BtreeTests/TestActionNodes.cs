using System;
using NUnit.Framework;

namespace BehaviourTree.BtreeTests
{
    public class TestActionNodes
    {
        private bool wasRun = false;


        [SetUp]
        public void Reset()
        {
            wasRun = false;
        }

        Status OneShotAction()
        {
            wasRun = true;
            return Status.Success;
        }

        [Test]
        public void CreateActionNode()
        {
            ActionNode action = new ActionNode("Runit", OneShotAction);
            Assert.NotNull(action);
        }

        [Test]
        public void RunActionNode()
        {
            Assert.IsFalse(wasRun);
            ActionNode actionNode = new ActionNode("RunIt", OneShotAction);
            actionNode.Process();
            Assert.IsTrue(wasRun);
        }
    }
}