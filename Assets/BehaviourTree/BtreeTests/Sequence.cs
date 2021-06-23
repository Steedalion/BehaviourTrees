namespace BehaviourTree.BtreeTests
{
    public class Sequence : Node
    {
        public override Status Process()
        {
            Status currentResult = children[currentChild].Process();
            if (currentResult == Status.Running)
                return Status.Running;
            if (currentResult == Status.Failure)
                return Status.Failure;
            if (currentResult == Status.Success) currentChild++;

            if (currentChild < children.Count) return Status.Running;
            currentChild = 0;
            return Status.Success;
        }

        public Sequence(string name) : base(name)
        {
        }
    }
}