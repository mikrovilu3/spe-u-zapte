using UnityEngine;
public class AirPush : MonoBehaviour
{
    public Rigidbody m_Rigidbody;
    public float m_Thrust = 20f;
    public float maxSize = 100;
    

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
        m_Rigidbody = other.GetComponent<Rigidbody>();
        if (m_Rigidbody != null)
        {
            if (m_Rigidbody.name == "First Person Controller")
            {
                if (m_Rigidbody.GetComponent<MovementScaler>() != null)
                {
                    if(m_Rigidbody.GetComponent <MovementScaler>().scale < maxSize)
                    {
                        m_Rigidbody.AddForce( -transform.up * m_Thrust);
                    }
                }
            }
            else {m_Rigidbody.AddForce( -transform.up * m_Thrust); }
        }
    }
}
