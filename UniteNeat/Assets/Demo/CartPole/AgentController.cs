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
            randomAngle = Random.Range(-12f, -5f);
        else
            randomAngle = Random.Range(5f, 12f);
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
        
        GetComponent<Rigidbody>().AddForce(new Vector3(outputs[0], 0, 0));

        cartAgent.Fitness = Time.time - StartTime;

        if (Mathf.Abs(poleAngle) > 20)
        {
            cartAgent.Kill();
            _dead = true;
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            cartAgent.Kill();
            _dead = true;
            gameObject.SetActive(false);
        }
    }
}
