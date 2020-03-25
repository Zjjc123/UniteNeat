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

    // Initialize a population
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

    // Naturally Select Agents
    public void NaturalSelection()
    {
        
    }

    // Sort Agents into species
    private void Speciate()
    {
        foreach (Species s in _species)
            s.Clear();
        foreach(Agent a in _population)
        {
            bool found = false;
            // If should be in the same species then add to species
            foreach (Species s in _species)
            {
                if (s.SameSpecies(a.Brain))
                {
                    found = true;
                    s.AddToSpecies(a);
                    break;
                }
            }
            // If does not fit into any species make a new species
            if (!found)
            {
                _species.Add(new Species(a));
            }
        }
    }
    //sorts the Agents within a species and the species by their fitnesses
    void SortSpecies()
    {
        //sort the Agents within a species
        foreach (Species s in _species)
        {
            s.SortAgents();
        }

        _species.Sort(new SpeciesComparer());
    }
}
