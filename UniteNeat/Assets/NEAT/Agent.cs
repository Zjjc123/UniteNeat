using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    private Genome _brain;
    private float _fitness;

    List<float> vision = new List<float>();
    List<float> decision = new List<float>();

    public void Initialize(int input, int output)
    {
        Genome genome = new Genome();
        for (int i = 0; i < input; i++)
        {
            Node node = new Node(Node.NodeType.INPUT, i + 1);
            genome.AddNode(node);
        }
        for (int i = input; i < output + input; i++)
        {
            Node node = new Node(Node.NodeType.OUTPUT, i + 1);
            genome.AddNode(node);
        }

        int inno = 0;

        for (int i = 1; i <= input; i++)
        {
            for (int j = input + 1; j <= output + input; j++)
            {
                genome.AddConnection(new Connection(i, input, 0f, true, ++inno));
            }
        }
    }

    public static Agent CreateChildrenThroughCrossOver(Agent parent1, Agent parent2, GameObject obj, Genome.Fitter f)
    {
        GameObject child = Instantiate(obj, Vector3.zero, Quaternion.identity);
        Agent childAgent = child.GetComponent<Agent>();
        childAgent.Brain = Genome.CrossOver(parent1.Brain, parent2.Brain, f);
        return childAgent;
    }

    public Genome Brain
    {
        get { return _brain; }
        set { _brain = value; }
    }

    public float Fitness
    {
        get { return _fitness; }
        set { _fitness = value; }
    }
}
