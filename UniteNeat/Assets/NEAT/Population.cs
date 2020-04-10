using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Population
{
    private List<Agent> _population  = new List<Agent>();
    private List<Species> _species = new List<Species>();

    // Agent Prefab
    public static GameObject AgentObject;

    public static int UNIMPROVED_KILL;

    // Initialize a population
    public Population(int input, int output, int size)
    {
        for (int i = 0; i < size; i++)
        {
            GameObject agent = GameObject.Instantiate(AgentObject, Vector3.zero, Quaternion.identity);
            _population.Add(agent.GetComponent<Agent>());
            agent.GetComponent<Agent>().Initialize(input, output);
        }
        History.SetInnovationDebug(input * output);
    }

    // Naturally Select Agents
    public void NaturalSelection()
    {
        Speciate();
        SortSpecies();
        ManipulateSpecies();
        KillUnimprovedSpecies();
        _population = GenerateOffspring(AgentObject);
        MutatePopulation();
    }

    // Place Agents into Species
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

    // Sorts the Agents within a species and the species by their fitnesses
    private void SortSpecies()
    {
        //sort the Agents within a species
        foreach (Species s in _species)
        {
            s.SortAgents();
        }

        _species.Sort(new SpeciesComparer());
    }

    // Manipulate Each Species
    private void ManipulateSpecies()
    {
        foreach(Species s in _species)
        {
            s.Cull();
            s.FitnessSharing();
        }
    }

    // Kill Unimproved Species
    private void KillUnimprovedSpecies()
    {
        for (int i = 2; i < _species.Count; i++)
        {
            if (_species[i].Unimproved >= UNIMPROVED_KILL)
            {
                _species.RemoveAt(i);
                i--;
            }
        }
    }

    // Return the sum of the adjusted fitness of all species
    private float TotalAverageFitness()
    {
        float total = 0;
        foreach (Species s in _species)
        {
            total += s.AverageFitness();
        }
        return total;
    }

    // Generate Offspring from Species
    private List<Agent> GenerateOffspring(GameObject agentObject)
    {
        List<Agent> children = new List<Agent>();

        // Copy Champion from last generation
        GameObject champGo = GameObject.Instantiate(AgentObject, Vector3.zero, Quaternion.identity);
        // Set to Red
        champGo.GetComponent<SpriteRenderer>().color = Color.red;
        // Move to front
        champGo.transform.position = new Vector3(champGo.transform.position.x, champGo.transform.position.y, champGo.transform.position.z - 0.1f);
        // Copy Brain
        champGo.GetComponent<Agent>().Brain = new Genome(_species[0].Champion);
        // Reference Agent Object
        Agent champ = champGo.GetComponent<Agent>();
        // Add Champ to children

        children.Add(champ);

        // Allocatte number of children based on the average fitness of the species
        for (int i = 0; i < _species.Count; i++)
        {
            int n = 0;
            if (TotalAverageFitness() != 0) 
                n = Mathf.FloorToInt(_species[i].AverageFitness() / TotalAverageFitness() * _population.Count) - 1;
            for (int j = 0; j < n; j++)
            {
                children.Add(_species[i].GenerateOffspring(AgentObject));
            }
        }

        // Fill up the rest with CHILDRENNN!!! from best species
        while (children.Count < _population.Count)
        {
            children.Add(_species[0].GenerateOffspring(AgentObject));
        }
        
        return children;
    }

    // Mutate All Agents (Biological Warfare basically)
    private void MutatePopulation()
    {
        foreach (Agent a in _population)
            a.Brain.Mutate();
    }

    public bool Over()
    {
        foreach (Agent a in _population)
        {
            if (a.Dead() == false)
                return false;
        }
        return true;
    }
}
