using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    /*
    Required:
    1. Game Controller
        - Initialize NEAT Controller
    2. NEAT Controller
        - Drag into scene
        - Assign Agent Prefab
    3. Agent Controller
        - Use Agents to propagate data
        - Move
        - Call Agents.Kill() if dead
    4. Agent
        - Drag Agent onto Agent Prefab
    5. Genome Print
        - Drag onto the same object as the NEAT Controller
    */
    NEATController c;

    public int size = 100;

    void Start()
    {
        c = GetComponent<NEATController>();
        c.Initialize(4, 2, size);
    }
}
