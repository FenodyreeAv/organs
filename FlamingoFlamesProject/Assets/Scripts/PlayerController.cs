using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject heart; // Reference to the heart object
    private SpringJoint springJoint; // Reference to the spring joint

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (springJoint != null)
            {
                RemoveSpringJoint();
            }
            else
            {
                AddSpringJoint();
            }
        }
    }

    void AddSpringJoint()
    {
        if (heart != null)
        {
            springJoint = heart.AddComponent<SpringJoint>();
            springJoint.connectedBody = this.GetComponent<Rigidbody>();
            springJoint.spring = 50.0f; // Adjust the spring force as needed
            springJoint.damper = 5.0f; // Adjust the damper as needed
            springJoint.anchor = Vector3.zero; // Adjust the anchor as needed
        }
    }

    void RemoveSpringJoint()
    {
        if (springJoint != null)
        {
            Destroy(springJoint);
            springJoint = null;
        }
    }
}
