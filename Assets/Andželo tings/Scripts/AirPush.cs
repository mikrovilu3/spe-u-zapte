using UnityEngine;

public class AirPush : MonoBehaviour
{
    LayerMask layerMask = LayerMask.GetMask("PhysicsItem", "Player");
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {

            transform.localPosition = pushAmount;
            Debug.Log("Skibidi");

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

        }
        else
        {
            transform.localPosition = originalPos;
            Debug.Log("Yo Mama is so Skinny.");

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        }
    }
}
