using UnityEngine;
public class AirPush : MonoBehaviour
{
    public float pushForce = 10f;
    public float maxPushDistance = 5f;

    private LayerMask layerMask;

    void Awake()
    {
        layerMask = LayerMask.GetMask("PhysicsItem", "Player");
    }

    void Update()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, maxPushDistance, layerMask))
        {
            Debug.Log("FanumTax");

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 pushDirection = transform.forward;
                rb.AddForce(pushDirection * pushForce, ForceMode.Force);
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * maxPushDistance, Color.white);
        }
    }
}
