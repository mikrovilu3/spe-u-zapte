using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoffyDrink : MonoBehaviour
{

    private bool isInRange = false;
    public float jumpAmount;
    public MovementScaler scaler;
    private Vector3 originalpos; 
    private float originaljump;    
    void Start()
    {
        scaler = GameObject.Find("First Person Controller").GetComponent<MovementScaler>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)&&isInRange)
        {



            i = 0;
            Invoke(nameof(EAT),0.1f);
            originalpos = transform.position;
            GetComponent<Collider>().enabled = false;

        }
        else if (isInRange)
        {


        }
    }        float i ;
    private void EAT (){

        i = i+ 0.1f;
        transform.position = Vector3.Lerp(originalpos, scaler.gameObject.transform.GetChild(0).position-new Vector3(0,1,0), i);
        if (i > 1)
        {
            //Debug.Log(scaler.scale + "  " + (scaler.scale + sizeAmount));
            //scaler.scale = scaler.scale+ sizeAmount;
            originaljump = scaler.jumpMultiplier;
            i = 0;
            GetComponent<MeshRenderer>().enabled = false;
            Invoke(nameof(GROW), 0.1f);
        }
        else { Invoke(nameof(EAT), 0.1f); }
        
    }
    private void GROW()
    {
        i = i+ 0.1f;
        scaler.jumpMultiplier =Mathf.Lerp(originaljump,originaljump +jumpAmount,i);
        Debug.Log("sogme boi"+ scaler.jumpMultiplier +" "+ Mathf.Lerp(originaljump, originaljump + jumpAmount, i));
        if (i > 1)
        {
            Destroy(gameObject);
        }
        else { Invoke(nameof(GROW), 0.1f); }
    }
    private void OnTriggerEnter(Collider other)
    {
        isInRange = true;
    }
    private void OnTriggerExit(Collider other)
    {
        isInRange = false;
    }
}