using UnityEngine;
public class AirPush : MonoBehaviour
{
    public Rigidbody m_Rigidbody;
    public float m_Thrust = 20f;
    

    private LayerMask layerMask;

    void Awake()
    {
        layerMask = LayerMask.GetMask("PhysicsItem", "Player");
    }
    void Start() 
    {
        


    }

    private void OnTriggerStay(Collider other)
    {
        //m_Rigidbody.AddForce(transform.up * m_Thrust);
        other.GetComponent<Rigidbody>().AddForce(transform.up * m_Thrust);
    }
}
