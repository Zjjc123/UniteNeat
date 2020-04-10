using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEATController : MonoBehaviour
{
    public Population population;
    public int Generation = 1;

    public void Initialize(int Input, int Output, int Size)
    {
        population = new Population(Input, Output, Size);
    }

    public void NaturalSelection()
    {
        population.NaturalSelection();
    }

    private void Update()
    {
        if (population.Over())
        {
            NaturalSelection();
            Generation++;
        }
    }

}
