using System.Collections;
using System.Collections.Generic;

public class AgentComparer : IComparer<Agent>
{
    // Comparing Agents based on Fitness 
    public int Compare(Agent x, Agent y)
    {
        if (x.Fitness < y.Fitness)
            return -1;
        else if (x.Fitness == y.Fitness)
            return 0;
        else
            return 1;
    }
}
