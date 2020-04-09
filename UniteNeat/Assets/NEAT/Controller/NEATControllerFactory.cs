using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEATControllerFactory
{
    private int INPUT = 1;
    private int OUTPUT = 1;
    private int SIZE = 100;

    // Genome

    public static float PROBABILITY_PERTURBING = 0.9f;
    public static float PERTURB_MAX = 0.02f;

    public static float WEIGHT_MUTATION_RATE = 0.8f;
    public static float NODE_MUTATION_RATE = 0.03f;
    public static float CONNECTION_MUTATION_RATE = 0.05f;

    public static float ENABLE_CHANCE = 0.25f;

    // Species

    public static float EXCESS_ADJOINT_COEFFICIENT = 1f;
    public static float WEIGHT_DIFF_COEFFICIENT = 0.4f;
    public static float COMPATIBILITY_THRESHOLD = 3f;

    public static float ONLY_MUTATION_RATE = 0.25f;

    // Population

    public static int UNIMPROVED_KILL = 15;

    public static GameObject AGENTOBJECT;


    public NEATControllerFactory Structure(int input, int output, int size)
    {
        INPUT = input;
        OUTPUT = output;
        SIZE = size;

        return this;
    }

    public NEATControllerFactory MutationRate(float weight, float node, float connection)
    {
        WEIGHT_MUTATION_RATE = weight;
        NODE_MUTATION_RATE = node;
        CONNECTION_MUTATION_RATE = connection;

        return this;
    }

    public NEATControllerFactory Peturbing(float perturb, float amount, float enable_chance)
    {
        PROBABILITY_PERTURBING = perturb;
        PERTURB_MAX = amount;
        ENABLE_CHANCE = enable_chance;

        return this;
    }

    public NEATControllerFactory PopulationSetting(int kill_unimprove)
    {
        UNIMPROVED_KILL = kill_unimprove;

        return this;
    }

    public NEATControllerFactory SetAgentObject(GameObject obj)
    {
        AGENTOBJECT = obj;

        return this;
    }

    public NEATController Build()
    {
        GameObject controllerObject = GameObject.Instantiate(AGENTOBJECT);

        Population population = controllerObject.GetComponent<Population>();

        population.Initialize(INPUT, OUTPUT, SIZE);

        // Genome
        Genome.PROBABILITY_PERTURBING = PROBABILITY_PERTURBING;
        Genome.PERTURB_MAX = PERTURB_MAX;

        Genome.WEIGHT_MUTATION_RATE = WEIGHT_MUTATION_RATE;
        Genome.NODE_MUTATION_RATE = NODE_MUTATION_RATE;
        Genome.CONNECTION_MUTATION_RATE = CONNECTION_MUTATION_RATE;

        Genome.ENABLE_CHANCE = ENABLE_CHANCE;

        // Species

        Species.EXCESS_ADJOINT_COEFFICIENT = EXCESS_ADJOINT_COEFFICIENT;
        Species.WEIGHT_DIFF_COEFFICIENT = WEIGHT_DIFF_COEFFICIENT;
        Species.COMPATIBILITY_THRESHOLD = COMPATIBILITY_THRESHOLD;

        Species.ONLY_MUTATION_RATE = ONLY_MUTATION_RATE;

        // Population
        Population.UNIMPROVED_KILL = UNIMPROVED_KILL;
        Population.AgentObject = AGENTOBJECT;


        return controllerObject.GetComponent<NEATController>();
    }
}
