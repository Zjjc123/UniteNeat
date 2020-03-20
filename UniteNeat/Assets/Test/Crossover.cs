using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Crossover : MonoBehaviour
{
    public void Start()
    {
        Genome parent1 = new Genome();
        for (int i = 0; i < 3; i++)
        {
            Node node = new Node(Node.NodeType.INPUT, i + 1);
            parent1.AddNodeCopy(node);
        }
        parent1.AddNodeCopy(new Node(Node.NodeType.OUTPUT, 4));
        parent1.AddNodeCopy(new Node(Node.NodeType.HIDDEN, 5));


        parent1.AddConnection(new Connection(1, 4, 1f, true, 1));
        parent1.AddConnection(new Connection(2, 4, 1f, false, 2));
        parent1.AddConnection(new Connection(3, 4, 1f, true, 3));
        parent1.AddConnection(new Connection(2, 5, 1f, true, 4));
        parent1.AddConnection(new Connection(5, 4, 1f, true, 5));
        parent1.AddConnection(new Connection(1, 5, 1f, true, 8));

        History.SetInnovationDebug(8);


        Genome parent2 = new Genome();
        for (int i = 0; i < 3; i++)
        {
            Node node = new Node(Node.NodeType.INPUT, i + 1);
            parent2.AddNodeCopy(node);
        }
        parent2.AddNodeCopy(new Node(Node.NodeType.OUTPUT, 4));
        parent2.AddNodeCopy(new Node(Node.NodeType.HIDDEN, 5));
        parent2.AddNodeCopy(new Node(Node.NodeType.HIDDEN, 6));

        parent2.AddConnection(new Connection(1, 4, -1f, true, 1));
        parent2.AddConnection(new Connection(2, 4, -1f, false, 2));
        parent2.AddConnection(new Connection(3, 4, -1f, true, 3));
        parent2.AddConnection(new Connection(2, 5, -1f, true, 4));
        parent2.AddConnection(new Connection(5, 4, -1f, false, 5));
        parent2.AddConnection(new Connection(5, 6, -1f, true, 6));
        parent2.AddConnection(new Connection(6, 4, -1f, true, 7));
        parent2.AddConnection(new Connection(3, 5, -1f, true, 9));
        parent2.AddConnection(new Connection(1, 6, -1f, true, 10));

        History.SetInnovationDebug(10);


        Genome child = Genome.CrossOver(parent1, parent2, Genome.Fitter.Parent2);
        
        gameObject.GetComponent<GenomePrinter>().Draw(child);
        Genome.DebugPrint(child);
     
    }

}
