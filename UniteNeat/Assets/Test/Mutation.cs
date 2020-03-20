using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mutation : MonoBehaviour {

    Genome genome = new Genome();


    List<float> vision = new List<float>();

    public void Start()
    {

        vision.Add(0);
        vision.Add(1);
        vision.Add(2);
        for (int i = 0; i < 3; i++)
        {
            Node node = new Node(Node.NodeType.INPUT, i + 1);
            genome.AddNode(node);
        }
        genome.AddNode(new Node(Node.NodeType.OUTPUT, 4));
        genome.AddNode(new Node(Node.NodeType.OUTPUT, 5));
        genome.AddNode(new Node(Node.NodeType.OUTPUT, 6));
        //genome.AddNode(new Node(Node.NodeType.HIDDEN, 5));
        //genome.AddNode(new Node(Node.NodeType.HIDDEN, 6));


        genome.AddConnection(new Connection(1, 4, 0f, true, 1));
        genome.AddConnection(new Connection(2, 5, 0f, true, 2));
        genome.AddConnection(new Connection(3, 6, 0f, true, 3));
        
        foreach (Connection c in genome.Connections.Values)
        {
            History.AddConnectionToInnovationHistoryDebug(c);
        }

        gameObject.GetComponent<GenomePrinter>().Draw(genome);
        DebugPrint(genome);

        History.SetInnovationDebug(3);

    }

    public void NodeMutation(){
        genome.NodeMutation();
        gameObject.GetComponent<GenomePrinter>().Draw(genome);
        //DebugPrint(genome);

    }
    public void ConnectionMutation()
    {
        genome.ConnectionMutation();
        gameObject.GetComponent<GenomePrinter>().Draw(genome);
        //DebugPrint(genome);
    }

    public void MutateWeights()
    {
        genome.WeightMutation();
        gameObject.GetComponent<GenomePrinter>().Draw(genome);
        DebugPrint(genome);
    }

    public void DebugPrint(Genome genome){
        foreach (KeyValuePair<int, Node> node in genome.Nodes)
        {
            Debug.Log("ID: " + node.Value.Id + " NodeType: " + node.Value.Type);
        }
        foreach (KeyValuePair<int, Connection> connection in genome.Connections)
        {
            Debug.Log("Innovation: " + connection.Value.Innovation + " In: " + connection.Value.InNode + " Out: " + connection.Value.OutNode + " Expressed: " + connection.Value.Expressed + " Weight: " + connection.Value.Weight);
        }
    }

}
