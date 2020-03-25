using System.Collections;
using System.Collections.Generic;

public class SpeciesComparer : IComparer<Species>
{
    // Comparing Agents based on Fitness 
    public int Compare(Species x, Species y)
    {
        if (x.BestFitness < y.BestFitness)
            return -1;
        else if (x.BestFitness == y.BestFitness)
            return 0;
        else
            return 1;
    }
}
