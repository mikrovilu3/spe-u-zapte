using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class BurgerEat : MonoBehaviour
{
    private TextMeshProUGUI Textt;
    private TextMeshProUGUI Textu;
    private bool isInRange = false;
    public float sizeAmount;
    public MovementScaler scaler;
    private Vector3 originalpos; 
    private float originalscale;    
    void Start()
    {
        scaler = GameObject.Find("First Person Controller").GetComponent<MovementScaler>();
        Textu = GameObject.Find("uii/tooltip").GetComponent<TextMeshProUGUI>();
        Textt = GameObject.Find("uii/Sum").GetComponent<TextMeshProUGUI>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isInRange)
        {



            i = 0;
            Invoke(nameof(EAT),0.1f);
            originalpos = transform.position;
            GetComponent<Collider>().enabled = false;

        }
        else if (isInRange)
        {
            Textu.text = "press E to eat";

        }
        else
        {
            Textu.text = "";
        }

    }
    float i ;
    private void EAT (){

        i = i+ 0.1f;
        transform.position = Vector3.Lerp(originalpos, scaler.gameObject.transform.GetChild(0).position - new Vector3(0, 1, 0), i);
        if (i > 1)
        {
            //Debug.Log(scaler.scale + "  " + (scaler.scale + sizeAmount));
            //scaler.scale = scaler.scale+ sizeAmount;
            originalscale = scaler.scale;
            i = 0;
            GetComponent<MeshRenderer>().enabled = false;
            Textt.text = "scale: " + (originalscale + sizeAmount).ToString();
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
            Textu.text = "";
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