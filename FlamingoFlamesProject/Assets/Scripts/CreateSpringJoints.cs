using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSpringJoints : MonoBehaviour
{
    public float spring = 50.0f;
    public float damper = 5.0f;
    public float minDistance = 0.0f;
    public float maxDistance = 0.0f;
    public bool configureConnectedAnchor = false;

    // Start is called before the first frame update
    void Start()
    {
        CreateJoints();
    }

    void CreateJoints()
    {
        Transform[] children = GetComponentsInChildren<Transform>(true);
        List<Transform> childTransforms = new List<Transform>();

        foreach (Transform child in children)
        {
            if (child.parent == transform)
            {
                childTransforms.Add(child);
            }
        }

        foreach (Transform child in childTransforms)
        {
            foreach (Transform target in childTransforms)
            {
                if (child != target)
                {
                    SpringJoint springJoint = child.gameObject.AddComponent<SpringJoint>();
                    springJoint.connectedBody = target.GetComponent<Rigidbody>();
                    springJoint.spring = spring;
                    springJoint.damper = damper;
                    springJoint.minDistance = minDistance;
                    springJoint.maxDistance = maxDistance;
                    springJoint.autoConfigureConnectedAnchor = configureConnectedAnchor;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
