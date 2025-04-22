using UnityEngine;

public class SimpleIL : MonoBehaviour
{
    public Transform pivot, upper, lower, effector, tip;
    public Vector3 target;
    public Transform targetTransform;
    public Vector3 normal = Vector3.up;

    public float speed = 5.0f;

    float upperLength, lowerLength, effectorLength, pivotLength;
    Vector3 effectorTarget, tipTarget;

    private SpringJoint springJoint;

    void Reset()
    {
        pivot = transform;
        try
        {
            upper = pivot.GetChild(0);
            lower = upper.GetChild(0);
            effector = lower.GetChild(0);
            tip = effector.GetChild(0);
        }
        catch (UnityException)
        {
            Debug.Log("Could not find required transforms, please assign manually.");
        }
    }

    void Awake()
    {
        upperLength = (lower.position - upper.position).magnitude;
        lowerLength = (effector.position - lower.position).magnitude;
        effectorLength = (tip.position - effector.position).magnitude;
        pivotLength = (upper.position - pivot.position).magnitude;
        target = targetTransform.position;
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        target += movement * speed * Time.deltaTime;

        tipTarget = target;
        effectorTarget = target + normal * effectorLength;
        Solve();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryAttachOrgan();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            DetachOrgan();
        }
    }

    void Solve()
    {
        var pivotDir = effectorTarget - pivot.position;
        pivot.rotation = Quaternion.LookRotation(pivotDir);

        var upperToTarget = (effectorTarget - upper.position);
        var a = upperLength;
        var b = lowerLength;
        var c = upperToTarget.magnitude;

        var B = Mathf.Acos((c * c + a * a - b * b) / (2 * c * a)) * Mathf.Rad2Deg;
        var C = Mathf.Acos((a * a + b * b - c * c) / (2 * a * b)) * Mathf.Rad2Deg;

        if (!float.IsNaN(C))
        {
            var upperRotation = Quaternion.AngleAxis((-B), Vector3.right);
            upper.localRotation = upperRotation;
            var lowerRotation = Quaternion.AngleAxis(180 - C, Vector3.right);
            lower.localRotation = lowerRotation;
        }
        var effectorRotation = Quaternion.LookRotation(tipTarget - effector.position);
        effector.rotation = effectorRotation;
    }

    void TryAttachOrgan()
    {
        Collider[] colliders = Physics.OverlapSphere(tip.position, 0.1f);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Organ"))
            {
                springJoint = tip.gameObject.AddComponent<SpringJoint>();
                springJoint.connectedBody = collider.attachedRigidbody;
                springJoint.spring = 10000.0f;
                springJoint.damper = 1.0f;
                springJoint.minDistance = 0.0f;
                springJoint.maxDistance = 0.0f;
                break;
            }
        }
    }

    void DetachOrgan()
    {
        if (springJoint != null)
        {
            Destroy(springJoint);
            springJoint = null;
        }
    }
}
