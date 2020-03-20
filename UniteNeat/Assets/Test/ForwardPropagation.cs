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
     

        genome.AddConnection(new Connection(1, 4, 1f, true, 1));
        genome.AddConnection(new Connection(2, 4, 1f, true, 2));
        genome.AddConnection(new Connection(4, 3, 1f, true, 3));
        genome.AddConnection(new Connection(1, 3, 1f, true, 4));

        History.SetInnovationDebug(4);

        for (int i = 0; i < 30; i++)
        {
            genome.Mutate();
        }
        
              
        GetComponent<GenomePrinter>().Draw(genome);

        ForwardProp();
    }

    public void ForwardProp(){
        List<float> input = new List<float>();

        input.Add(0.3f);
        input.Add(4f);
        //input.Add(2f);

        Debug.Log(genome.ForwardPropagate(input)[0]);
    }

}
   
