using System.Runtime.CompilerServices;
using UnityEngine;

public class MovementScaler : MonoBehaviour
{
    private FirstPersonMovement movment;
    private Jump Jump;
    private Crouch Crouch;
    private Rigidbody Rigidbody;
    public float scale=1;
    public float jumpMultiplier=1;
    public GameObject GayObject;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        movment = GetComponent<FirstPersonMovement>();
        Jump = GetComponent<Jump>();
        Crouch = GetComponent<Crouch>();
        Rigidbody = GetComponent<Rigidbody>();
    }
        // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(scale, scale, scale);
        movment.speed = 5*scale;
        movment.runSpeed = 9*scale; 
        Jump.jumpStrength = 1+scale*jumpMultiplier;
        Crouch.movementSpeed = 2*scale;
        Rigidbody.mass = 1+scale/12;
        if(Input.GetKeyDown(KeyCode.F) )
        {
            Instantiate<GameObject>(GayObject,transform.position+ new Vector3(1,0,2),new Quaternion());
        }
    }
}
