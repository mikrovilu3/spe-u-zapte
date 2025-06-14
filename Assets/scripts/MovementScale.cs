using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;

public class MovementScaler : MonoBehaviour
{
    private FirstPersonMovement movment;
    private Jump Jump;
    private Crouch Crouch;
    private Rigidbody Rigidbody;
    public float scale=1;
    public float jumpMultiplier=1;

    
    
    
    //public GameObject text2;
   // public TMPro.TextMeshProUGUI sizeText;
    



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        movment = GetComponent<FirstPersonMovement>();
        Jump = GetComponent<Jump>();
        Crouch = GetComponent<Crouch>();
        Rigidbody = GetComponent<Rigidbody>();
    }
   //void Start() 
   // {
     //   sizeText = text2.GetComponent<TextMeshProUGUI>();
  //  }
    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(scale, scale, scale);
        movment.speed = (2*scale)+3;
        movment.runSpeed = 9*scale; 
        Jump.jumpStrength = 2*scale*jumpMultiplier;
        Crouch.movementSpeed = 2*scale;
        Rigidbody.mass = 1+scale/12;
        
    }
}
