using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoffyDrink : MonoBehaviour
{


    public float jumpAmount;
    public MovementScaler scaler;
    private Vector3 originalpos; 
    private float originalscale;    
    void Start()
    {
        scaler = GameObject.Find("First Person Controller").GetComponent<MovementScaler>();
    }


    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {



            i = 0;
            Invoke(nameof(EAT),0.1f);
            originalpos = transform.position;
            GetComponent<Collider>().enabled = false;

        }
        else
        {


        }
    }        float i ;
    private void EAT (){

        i = i+ 0.1f;
        transform.position = Vector3.Lerp(originalpos, scaler.gameObject.transform.GetChild(0).position-new Vector3(0,-1,0), i);
        if (i > 1)
        {
            //Debug.Log(scaler.scale + "  " + (scaler.scale + sizeAmount));
            //scaler.scale = scaler.scale+ sizeAmount;
            originalscale = scaler.scale;
            i = 0;
            GetComponent<MeshRenderer>().enabled = false;
            Invoke(nameof(GROW), 0.1f);
        }
        else { Invoke(nameof(EAT), 0.1f); }
        
    }
    private void GROW()
    {
        i = i+ 0.1f;
        scaler.jumpMultiplier =Mathf.Lerp(originalscale,originalscale+jumpAmount,i);
        if (i > 1)
        {
            Destroy(gameObject);
        }
        else { Invoke(nameof(GROW), 0.1f); }
    }
}