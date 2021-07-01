namespace BehaviourTree.BtreeTests
{
    public class ActionNode : Node
    {
        public delegate Status Tick();

        private Tick processAction;

        
        public ActionNode(string name, Tick oneShotTick) : base(name)
        {
            processAction = oneShotTick;
        }

        public override Status Process()
        {
            return processAction.Invoke();
        }
    }
}