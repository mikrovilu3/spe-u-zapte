using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BurgerEat : MonoBehaviour
{


    public float sizeAmount;
    public MovementScaler scaler;
    private Vector3 originalpos; 
    private float originalscale;    
    void Start()
    {
        scaler.scale = 1f;
    }


    private void OnTriggerStay(Collider other)
    {
     
            i = 0;
            Invoke(nameof(EAT),0.1f);
            originalpos = transform.position;
            GetComponent<Collider>().enabled = false;

    }        float i ;
    private void EAT (){

        i = i+ 0.1f;
        transform.position = Vector3.Lerp(originalpos, scaler.gameObject.transform.GetChild(0).position, i);
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
        scaler.scale =Mathf.Lerp(originalscale,originalscale+sizeAmount,i);
        if (i > 1)
        {
            Destroy(gameObject);
        }
        else { Invoke(nameof(GROW), 0.1f); }
    }
}