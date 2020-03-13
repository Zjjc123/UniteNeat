using System.Collections;
using System.Collections.Generic;

public class Connections
{
    // Connection Properties
    private int _in;
    private int _out;
    private float _weight;
    private bool _expressed;
    private int _innovation;

    // Constructor
    public Connections(int inNode, int outNode, float weight, bool expressed, int innovation)
    {
        _in = inNode;
        _out = outNode;
        _weight = weight;
        _expressed = expressed;
        _innovation = innovation;
    }

    // Getters and Setters
    public int InNode
    {
        get { return _in; }
    }

    public int OutNode
    {
        get { return _out; }
    }

    public float Weight
    {
        get { return _weight; }
        set { _weight = value; }
    }

    public bool Expressed
    {
        get { return _expressed; }
        set { _expressed = value; }
    }

    public void Disable()
    {
        this.Expressed = false;
    }

    public int Innovation
    {
        get { return _innovation; }
    }

}
