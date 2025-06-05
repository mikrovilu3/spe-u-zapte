using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CoffyDrink : MonoBehaviour
{
    private tooltipwarden Textuw;

    private bool isInRange = false;
    public float jumpAmount;
    public MovementScaler scaler;
    private Vector3 originalpos; 
    private float originaljump;    
    void Start()
    {
        Textuw = GameObject.Find("uii/tooltip").GetComponent<tooltipwarden>();
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
            Textuw.avalablecoffy =- 1;
            Destroy(this);
            
        }
        else { Invoke(nameof(Drink), 0.1f); }
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        isInRange = true;
        Textuw.avalablecoffy ++;
    }
    private void OnTriggerExit(Collider other)
    {
        isInRange = false;
        Textuw.avalablecoffy --;
    }
}