using System.Collections;
using System.Collections.Generic;

public class Node
{
    // Types of Nodes
    public enum NodeType
    {
        INPUT,
        HIDDEN,
        OUTPUT
    };

    // Node Properties
    private NodeType _type;
    private int _id;
    private float _value = 0f;
    
    // Constructor
    public Node(NodeType type, int id)
    {
        _type = type;
        _id = id;
    }

    // Copy Constructor
    public Node(Node n)
    {
        _type = n.Type;
        _id = n.Id;
        _value = n.Value;
    }

    // Getters and Setters
    public NodeType Type
    {
        get { return _type; }
    }
    public float Value
    {
        get { return _value; }
        set { _value = value; }
    }

    public int Id
    {
        get { return _id; }
    }


}
