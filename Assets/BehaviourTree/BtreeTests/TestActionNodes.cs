using System;
using NUnit.Framework;
using UnityEngine;

namespace BehaviourTree.BtreeTests
{
    public class TestActionNodes : TreeTest
    {
        private bool oneShotRun = false;
        private ActionNode oneShotNode;

        [SetUp]
        public void Reset()
        {
            oneShotRun = false;
            oneShotNode = new ActionNode("RunIt", OneShotAction);
        }

        Status OneShotAction()
        {
            oneShotRun = true;
            return Status.Success;
        }

        [Test]
        public void CreateActionNode()
        {
            Assert.NotNull(oneShotNode);
        }

        [Test]
        public void RunActionNode()
        {
            Assert.IsFalse(oneShotRun);
            oneShotNode.Process();
            Assert.IsTrue(oneShotRun);
        }

        [Test]
        public void TestActionNodeInProcess()
        {
            int counter = 0;
            ActionNode multiShotNode = new ActionNode("multi", () =>
            {
                if (counter >= 2) return Status.Success;
                counter++;
                return Status.Running;
            });
            tree.AddChild(multiShotNode);
            tree.AddChild(oneShotNode);
            Assert.IsFalse(oneShotRun);
            Assert.AreEqual(0, counter);

            tree.Process();
            Assert.IsFalse(oneShotRun); //1
            Assert.AreEqual(1, counter);

            tree.Process();
            Assert.IsFalse(oneShotRun); //2
            Assert.AreEqual(2, counter);
            Assert.AreSame(multiShotNode, tree.GetCurrentChild());
            tree.Process();
            Assert.AreSame(oneShotNode, tree.GetCurrentChild());
            tree.Process();
            Assert.True(oneShotRun); // success = run;
        }
    }
}