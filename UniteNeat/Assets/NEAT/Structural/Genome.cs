using System;
using System.Collections;
using System.Collections.Generic;

public class Genome
{
    // Nodes and Connections
    private SortedDictionary<int, Node> nodes;
    private SortedDictionary<int, Connection> connections;

    public static float PROBABILITY_PERTURBING;
    public static float PERTURB_MAX;

    public static float WEIGHT_MUTATION_RATE;
    public static float NODE_MUTATION_RATE;
    public static float CONNECTION_MUTATION_RATE;

    public static float ENABLE_CHANCE;

    // Constructor
    public Genome()
    {
        nodes = new SortedDictionary<int, Node>();
        connections = new SortedDictionary<int, Connection>();
    }

    // Copy Constructor
    public Genome(Genome g) : this()
    {
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
        if (!nodes.ContainsKey(n.Id))
        {
            nodes.Add(n.Id, n);
        }
        else
        {
            throw new Exception("ERROR: Adding Two Nodes of the SAME ID");
        }
    }
    public void AddConnection(Connection c)
    {
        if (!connections.ContainsKey(c.Innovation))
        {
            connections.Add(c.Innovation, c);
        }
        else
        {
            throw new Exception("ERROR: Adding Two Connections of the SAME Innovation");
        }
    }

    public void AddNodeCopy(Node n)
    {
        if (!nodes.ContainsKey(n.Id))
        {
            nodes.Add(n.Id, new Node(n));
        }
        else
        {
            throw new Exception("ERROR: Adding Two Nodes of the SAME ID");
        }
    }
    public void AddConnectionCopy(Connection c)
    {
        if (!connections.ContainsKey(c.Innovation))
        {
            connections.Add(c.Innovation, new Connection(c));
        }
        else
        {
            throw new Exception("ERROR: Adding Two Connections of the SAME Innovation");
        }
    }

    // ==================================
    //              Mutations
    // ==================================

    public void Mutate()
    {
        var rand = new Random();

        if (rand.NextDouble() < NODE_MUTATION_RATE)
            WeightMutation();
        if (rand.NextDouble() < NODE_MUTATION_RATE)
            NodeMutation();
        if (rand.NextDouble() < CONNECTION_MUTATION_RATE)
            ConnectionMutation();
    }

    // Weight Mutation
    public void WeightMutation()
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
    public bool ConnectionMutation()
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
    public void NodeMutation()
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
    //           Static Methods
    // ==================================

    public enum Fitter
    {
        Parent1, Parent2
    }

    public static Genome CrossOver(Genome parent1, Genome parent2, Fitter f)
    {
        Genome child = new Genome();

        var rand = new Random();

        // Add Disjoint and Excess Connections from Fitter Parent
        if (f == Fitter.Parent1)
        {
            // Add all nodes from the fitter parent
            foreach (Node n in parent1.Nodes.Values)
            {
                child.AddNodeCopy(n);
            }
            foreach (Connection c in parent1.Connections.Values)
            {
                if (!parent2.Connections.ContainsKey(c.Innovation))
                {
                    child.AddConnectionCopy(c);
                }
            }
        }
        else
        {
            foreach (Node n in parent2.Nodes.Values)
            {
                child.AddNodeCopy(n);
            }
            foreach (Connection c in parent2.Connections.Values)
            {
                if (!parent1.Connections.ContainsKey(c.Innovation))
                {
                    child.AddConnectionCopy(c);
                }
            }
        }

        // Go through again to add all the matching gene randomly
        foreach (Connection c1 in parent1.Connections.Values)
        {
            foreach (Connection c2 in parent2.connections.Values)
            {
                // If matching gene --> pick randomly (50/50)
                if (c1.Innovation == c2.Innovation)
                {
                    if (rand.Next(0, 2) == 0)
                    {
                        child.AddConnectionCopy(c1);
                    }
                    else
                    {
                        child.AddConnectionCopy(c2);
                    }
                    // Randomly enable disabled inherited connection
                    if (!c1.Expressed && !c2.Expressed)
                    {
                        double r = rand.NextDouble();
                        if (r < ENABLE_CHANCE)
                        {
                            c2.Expressed = true;
                        }
                    }
                }
            }
        }

        return child;
    }
    // Getting Excess and Disjoint Genes to Calculate for Speciation
    public static int GetExcessDisjoint(Genome g1, Genome g2)
    {
        int matching = 0;

        foreach (KeyValuePair<int, Connection> c1 in g1.Connections)
        {
            foreach (KeyValuePair<int, Connection> c2 in g2.Connections)
            {
                if (c1.Value.Innovation == c2.Value.Innovation)
                {
                    matching++;
                    break;
                }
            }
        }
        return (g1.Connections.Count + g2.Connections.Count - 2 * (matching));
    }

    // Getting the Weight Difference Average
    public static float GetWeightDifferenceAverage(Genome g1, Genome g2)
    {
        float differenceSum = 0f;
        int matching = 0;
        foreach (KeyValuePair<int, Connection> c1 in g1.Connections)
        {
            foreach (KeyValuePair<int, Connection> c2 in g2.Connections)
            {
                if (c1.Value.Innovation == c2.Value.Innovation)
                {
                    differenceSum += Math.Abs(c1.Value.Weight - c2.Value.Weight);
                    matching++;
                    break;
                }
            }
        }

        return differenceSum / matching;
    }

    // Forward Propagation
    public List<float> ForwardPropagate(List<float> inputs)
    {
        ResetNodes();

        List<float> output = new List<float>();

        // Add input values to input nodes
        for (int i = 1; i <= inputs.Count; i++)
        {
            nodes[i].Value = inputs[i-1];
        }

        // Continue until all nodes have been propagated
        int total = 0;
        while (total < nodes.Count - inputs.Count)
        {
            // Everytime check if previous nodes connecting into it has been propagated
            // If they have then it's safe to propagate this node
            foreach (Node node in nodes.Values)
            {
                int currentCalculatedConnections = 0;

                // If has not been used and not input
                if (!node.Used && node.Type != Node.NodeType.INPUT)
                {
                    // Get all connections going into the node
                    List<Connection> InConnections = new List<Connection>();

                    foreach (Connection connection in connections.Values)
                    {
                        if (connection.OutNode == node.Id)
                        {
                            InConnections.Add(connection);
                        }
                    }

                    // Propagate all nodes connecting into the node if possible
                    foreach (Connection connection in InConnections)
                    {
                        // If already propagated or is input --> safe to propagate
                        if (nodes[connection.InNode].Used == true || nodes[connection.InNode].Type == Node.NodeType.INPUT)
                        {
                            // If has not been propagated yet
                            if (connection.Used == false)
                            {
                                node.Value = nodes[connection.InNode].Value * connection.Weight + node.Value;
                                connection.Used = true;
                            }
                        }
                    }

                    // Count up the used connections
                    foreach (Connection connection in InConnections)
                    {
                        if (connection.Used == true)
                        {
                            currentCalculatedConnections++;
                        }
                    }
                    
                    // If all connections have been used --> finished propagating this node
                    if (currentCalculatedConnections == InConnections.Count)
                    {
                        node.Used = true;
                        node.Value = (float)Activation(node.Value);
                        total++;
                    }
                }
            }
        }

        foreach (Node endNode in nodes.Values)
        {
            if (endNode.Type == Node.NodeType.OUTPUT)
                output.Add(endNode.Value);
        }
        ResetNodes();
        return output;
    }


    // Debug Print
    public static void DebugPrint(Genome genome)
    {
        foreach (KeyValuePair<int, Node> node in genome.Nodes)
        {
            UnityEngine.Debug.Log("ID: " + node.Value.Id + " NodeType: " + node.Value.Type);
        }
        foreach (KeyValuePair<int, Connection> connection in genome.Connections)
        {
            UnityEngine.Debug.Log("Innovation: " + connection.Value.Innovation + " In: " + connection.Value.InNode + " Out: " + connection.Value.OutNode + " Expressed: " + connection.Value.Expressed + " Weight: " + connection.Value.Weight);
        }
    }

    // Activation Function
    public static double Activation(double num)
    {
        return 1.0 / (1 + Math.Exp(-4.9 * num));
        // return System.Math.Tanh(num);
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

    // Clear nodes and connections for propagation
    private void ResetNodes()
    {
        foreach (Node node in nodes.Values)
        {
            node.Value = 0;
            node.Used = false;
        }
        foreach (Connection connection in connections.Values)
        {
            connection.Used = false;
        }
    }

    // Give information to genome printer
    public float[][] GetGeneDrawConnections(Genome genome)
    {
        int numberOfGenes = genome.Connections.Count; //copy gene count

        float[][] connectionsDraw = null; //2D connections to return 

        List<float[]> connectionList = new List<float[]>(); //empty connections list to fill with genome details

        foreach (Connection connectionGene in genome.Connections.Values)
        {
            //do something by value....accessing item.value
            Connection gene = connectionGene;

            float[] details = new float[3]; //will copy in node Id, out node Id and weight

            details[0] = gene.InNode; //copy in node Id
            details[1] = gene.OutNode; //copy out node Id

            if (gene.Expressed == true) //gene is enabled
                details[2] = gene.Weight; //copy weight
            else //gene is disabled
                details[2] = 0f; //set to 0

            connectionList.Add(details); //add detail to the connection list
        }

        connectionsDraw = connectionList.ToArray(); //convert connection list to 2D connection array

        return connectionsDraw; //return 2D connection array

    }

}
