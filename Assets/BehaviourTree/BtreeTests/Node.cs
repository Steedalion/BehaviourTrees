using System.Collections.Generic;

public abstract class Node
{
    protected List<Node> children = new List<Node>();
    public string name;

    protected Node(string name)
    {
        this.name = name;
    }

    public abstract Status Process();

    public void AddChild(Node child)
    {
        children.Add(child);
    }

    protected string PrintName(int level)
    {
        string output = CreateIndent(level);

        output += name + "\n";
        
        children.ForEach( child => output+= child.PrintName(level+1));
        return output;
    }

    private static string CreateIndent(int level)
    {
        string output = "";
        for (int i = 0; i < level; i++)
        {
            output += "-";
        }
        return output;
    }
}