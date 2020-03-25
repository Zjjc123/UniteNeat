using System.Collections;
using System.Collections.Generic;

public class Species
{
    List<Agent> _agents = new List<Agent>();
    float bestFitness = 0;
    Genome champ;

    float excessAdjointCoeff = 1f;
    float weightDiffCoeff = 0.4f;
    float compatibilityThreshold = 3f;

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

        compatibility = (excessAdjointCoeff * excessAndDisjoint / largeGenomeNormaliser) + (weightDiffCoeff * averageWeightDiff);
        return (compatibilityThreshold > compatibility);
    }

    public void AddToSpecies(Agent a)
    {
        _agents.Add(a);

        if (a.Fitness > bestFitness)
        {
            bestFitness = a.Fitness;
        }
    }
    // Sort Species based on fitness
    public void SortSpecies()
    {
        _agents.Sort(new AgentComparer());
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
}
