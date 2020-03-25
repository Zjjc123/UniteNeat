using System.Collections;
using System.Collections.Generic;

public class Species
{
    List<Agent> _agents = new List<Agent>();
    private float bestFitness = 0;
    private Genome champ;

    private float EXCESS_ADJOINT_COEFFICIENT = 1f;
    private float WEIGHT_DIFF_COEFFICIENT = 0.4f;
    private float COMPATIBILITY_THRESHOLD = 3f;

    private int UNIMPROVEMENT_MASSACRE = 15;

    private int _unimproved = 0;

    public Species(Agent a)
    {
        bestFitness = a.Fitness;
        champ = new Genome(a.Brain);
    }

    // Calculate if the new genome is in the same species or not
    public bool SameSpecies(Genome g)
    {
        float compatibility;
        float excessAndDisjoint = Genome.GetExcessDisjoint(g, champ);
        float averageWeightDiff = Genome.GetWeightDifferenceAverage(g, champ);


        float largeGenomeNormaliser = g.Connections.Count - 20;
        if (largeGenomeNormaliser < 1)
        {
            largeGenomeNormaliser = 1;
        }

        compatibility = (EXCESS_ADJOINT_COEFFICIENT * excessAndDisjoint / largeGenomeNormaliser) + (WEIGHT_DIFF_COEFFICIENT * averageWeightDiff);
        return (COMPATIBILITY_THRESHOLD > compatibility);
    }

    // Add new genome to species and update fitness if possible
    public void AddToSpecies(Agent a)
    {
        _agents.Add(a);

        if (a.Fitness > bestFitness)
        {
            bestFitness = a.Fitness;
        }
    }
    // Sort Species based on fitness
    // Add to unimprovement
    public void SortSpecies()
    {
        _agents.Sort(new AgentComparer());

        if (_agents[0].Fitness <= bestFitness)
            _unimproved++;
    }

    // Kill bottom half of species
    public void Cull()
    {
        if (_agents.Count > 2)
        {
            for (int i = _agents.Count / 2; i < _agents.Count; i++)
            {
                _agents.RemoveAt(i);
                i--;
            }
        }
    }

    // Share fitness based on species size
    // No need to compare because they are already in a species
    public void FitnessSharing()
    {
        for (int i = 0; i < _agents.Count; i++)
        {
            _agents[i].Fitness = _agents[i].Fitness / _agents.Count;
        }
    }

    // Get Unimprovement
    public int Unimproved
    {
        get { return _unimproved; }
    }

}
