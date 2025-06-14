using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonPress : MonoBehaviour
{
    public Vector3 pushEndDistance;
    public float sphereRadius;
    private Vector3 originalPos;
    [Range(0f, 1f)]
    public float alpha = 0.5f;
    public float sizeAmount;
    public MovementScaler scaler;

    void Start()
    {
        originalPos = transform.localPosition;
        scaler.scale = 1f;
    }

    void Update()
    {
        if (Physics.CheckSphere(transform.localPosition, sphereRadius))
        {
            //Debug.Log("Deez Nutz");
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

            transform.localPosition = pushEndDistance;
            //Debug.Log("Ligma balls");
             
            scaler.scale = scaler.scale+ sizeAmount;



        }
        else
        {
            transform.localPosition = originalPos;
            //Debug.Log("Yo Mama is so fat.");
        }
    }

    private void OnDrawGizmos()
    {
        // Set the color with custom alpha.
        Gizmos.color = new Color(1.46838839754575f, 2.1f, 0f, alpha); // Red with custom alpha

        // Draw the sphere.
        Gizmos.DrawSphere(transform.position, sphereRadius);

        // Draw wire sphere outline.
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
    }

}