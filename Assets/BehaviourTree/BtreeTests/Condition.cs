namespace BehaviourTree.BtreeTests
{
    public class Condition : Node
    {
        public delegate bool myCondition();

        private myCondition eval;
        public Condition(string name, myCondition condition) : base(name)
        {
            eval = condition;
        }

        public override Status Process()
        {
            return eval.Invoke() ? Status.Success : Status.Failure;
        }
    }
}