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

    List<float> inputs;
    List<float> outputs;

    private void Start()
    {
        cartAgent = GetComponent<Agent>();
        inputs = new List<float>();
        outputs = new List<float>();
    }

    void Update()
    {
        if (!cartAgent.Initialized)
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
        
        cartRB.AddForce(new Vector3(outputs[1] - outputs[0], 0));

        if (Mathf.Abs(poleAngle) > 90)
            cartAgent.Kill();
    }
}
