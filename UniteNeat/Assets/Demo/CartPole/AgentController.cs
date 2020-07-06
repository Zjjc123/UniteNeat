using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    [SerializeField]
    Rigidbody cartRB;

    [SerializeField]
    HingeJoint poleJoint;

    Agent cartAgent;

    float cartPosition;
    float cartVelocity;
    float poleAngle;
    float poleRotationalVelocity;

    bool _dead;

    List<float> inputs;
    List<float> outputs;

    float StartTime;

    private void Start()
    {
        cartAgent = GetComponent<Agent>();
        inputs = new List<float>();
        outputs = new List<float>();
        StartTime = Time.time;

        float randomAngle;

        if (Random.Range(0, 2) > 0)
            randomAngle = Random.Range(-40, -10);
        else
            randomAngle = Random.Range(10, 40);
        poleJoint.transform.eulerAngles = new Vector3(0, 0, randomAngle);
    }

    void Update()
    {
        if (!cartAgent.Initialized)
            return;

        if (_dead)
            return;
                    
        inputs.Clear();
        outputs.Clear();

        cartPosition = transform.position.x;
        cartVelocity = cartRB.velocity.x;
        poleAngle = poleJoint.angle;
        poleRotationalVelocity = poleJoint.velocity;

        inputs.Add(cartPosition);
        inputs.Add(cartVelocity);
        inputs.Add(poleAngle);
        inputs.Add(poleRotationalVelocity);

        outputs = cartAgent.ForwardPropagate(inputs);
        
        GetComponent<Rigidbody>().AddForce(new Vector3(outputs[1] - outputs[0] * 10, 0, 0));

        cartAgent.Fitness = Time.time - StartTime;

        if (Mathf.Abs(poleAngle) > 90)
        {
            cartAgent.Kill();
            _dead = true;
        }
    }
}
