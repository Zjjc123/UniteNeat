using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEATController : MonoBehaviour
{
    public Population population;
    public static GameObject AgentObject;
    public int Generation = 1;

    private int INPUT = 1;
    private int OUTPUT = 1;
    private int SIZE = 100;

    // Genome

    private static float PROBABILITY_PERTURBING = 0.9f;
    private static float PERTURB_MAX = 0.02f;

    private static float WEIGHT_MUTATION_RATE = 0.8f;
    private static float NODE_MUTATION_RATE = 0.03f;
    private static float CONNECTION_MUTATION_RATE = 0.05f;

    private static float ENABLE_CHANCE = 0.25f;

    // Species

    private static float EXCESS_ADJOINT_COEFFICIENT = 1f;
    private static float WEIGHT_DIFF_COEFFICIENT = 0.4f;
    private static float COMPATIBILITY_THRESHOLD = 3f;

    private static float ONLY_MUTATION_RATE = 0.25f;

    // Population

    private static int UNIMPROVED_KILL = 15;

    private bool _initialized;

    public void MutationRate(float weight, float node, float connection)
    {
        WEIGHT_MUTATION_RATE = weight;
        NODE_MUTATION_RATE = node;
        CONNECTION_MUTATION_RATE = connection;
    }

    public void Peturbing(float perturb, float amount, float enable_chance)
    {
        PROBABILITY_PERTURBING = perturb;
        PERTURB_MAX = amount;
        ENABLE_CHANCE = enable_chance;
    }

    public void Species(float excessAdjoint, float weightDiff, float compatibilityThreshold)
    {
        EXCESS_ADJOINT_COEFFICIENT = excessAdjoint;
        WEIGHT_DIFF_COEFFICIENT = weightDiff;
        COMPATIBILITY_THRESHOLD = compatibilityThreshold;
    }

    public void Initialize(int Input, int Output, int Size)
    {
        population = new Population(Input, Output, Size, AgentObject);
    }

    private void Update()
    {
        if (!_initialized)
            return;
        if (population.Over())
        {
            NaturalSelection();
            Generation++;
        }
    }

    public void NaturalSelection()
    {
        population.NaturalSelection();
    }

}
