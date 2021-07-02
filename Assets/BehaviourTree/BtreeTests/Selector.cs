namespace BehaviourTree.BtreeTests
{
    public class Selector : Node
    {
        public Selector(string name) : base(name)
        {
        }

        public override Status Process()
        {
            Status currentChildStatus = children[currentChild].Process();

            if (currentChildStatus == Status.Running)
            {
                return Status.Running;
            }

            if (currentChildStatus == Status.Success)
            {
                return Status.Success;
            }

            if (currentChildStatus == Status.Failure)
            {
                if (lastChild) 
                {
                    return Status.Failure;
                }

                currentChild++;
            }

            return Status.Running;
        }

      
    }
}