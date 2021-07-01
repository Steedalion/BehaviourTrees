namespace BehaviourTree.BtreeTests
{
    public class BehaviourTreeRoot : Node
    {
    

        public override Status Process()
        {
            if (currentChild >= children.Count)
            {
                return Status.Success;
            }

            if (children[currentChild].Process() == Status.Success)
            {
                currentChild++;
                return Status.Running;
            }


            return Status.Running;
        }

    

        public string TreeString()
        {
            return PrintName(0);
        }

    

        public BehaviourTreeRoot(string name) : base(name)
        {
        }
    }

    public enum Status
    {
        Success,
        Running,
        Failure
    }
}