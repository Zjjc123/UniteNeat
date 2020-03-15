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

        foreach (Connection c in Connections.Values)
        {
            double r = rand.NextDouble();
            // If perturb
            if (r < PROBABILITY_PERTURBING)
            {
                // Add plus or minus PERTURB_MAX
                c.Weight += (float)rand.NextDouble() * PERTURB_MAX - PERTURB_MAX;
            }
            else
            {
                // Random weights
                c.Weight = (float)rand.NextDouble() * 2 - 1;
            }

            // Limit to -1 and 1
            c.Weight = Math.Max(c.Weight, -1);
            c.Weight = Math.Min(c.Weight, 1);
        }
    }

    // Connection Mutation
    private bool ConnectionMutation()
    {
        List<Tuple<int, int>> tried = new List<Tuple<int, int>>();

        var rand = new Random();

        bool found = false;

        while (!found)
        {

            if (tried.Count >= Nodes.Count * Nodes.Count)
                return false;

            int nodeInID = rand.Next(1, Nodes.Count + 1);
            int nodeOutID = rand.Next(1, Nodes.Count + 1);

            Tuple<int, int> t = new Tuple<int, int>(nodeInID, nodeOutID);

            if (!tried.Contains(t))
            {
                tried.Add(t);
            }

            Node nodeIn = Nodes[nodeInID];
            Node nodeOut = Nodes[nodeOutID];


            bool impossible = false;

            // Check if connection already exists
            foreach (Connection c in Connections.Values)
            {
                if ((c.InNode == nodeInID && c.OutNode == nodeOutID)
                    || (c.InNode == nodeOutID && c.OutNode == nodeInID))
                {
                    impossible = true;
                }
            }

            // Check if connection is possible
            if (nodeIn.Type == Node.NodeType.INPUT && nodeOut.Type == Node.NodeType.INPUT)
            {
                impossible = true;
            }
            else if (nodeIn.Type == Node.NodeType.OUTPUT && nodeOut.Type == Node.NodeType.OUTPUT)
            {
                impossible = true;
            }

            // If it exists or not allowed --> next try
            if (impossible)
            {
                continue;
            }



            // Get Correct Orientation
            bool reversed = false;

            // Cases that should be reversed
            // Case 1: Hidden --> Input
            if (nodeIn.Type == Node.NodeType.HIDDEN && nodeOut.Type == Node.NodeType.INPUT)
            {
                reversed = true;
            }
            // Case 2: Output --> Hidden
            else if (nodeIn.Type == Node.NodeType.OUTPUT && nodeOut.Type == Node.NodeType.HIDDEN)
            {
                reversed = true;
            }
            // Case 3: Output --> Input
            else if (nodeIn.Type == Node.NodeType.OUTPUT && nodeOut.Type == Node.NodeType.INPUT)
            {
                reversed = true;
            }

            // If should be reversed then reverse
            if (reversed)
            {
                nodeIn = Nodes[nodeOutID];
                nodeOut = Nodes[nodeInID];
            }

            // Check if a connection creates a loop
            if (IsLoopCreated(nodeInID, nodeOutID))
            {
                continue;
            }

            // Generate Weight
            float weight = (float)rand.NextDouble() * 2 - 1;

            // Contains history automatically adds new connections to history if does not exist
            // Use placeholder innovation
            Connection newConnection = new Connection(nodeInID, nodeOutID, weight, true, 0);

            // Alter innovation based on history
            History.AlterInnovationBasedOnHistory(newConnection);

            // Add connection to list of connections
            Connections.Add(newConnection.Innovation, newConnection);

            found = true;

        }

        return true;
    }

    // Node Mutation
    private void NodeMutation()
    {
        var rand = new Random();
        int r = rand.Next(0, Connections.Count);

        int count = 0;

        Connection newC = null;

        foreach (Connection connection in Connections.Values)
        {
            if (count == r)
            {
                newC = connection;
                break;
            }
            count++;
        }

        Node inNode = nodes[newC.InNode];
        Node outNode = nodes[newC.OutNode];

        // Disable orignal connection
        newC.Disable();

        // Create New Node
        Node newNode = new Node(Node.NodeType.HIDDEN, nodes.Count + 1);

        // In Connection
        Connection inToNew = new Connection(inNode.Id, newNode.Id, 1f, true, 0);
        History.AlterInnovationBasedOnHistory(inToNew);

        // Out Connection
        Connection newToOut = new Connection(newNode.Id, outNode.Id, newC.Weight, true, 0);
        History.AlterInnovationBasedOnHistory(newToOut);

        // Add to lists
        nodes.Add(newNode.Id, newNode);
        connections.Add(inToNew.Innovation, inToNew);
        connections.Add(newToOut.Innovation, newToOut);

    }

    // ==================================
    //              Helpers
    // ==================================

    // Loop Connection Checker
    // Use recursion to add output nodes to a list 
    // If new connection connects from an existing out node to the in node a loop will form
    private bool IsLoopCreated(int inID, int outID)
    {
        List<int> outNodes = new List<int>();
        AddReachableNodes(outNodes, outID);
        return outNodes.Contains(inID);
    }

    // Add all the reachable nodes from an input node to a list
    private void AddReachableNodes(List<int> OutNodesList, int start)
    {
        // Add outnode
        OutNodesList.Add(start);
        foreach (Connection connection in connections.Values)
        {
            // If any connections c start with the outnode --> add c's outnode 
            // run recursion until all reachable nodes will be added
            // No infinite loop because there will not be any loop created in the first place
            if (connection.InNode == start)
            {
                AddReachableNodes(OutNodesList, connection.OutNode);
            }
        }
    }
}
