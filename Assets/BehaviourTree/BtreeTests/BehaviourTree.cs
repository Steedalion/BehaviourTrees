public class BehaviourTree : Node
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

    

    public BehaviourTree(string name) : base(name)
    {
    }
}

public enum Status
{
    Success,
    Running
}