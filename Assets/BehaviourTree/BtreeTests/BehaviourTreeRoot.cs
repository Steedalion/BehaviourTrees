public class BehaviourTreeRoot : Node
{
    private int currentChild = 0;

    public override Status Process()
    {
        if (children[currentChild].Process() == Status.Success)
        {
            currentChild++;
        }

        if (currentChild >= children.Count)
        {
            return Status.Success;
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
    Running
}