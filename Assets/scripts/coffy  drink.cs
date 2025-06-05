using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CoffyDrink : MonoBehaviour
{
    private TextMeshProUGUI Textu;

    private bool isInRange = false;
    public float jumpAmount;
    public MovementScaler scaler;
    //public GameObject coffeEmpty;
    private Vector3 originalpos; 
    private float originaljump;
    //public Audio
    public AudioSource coffeeDrink;
    void Start()
    {
        Textu = GameObject.Find("uii/tooltip").GetComponent<TextMeshProUGUI>();
        scaler= GameObject.Find("First Person Controller").GetComponent<MovementScaler>() ;

        

    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)&&isInRange)
        {



            i = 0;
            Invoke(nameof(Drink),0.1f);
            originalpos = transform.position;
            GetComponent<Collider>().enabled = false;

        }
        else if (isInRange)
        {
            Textu.text = "press E to drink";
            Debug.Log("tool"+isInRange+Textu.text);
        }

    }        float i ;
    private void Drink (){

        i = i+ 0.1f;
        transform.position = Vector3.Lerp(originalpos, scaler.gameObject.transform.GetChild(0).position-new Vector3(0,1,0), i);
        if (i > 1)
        {
            //Debug.Log(scaler.scale + "  " + (scaler.scale + sizeAmount));
            //scaler.scale = scaler.scale+ sizeAmount;
            scaler.jumpMultiplier = scaler.jumpMultiplier + jumpAmount;
            transform.position = originalpos;
            GetComponent<Collider>().enabled = true;
            Textu.text = "";
            coffeeDrink.Play(0);
            Destroy(this);
            
        }
        else { Invoke(nameof(Drink), 0.1f); }
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        isInRange = true;
        Textu.text = "press E to drink";
    }
    private void OnTriggerExit(Collider other)
    {
        isInRange = false;
       
            Textu.text = "";
    }
}