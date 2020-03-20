using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardPropagation : MonoBehaviour
{
    Genome genome = new Genome();
   
    private void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            Node node = new Node(Node.NodeType.INPUT, i + 1);
            genome.AddNode(node);
        }

        genome.AddNode(new Node(Node.NodeType.OUTPUT, 3));
        genome.AddNode(new Node(Node.NodeType.HIDDEN, 4));
     

        genome.AddConnection(new Connection(1, 4, 0.2f, true, 1));
        genome.AddConnection(new Connection(2, 4, 0.4f, true, 2));
        genome.AddConnection(new Connection(4, 3, 1f, true, 3));
        genome.AddConnection(new Connection(1, 3, -0.3f, true, 4));

        History.SetInnovationDebug(4);

        /*
        for (int i = 0; i < 30; i++)
        {
            genome.Mutate();
        }
        
        */

        ForwardProp();
        GetComponent<GenomePrinter>().Draw(genome);
    }

    public void ForwardProp()
    {
        List<float> input = new List<float>();

        input.Add(0.2f);
        input.Add(4f);


        /*
        Expected:      
            
        Node 1: 0.2
        Node 2: 4

        Node 4 = activation (0.2 * 0.2 + 0.4 * 4)
        Node 4 = activation (1.64)

        */
        float node4 = (float)Genome.Activation(input[0] * 0.2f + input[1] * 0.4f);

        /*
        Node 3 = activation (node 4 - 0.3 * 0.2)
        Node 3 = activation (node 4 - 0.06)
        */

        float node3 = (float)Genome.Activation(node4 + input[0] * -0.3f);

        Debug.Log(genome.ForwardPropagate(input)[0]);
        Debug.Log("Expected: " + node3);
        Debug.Assert(node3 == genome.ForwardPropagate(input)[0]);
    }


}
   
