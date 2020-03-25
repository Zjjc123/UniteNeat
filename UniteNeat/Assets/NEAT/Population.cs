using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Population : MonoBehaviour
{
    private List<Agent> _population  = new List<Agent>();
    private List<Species> _species = new List<Species>();

    // Agent Prefab
    [SerializeField]
    public GameObject AgentObject;

    public void Initialize(int input, int output, int size)
    {
        for (int i = 0; i < size; i++)
        {
            GameObject agent = Instantiate(AgentObject, Vector3.zero, Quaternion.identity);
            _population.Add(agent.GetComponent<Agent>());
            agent.GetComponent<Agent>().Initialize(input, output);
        }

        History.SetInnovationDebug(input * output);
    }

}
