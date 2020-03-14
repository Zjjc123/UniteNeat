using System;
using System.Collections;
using System.Collections.Generic;

public class Genome
{
    // Nodes and Connections
    private SortedDictionary<int, Node> nodes;
    private SortedDictionary<int, Connection> connections;

    // Constants
    private float PROBABILITY_PERTURBING = 0.9f;
    private float PERTURB_MAX = 0.02f;

    private const float WEIGHT_MUTATION_RATE = 0.01f;
    private const float NODE_MUTATION_RATE = 0.03f;
    private const float CONNECTION_MUTATION_RATE = 0.05f;

    // Constructor
    public Genome()
    {
        nodes = new SortedDictionary<int, Node>();
        connections = new SortedDictionary<int, Connection>();
    }

    // Copy Constructor
    public Genome(Genome g)
    {
        nodes = new SortedDictionary<int, Node>();
        connections = new SortedDictionary<int, Connection>();

        foreach (KeyValuePair<int, Node> n in g.Nodes)
        {
            Node newNode = new Node(n.Value);
            nodes.Add(n.Key, newNode);
        }

        foreach (KeyValuePair<int, Connection> c in g.Connections)
        {
            Connection newConnection = new Connection(c.Value);
            connections.Add(c.Key, newConnection);
        }
    }

    // Getters and Setters
    public SortedDictionary<int, Node> Nodes
    {
        get { return nodes; }
    }
    
    public SortedDictionary<int, Connection> Connections
    {
        get { return connections; }
    }

    // Add Nodes and Connections
    public void AddNode(Node n)
    {
        nodes.Add(n.Id, n);
    }
    public void AddConnection(Connection c)
    {
        connections.Add(c.Innovation, c);
    }

    // ==================================
    //              Mutations
    // ==================================

    public void Mutate()
    {
        WeightMutation();
        NodeMutation();
        ConnectionMutation();
    }

    // Weight Mutation
    private void WeightMutation()
    {
        var rand = new Random();

        foreach (KeyValuePair<int, Connection> c in Connections)
        {
            double r = rand.NextDouble();
            // If perturb
            if (r < PROBABILITY_PERTURBING)
            {
                // Add plus or minus PERTURB_MAX
                c.Value.Weight += (float)rand.NextDouble() * PERTURB_MAX - PERTURB_MAX;
            }
            else
            {
                // Random weights
                c.Value.Weight = (float)rand.NextDouble() * 2 - 1;
            }

            // Limit to -1 and 1
            c.Value.Weight = Math.Max(c.Value.Weight, -1);
            c.Value.Weight = Math.Min(c.Value.Weight, 1);
        }
    }

    // Connection Mutation
    private void ConnectionMutation()
    {

    }

    // Node Mutation
    private void NodeMutation()
    {

    }

}
